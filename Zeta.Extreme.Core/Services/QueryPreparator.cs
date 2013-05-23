#region LICENSE
// Copyright 2007-2013 Qorpent Team - http://github.com/Qorpent
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// PROJECT ORIGIN: Zeta.Extreme.Core/QueryProcessor.cs
#endregion
using System;
using System.Linq;
using System.Threading;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.Extensions;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme {
	/// <summary>
	/// 	Стандартная реализация подготовщика запросов
	/// </summary>
	/// <remarks>
	/// 	Подготовщик работает в ситуации уверенности
	/// 	в том, что запрос полностью нормализован,
	/// 	уникален и прописан во всех требуемых списках
	/// </remarks>
	public class QueryPreparator : IQueryPreparator {
		/// <summary>
		/// 	Основной конструктор, связывает с сессией
		/// </summary>
		/// <param name="session"> </param>
		public QueryPreparator(Session session) {
			_session = session;
			_sumh = new StrongSumProvider();
		}

		/// <summary>
		/// 	Выполняет подготовку запроса к выполнению
		/// 	выполняется после препроцессора и проверок
		/// </summary>
		/// <param name="query"> </param>
		public void Prepare(IQuery query) {
			var processableQuery = query as IQueryWithProcessing;
			if(null==processableQuery)return;
			if (processableQuery.PrepareState == PrepareState.InPrepare || processableQuery.PrepareState == PrepareState.Prepared)
			{
				return;
			}
			processableQuery.PrepareState = PrepareState.InPrepare;

			if (processableQuery.IsPrimary)
			{
				RegisterPrimaryRequest(processableQuery);
			}
			else {
				var mostpriority = GetMostPriorityNoPrimarySource(processableQuery);
				if (_sumh.IsSum(mostpriority)) {
					
					ExpandSum(processableQuery, mostpriority);
				}
				else {
					PrepareFormulas(processableQuery, mostpriority);
				}
			}
			processableQuery.PrepareState = PrepareState.Prepared;
		}

		private IZetaQueryDimension GetMostPriorityNoPrimarySource(IQuery query) {
			if (query.Obj.IsFormula || (query.Obj.IsForObj && _sumh.IsSum(query.Obj))) {
				return (query.Obj.ObjRef) ?? (IZetaQueryDimension) query.Obj;
			}
			if ((query.Row.IsFormula || _sumh.IsSum(query.Row.Native)) && !query.Row.IsPrimary()) {
				if (query.Col.IsFormula && query.Col.Native != null)
				{
					if (query.Col.Native.IsMarkSeted("DOSUM") && TagHelper.Value(query.Row.Tag, "nodosum") != "1")
					{
						return query.Row.Native ?? (IZetaQueryDimension) query.Row;
					}
				}
			}
			if (query.Col.IsFormula || _sumh.IsSum(query.Col.Native)) {
				return query.Col.Native ?? (IZetaQueryDimension) query.Col;
			}
			return query.Row.Native ?? (IZetaQueryDimension) query.Row;
		}
		IFormulaStorage Formulas
		{
			get
			{
				if (_session is ISessionWithExtendedServices)
				{
					return (_session as ISessionWithExtendedServices).FormulaStorage ?? FormulaStorage.Default;
				}
				return FormulaStorage.Default;
			}
		}


		private void PrepareFormulas(IQueryWithProcessing query, IZetaQueryDimension mostpriority) {
			query.EvaluationType = QueryEvaluationType.Formula;
			_session.StatIncQueryTypeFormula();
			var key = GetKey(mostpriority);
			var formula = Formulas.GetFormula(key, false);
			if (null == formula) {
				Formulas.Register(new FormulaRequest
					{Formula = mostpriority.Formula, Language = mostpriority.FormulaType, Key = key});
				formula = Formulas.GetFormula(key, false);
			}

			if (null == formula) {
				query.Result = new QueryResult {IsComplete = true, Error = new Exception("formula not found")};
				return;
			}
			query.AssignedFormula = formula;
			query.AssignedFormulaKey = key;
			formula.Playback(query);
		}

		private static string GetKey(IZetaQueryDimension mostpriority) {
			var key = mostpriority.Code ?? Guid.NewGuid().ToString();
			if (mostpriority is RowHandler || mostpriority is IZetaRow) {
				key = "row:" + key;
			}
			else if (mostpriority is ColumnHandler || mostpriority is IZetaColumn) {
				if (mostpriority is ColumnHandler && null == ((ColumnHandler) mostpriority).Native) {
					key = "dyncol:" + mostpriority.Formula;
				}
				else {
					key = "col:" + key;
				}
			}
			else if (mostpriority is ObjHandler || mostpriority is IZetaMainObject) {
				key = "obj:" + key;
			}
			return key;
		}

		private void ExpandSum(IQuery query, IZetaQueryDimension mostpriority) {
			if (IsNoCalcSum(query)) {
					query.Result = new QueryResult{IsComplete = true,NumericResult = 0};
				return;
			}

			var processablequery = query as IQueryWithProcessing;
			if(null==processablequery)return;
			processablequery.EvaluationType = QueryEvaluationType.Summa;
			_session.StatIncQueryTypeSum();
			foreach (var r in _sumh.GetSumDelta(mostpriority)) {
				var sq = r.Apply(processablequery);
				sq = _session.Register(sq);
				if (null == sq) {
					continue;
				}
				processablequery.SummaDependency.Add(new Tuple<decimal, IQuery>(r.Multiplicator, sq));
			}

			if (processablequery.SummaDependency.Count == 0) {
				processablequery.Result = new QueryResult {IsComplete = true, NumericResult = 0m};
			}
		}

		private static bool IsNoCalcSum(IQuery query) {
			return query.Col.Native != null && query.Row.Native != null && query.Col.Native.IsMarkSeted("NOCALCSUM") &&
			       query.Row.Native.IsMarkSeted("0SA") && !query.Row.Native.IsMarkSeted("CALCSUM");
		}

		/// <summary>
		/// 	Регистрирует первичный запрос
		/// </summary>
		/// <param name="query"> </param>
		protected virtual void RegisterPrimaryRequest(IQuery query) {
			_session.StatIncQueryTypePrimary();
			_session.GetPrimarySource().Register(query);
		}

		private readonly ISession _session;
		private readonly StrongSumProvider _sumh;
	}
}
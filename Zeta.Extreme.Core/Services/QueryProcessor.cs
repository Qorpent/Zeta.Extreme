#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : QueryProcessor.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Threading;
using Comdiv.Extensions;
using Comdiv.Zeta.Model;
using Comdiv.Zeta.Model.ExtremeSupport;
using Qorpent.Utils.Extensions;

namespace Zeta.Extreme {
	/// <summary>
	/// 	Стандартная реализация подготовщика запросов
	/// </summary>
	/// <remarks>
	/// 	Подготовщик работает в ситуации уверенности
	/// 	в том, что запрос полностью нормализован,
	/// 	уникален и прописан во всех требуемых списках
	/// </remarks>
	public class QueryProcessor : IQueryPreparator {
		/// <summary>
		/// 	Основной конструктор, связывает с сессией
		/// </summary>
		/// <param name="session"> </param>
		public QueryProcessor(Session session) {
			_session = session;
			_stat = _session.CollectStatistics;
			_sumh = new StrongSumProvider();
		}

		/// <summary>
		/// 	Выполняет подготовку запроса к выполнению
		/// 	выполняется после препроцессора и проверок
		/// </summary>
		/// <param name="query"> </param>
		public void Prepare(Query query) {
			if(query.PrepareState==PrepareState.InPrepare|| query.PrepareState==PrepareState.Prepared)return;
			query.PrepareState = PrepareState.InPrepare;
			
			if (query.IsPrimary) {
				RegisterPrimaryRequest(query);
			}
			else {
				var mostpriority = GetMostPriorityNoPrimarySource(query);
				if (_sumh.IsSum(mostpriority)) {
					ExpandSum(query, mostpriority);
				}
				else {
					PrepareFormulas(query, mostpriority);
				}
			}
			query.PrepareState = PrepareState.Prepared;
		}

		private IZetaQueryDimension GetMostPriorityNoPrimarySource(Query query) {
			if (query.Obj.IsFormula || (query.Obj.IsForObj && _sumh.IsSum(query.Obj))) {
				return (query.Obj.ObjRef) ?? (IZetaQueryDimension) query.Obj;
			}
			if(query.Row.IsFormula || _sumh.IsSum(query.Row.Native)) {
				if(query.Col.IsFormula && query.Col.Native!=null) {
					if(query.Col.Native.IsMarkSeted("DOSUM")) {
						return query.Row.Native ??(IZetaQueryDimension) query.Row;
					}
				}
			}
			if (query.Col.IsFormula || _sumh.IsSum(query.Col.Native)) {
				return query.Col.Native ?? (IZetaQueryDimension) query.Col;
			}
			return query.Row.Native ?? (IZetaQueryDimension) query.Row;
		}

		private void PrepareFormulas(Query query, IZetaQueryDimension mostpriority) {
			query.EvaluationType = QueryEvaluationType.Formula;

			if (_stat) {
				Interlocked.Increment(ref _session.Stat_QueryType_Formula);
			}
			var key = GetKey(mostpriority);
			var formula = FormulaStorage.Default.GetFormula(key, false);
			if (null == formula) {
				FormulaStorage.Default.Register(new FormulaRequest
					{Formula = mostpriority.Formula, Language = mostpriority.FormulaEvaluator, Key = key});
				formula = FormulaStorage.Default.GetFormula(key, false);
			}

			if (null == formula) {
				query.Result = new QueryResult {IsComplete = true, Error = new Exception("formula not found")};
				return;
			}
			query.AssignedFormula = formula;
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

		private void ExpandSum(Query query, IZetaQueryDimension mostpriority) {
			query.EvaluationType = QueryEvaluationType.Summa;

			if (_stat) {
				Interlocked.Increment(ref _session.Stat_QueryType_Sum);
			}

			foreach (var r in _sumh.GetSumDelta(mostpriority)) {
				var sq = r.Apply(query);
				sq = (Query)_session.Register(sq);
				if (null == sq) {
					continue;
				}
				query.SummaDependency.Add(new Tuple<decimal, IQueryWithProcessing>(r.Multiplicator, sq));
			}

			if (query.SummaDependency.Count == 0) {
				query.Result = new QueryResult {IsComplete = true, NumericResult = 0m};
				return;
			}
		}

		/// <summary>
		/// 	Регистрирует первичный запрос
		/// </summary>
		/// <param name="query"> </param>
		protected virtual void RegisterPrimaryRequest(Query query) {
			if (_stat) {
				Interlocked.Increment(ref _session.Stat_QueryType_Primary);
			}
			_session.PrimarySource.Register(query);
			if (_session.TraceQuery) {
				query.TraceList.Add(_session.Id + " registered to primary ");
			}
		}

		private readonly Session _session;
		private readonly bool _stat;
		private readonly StrongSumProvider _sumh;
	}
}
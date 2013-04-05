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
// PROJECT ORIGIN: Zeta.Extreme.Core/Query.cs
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Zeta.Extreme.Model.Extensions;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme {
	/// <summary>
	/// 	Инкапсуляция запроса к Zeta
	/// </summary>
	/// <remarks>
	/// 	В обновленной версии не используется избыточных
	/// 	интерфейсов IQuery, IQueryBuilder, наоборот ZexQuery
	/// 	создан с учетом оптимизации и минимальной мутации
	/// </remarks>
	public sealed class Query : CacheKeyGeneratorBase, IQueryWithProcessing {
		/// <summary>
		/// 	Конструктор запроса по умолчанию
		/// </summary>
		public Query() {
			Time = new TimeHandler();
			Row = new RowHandler();
			Col = new ColumnHandler();
			Obj = new ObjHandler();
			Reference = new ReferenceHandler();
			Currency = "NONE";
		}
		/// <summary>
		/// Простой конструктор типовых запросов
		/// </summary>
		/// <param name="rowcode"></param>
		/// <param name="colcode"></param>
		/// <param name="obj"></param>
		/// <param name="year"></param>
		/// <param name="period"></param>
		public Query(string rowcode, string colcode, int obj, int year, int period):this() {
			Row.Code = rowcode;
			if (!Regex.IsMatch(rowcode, @"^[\w\d]+$")) {
				Row.Code = Convert.ToBase64String(Encoding.UTF8.GetBytes(rowcode));
				Row.IsFormula = true;
				Row.Formula = rowcode;
				Row.FormulaType = "boo";
			}
			Col.Code = colcode;
			if (!Regex.IsMatch(colcode, @"^[\w\d]+$"))
			{
				Col.Code = Convert.ToBase64String(Encoding.UTF8.GetBytes(colcode));
				Col.IsFormula = true;
				Col.Formula = rowcode;
				Col.FormulaType = "boo";
			}
			Obj.Id = obj;
			Time.Year = year;
			Time.Period = period;
		}

		/// <summary>
		/// 	Дочерние запросы
		/// </summary>
		public IList<IQuery> FormulaDependency {
			get { return _formulaDependency ?? (_formulaDependency = new List<IQuery>()); }
		}

		/// <summary>
		/// 	Проверяет "первичность запроса"
		/// </summary>
		public bool IsPrimary {
			get { return Obj.IsPrimary() && Col.IsPrimary() && Row.IsPrimary(); }
		}


		/// <summary>
		/// 	Синхронный результат
		/// </summary>
		public QueryResult Result { get; set; }


		/// <summary>
		/// 	Back-reference to preparation tasks
		/// </summary>
		public Task PrepareTask { get; set; }

		private bool? _isrecycle;

		/// <summary>
		/// Проверяет, является ли запрос циклическим
		/// </summary>
		public bool GetIsRecycle(IDictionary<long, bool> dictionary=null)
		{
			if (_isrecycle.HasValue) return _isrecycle.Value;
			return (_isrecycle = CheckRecycle(dictionary??new Dictionary<long,bool> ())).Value;
		}

		private bool CheckRecycle(IDictionary<long, bool> dictionary) {
			if (EvaluationType == QueryEvaluationType.Primary || EvaluationType == QueryEvaluationType.Unknown) {
				return false;
			}
			if (dictionary.ContainsKey(Uid)) return true;
			dictionary[Uid] = true;
			if (EvaluationType == QueryEvaluationType.Formula) {
				foreach (var d in FormulaDependency.OfType<IQueryWithProcessing>()) {
					if (d.GetIsRecycle(dictionary)) return true;
				}
			}else if (EvaluationType == QueryEvaluationType.Summa) {
				foreach (var sd in SummaDependency.Select(_=>_.Item2).OfType<IQueryWithProcessing>()) {
					if (sd.GetIsRecycle(dictionary)) return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 	Client processed mark
		/// </summary>
		public bool Processed { get; set; }

		/// <summary>
		/// 	Зависимости для суммовых запросов
		/// </summary>
		public IList<Tuple<decimal, IQuery>> SummaDependency {
			get { return _summaDependency ?? (_summaDependency = new List<Tuple<decimal, IQuery>>()); }
		}

		/// <summary>
		/// 	Условие на время
		/// </summary>
		public ITimeHandler Time { get; set; }

		/// <summary>
		/// 	Условие на строку
		/// </summary>
		public IRowHandler Row { get; set; }

		/// <summary>
		///  Измерение по контрагенту
		/// </summary>
		public IReferenceHandler Reference { get; set; }

		/// <summary>
		/// 	Условие на колонку
		/// </summary>
		public IColumnHandler Col { get; set; }

		/// <summary>
		/// 	Условие на объект
		/// </summary>
		public IObjHandler Obj { get; set; }

		/// <summary>
		/// 	Выходная валюта
		/// </summary>
		public string Currency { get; set; }

		/// <summary>
		/// 	Сбрасывает кэш-строку
		/// </summary>
		public override void InvalidateCacheKey() {
			base.InvalidateCacheKey();
			Row.InvalidateCacheKey();
			Col.InvalidateCacheKey();
			Time.InvalidateCacheKey();
			Obj.InvalidateCacheKey();
		}

		/// <summary>
		/// 	Обеспечивает возврат результата запроса
		/// </summary>
		/// <param name="timeout"> </param>
		/// <returns> </returns>
		/// <exception cref="Exception"></exception>
		public QueryResult GetResult(int timeout = -1) {
			lock (this) {
				WaitPrepare();
				
				if (null != Result) {
					if (null != Result.Error) {
						if (!(Result.Error is QueryException)) {
							Result.Error = new QueryException(this,Result.Error);
						}
					}
					return Result;
				}

				if (GetIsRecycle()) {
					Result = new QueryResult{IsComplete = false, Error = new Exception("circular dependency")};
				}
				
				if (EvaluationType == QueryEvaluationType.Summa && null == Result) {
					return GetSummaResult();
				}

				if (EvaluationType == QueryEvaluationType.Formula && null == Result) {
					return GetFormulaResult();
				}
				WaitResult(timeout);
				if (null != Result) {
					return Result;
				}
				return Result;
			}
		}

		private QueryResult GetFormulaResult() {
			AssignedFormula.Init(this);
			try {
				Result = AssignedFormula.Eval();
			}
			finally {
				AssignedFormula.CleanUp();
				//FormulaStorage.Default.Return(key, formula);
			}
			return Result;
		}

		private QueryResult GetSummaResult() {
			
			var subresults = SummaDependency.Select(sq => new {sq, val = sq.Item2.GetResult()}).ToArray();
			var fsterror = subresults.FirstOrDefault(_ => null != _.val.Error);
			if (null != fsterror) {		
				Result = new QueryResult {IsComplete = false, Error = new QueryException(this, fsterror.val.Error)};
			}
			else {
				var result =
					(subresults
						.Where(@t => null != @t.val)
						.Select(@t => @t.val.NumericResult*@t.sq.Item1)).
						Sum();
				Result = new QueryResult {IsComplete = true, NumericResult = result};
			}
			return Result;
		}


		/// <summary>
		/// 	Позволяет синхронизировать запросы в подсессиях
		/// </summary>
		/// <param name="timeout"> </param>
		public void WaitPrepare(int timeout = -1) {
			while (PrepareState.Prepared != PrepareState) {
				if (PrepareTask != null) {
					if (!PrepareTask.IsCompleted) {
						PrepareTask.Wait();
					}
				}
				else {
					Thread.Sleep(20);
				}
			}
			PrepareTask = null;
		}


		/// <summary>
		/// 	Функция непосредственного вычисления кэшевой строки
		/// </summary>
		/// <returns> </returns>
		protected override string EvalCacheKey() {
			var sb = new StringBuilder();

			if (null != CustomHashPrefix) {
				sb.Append('/');
				sb.Append(CustomHashPrefix);
			}
			sb.Append('/');
			sb.Append(null == Obj ? "NOOBJ" : Obj.GetCacheKey());
			sb.Append('/');
			sb.Append(null == Row ? "NOROW" : Row.GetCacheKey());
			sb.Append('/');
			sb.Append(null == Col ? "NOCOL" : Col.GetCacheKey());
			sb.Append('/');
			sb.Append(null == Time ? "NOTIME" : Time.GetCacheKey());
			sb.Append('/');
			sb.Append(string.IsNullOrWhiteSpace(Currency) ? "NOVAL" : "VAL:" + Currency);
			sb.Append('/');
			sb.Append(Reference.GetCacheKey());
			return sb.ToString();
		}

		/// <summary>
		/// 	Простая копия условия на время
		/// </summary>
		/// <param name="deep"> Если да, то делает копии вложенных измерений </param>
		/// <returns> </returns>
		public IQuery Copy(bool deep = false) {
			var result = (Query) MemberwiseClone();
			result.PrepareTask = null;
			result.Result = null;
			result.EvaluationType = QueryEvaluationType.Unknown;
			result._summaDependency = null;
			result._formulaDependency = null;
			result.AssignedFormula = null;
			result.PrepareState = PrepareState.None;

			if (null != TraceList) {
				result.TraceList = new List<string>();
			}
			if (deep) {
				result.Col = result.Col.Copy();
				result.Row = result.Row.Copy();
				result.Time = result.Time.Copy();
				result.Obj = result.Obj.Copy();
				result.Reference = result.Reference.Copy();
			}

			return result;
		}


		/// <summary>
		/// 	Стандартная процедура нормализации
		/// </summary>
		public void Normalize(ISession session = null) {
			Session = session;
			var objt = Task.Run(() => Obj.Normalize(this)); //объекты зачастую из БД догружаются
			Time.Normalize(this);
			Col.Normalize(this);
			ResolveTemporalCustomCodeBasedColumns(session);
			Row.Normalize(this); //тут формулы парсим простые как рефы			
			objt.Wait();
			AdaptDetailModeForDetailBasedSubtrees();
			AdaptExRefLinkSourceForColumns(session);
			Reference.Normalize(this);
			InvalidateCacheKey();
		}

		private void AdaptExRefLinkSourceForColumns(ISession session) {
			if (null != Col.Native  && null != Row.Native && null!=Col.Tag && null!=Row.Tag) {
				if (Col.Tag.Contains("/linkcol")) {
					var resolvedCode = Row.Native.GetRedirectColCode(Col.Native);
					if (resolvedCode != Col.Code) {
						Col.Native = (session ?? Session).GetMetaCache().Get<IZetaColumn>(resolvedCode);
					}
				}
			}
		}

		private void AdaptDetailModeForDetailBasedSubtrees() {
//требуем использования сумм для запросов на деталях по сумме
			if (Obj.IsForObj && Row.Native.ResolveTag("usedetails") == "1") {
				Obj.DetailMode = DetailMode.SafeSumObject;
			}
		}

		private void ResolveTemporalCustomCodeBasedColumns(ISession session) {
			while (null != Col.Native && !string.IsNullOrWhiteSpace(Col.Native.ForeignCode)) {
				var _c = Col;
				Col = new ColumnHandler {Code = _c.Native.ForeignCode};
				if (0 != _c.Native.Year || 0 != _c.Native.Period) {
					Time = new TimeHandler {Year = _c.Native.Year, Period = _c.Native.Period};
				}
				Col.Normalize(this);
			}
		}

		/// <summary>
		/// 	Переводит строку (по нативу)
		/// </summary>
		/// <param name="zetaRow"> </param>
		/// <param name="selfcopy"> </param>
		/// <param name="rowcopy"> </param>
		public IQuery ToRow(IZetaRow zetaRow, bool selfcopy = false, bool rowcopy = false) {
			var q = this;
			if (selfcopy) {
				q = (Query)Copy();
			}
			if (rowcopy || selfcopy) {
				q.Row = q.Row.Copy();
			}
			q.Row.Native = zetaRow;
			q.InvalidateCacheKey();
			return q;
		}

		/// <summary>
		/// 	Синхронизатор результата
		/// </summary>
		/// <param name="timeout"> </param>
		public void WaitResult(int timeout =-1) {
			WaitPrepare(timeout);
			if (IsPrimary && null == Result) {
				Session.WaitForPrimary(timeout);
			}
		}

		/// <summary>
		/// 	Формула, которая присоединяется к запросу на фазе подготовки
		/// </summary>
		public IFormula AssignedFormula { get; set; }

		/// <summary>
		/// 	Модификатор кэш-строки (префикс)
		/// </summary>
		public string CustomHashPrefix;

		/// <summary>
		/// 	Тип вычисления запроса
		/// </summary>
		public QueryEvaluationType EvaluationType { get; set; }

		/// <summary>
		/// 	Sign that primary was not set
		/// </summary>
		public bool HavePrimary { get; set; }

		/// <summary>
		/// 	Статус по подготовке
		/// </summary>
		public PrepareState PrepareState { get; set; }

		/// <summary>
		/// 	Обратная ссылка на сессию
		/// </summary>
		public ISession Session { get; set; }

		/// <summary>
		/// 	Кэшированный запрос SQL
		/// </summary>
		public string SqlRequest;

		/// <summary>
		/// 	Реестр трассы
		/// </summary>
		public List<string> TraceList;

		/// <summary>
		/// 	Автоматический код запроса, присваиваемый системой
		/// </summary>
		public long Uid { get; set; }

		private List<IQuery> _formulaDependency;

		private List<Tuple<decimal, IQuery>> _summaDependency;
	}
}
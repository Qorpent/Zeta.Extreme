#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : QueryProcessor.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using System.Threading;
using Comdiv.Zeta.Model;

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
		}

		private IZetaQueryDimension GetMostPriorityNoPrimarySource(Query query) {
			if (query.Obj.IsFormula || (query.Obj.IsForObj && _sumh.IsSum(query.Obj.ObjRef))) {
				return query.Obj.ObjRef;
			}
			if (query.Col.IsFormula || _sumh.IsSum(query.Col.Native)) {
				return query.Col.Native;
			}
			return query.Col.Native;
		}

		private void PrepareFormulas(Query query, IZetaQueryDimension mostpriority) {
			if (_stat) {
				Interlocked.Increment(ref _session.Stat_QueryType_Formula);
			}
			var key = mostpriority.Code;
			var formula = FormulaStorage.Default.GetFormula(key, false);
			if (null == formula) {
				query.Result = new QueryResult {IsComplete = true, Error = new Exception("formula not found")};
				return;
			}
			query.AssignedFormula = formula;
			formula.Playback(query);
			var resulttask = new Func<QueryResult>(() =>
				{
					formula.Init(query);
					try {
						return formula.Eval();
					}
					finally {
						formula.CleanUp();
						FormulaStorage.Default.Return(key, formula);
					}
				});
			query.GetResultTask = _session.RegisterEvalTask(resulttask, false);
		}

		private void ExpandSum(Query query, IZetaQueryDimension mostpriority) {
			if (_stat) {
				Interlocked.Increment(ref _session.Stat_QueryType_Sum);
			}
			var subqueries = new List<Tuple<decimal, Query>>();
			foreach (var r in _sumh.GetSumDelta(query.Row.Native)) {
				var sq = r.Apply(query);
				sq = _session.Register(sq);
				if (null == sq) {
					continue;
				}
				subqueries.Add(new Tuple<decimal, Query>(r.Multiplicator, sq));
			}
			var subq = subqueries.ToArray();
			if (subq.Length == 0) {
				query.Result = new QueryResult {IsComplete = true, NumericResult = 0m};
				return;
			}

			var resulttask = new Func<QueryResult>(() =>
				{
					var result = 0m;
					foreach (var sq in subq) {
						var val = sq.Item2.GetResult();
						if (null != val) {
							result += val.NumericResult*sq.Item1;
						}
					}

					return new QueryResult {IsComplete = true, NumericResult = result};
				});
			query.GetResultTask = _session.RegisterEvalTask(resulttask, false);
		}

		/// <summary>
		/// 	Регистрирует первичный запрос
		/// </summary>
		/// <param name="query"> </param>
		protected virtual void RegisterPrimaryRequest(Query query) {
			if (_stat) {
				Interlocked.Increment(ref _session.Stat_QueryType_Primary);
			}
			if (_session.DoNotExecuteRealSql) {
				if (null != _session.StubDataGenerator) {
					query.Result = _session.StubDataGenerator(query);
				}
				else {
					query.Result = new QueryResult
						{IsComplete = false, Error = new Exception("no sql or sql stub supported by session")};
				}
			}
			query.GetResultTask = _session.RegisterSqlRequest(query);
			if (_session.TraceQuery) {
				query.TraceList.Add(_session.Id + " preq taskid: " + query.GetResultTask.Id);
			}
		}

		private readonly Session _session;
		private readonly bool _stat;
		private readonly StrongSumProvider _sumh;
	}
}
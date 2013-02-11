#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : DefaultZexQueryPreparator.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Zeta.Extreme {
	/// <summary>
	/// 	Стандартная реализация подготовщика запросов
	/// </summary>
	/// <remarks>
	/// 	Подготовщик работает в ситуации уверенности
	/// 	в том, что запрос полностью нормализован,
	/// 	уникален и прописан во всех требуемых списках
	/// </remarks>
	public class DefaultZexQueryPreparator : IZexQueryPreparator {
		/// <summary>
		/// 	Основной конструктор, связывает с сессией
		/// </summary>
		/// <param name="session"> </param>
		public DefaultZexQueryPreparator(ZexSession session) {
			_session = session;
			_stat = _session.CollectStatistics;
			_sumh = new ZetaVirtualSumHelper();
		}

		/// <summary>
		/// 	Выполняет подготовку запроса к выполнению
		/// 	выполняется после препроцессора и проверок
		/// </summary>
		/// <param name="query"> </param>
		public void Prepare(ZexQuery query) {
			if (query.IsPrimary) {
				RegisterPrimaryRequest(query);
			}
			else if (null != query.Row.Native && _sumh.IsSum(query.Row.Native) && query.Col.IsPrimary()) {
				ExpandSum(query);
			}
			else {
				PrepareFormulas(query);
			}
		}

		private void PrepareFormulas(ZexQuery query) {
			if (_stat) {
				Interlocked.Increment(ref _session.Stat_QueryType_Formula);
			}
			var key = query.Row.Formula.Trim();
			var formula = FormulaStorage.Default.GetFormula(key, false);
			if (null == formula) {
				query.Result = new QueryResult {IsComplete = true, Error = new Exception("formula not found")};
				return;
			}
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

		private void ExpandSum(ZexQuery query) {
			if (_stat) {
				Interlocked.Increment(ref _session.Stat_QueryType_Sum);
			}
			var subqueries = new List<Tuple<ZexQueryDelta, Task<ZexQuery>>>();
			foreach (var r in _sumh.GetSumDelta(query.Row.Native)) {
				var sq = r.Apply(query);
				subqueries.Add(new Tuple<ZexQueryDelta, Task<ZexQuery>>(r, _session.RegisterAsync(sq)));
			}
			Task.WaitAll(subqueries.Select(_ => _.Item2).ToArray());

			var subq = subqueries.Where(_ => null != _.Item2.Result).ToArray();

			if (subq.Length == 0) {
				query.Result = new QueryResult {IsComplete = true, NumericResult = 0m};
				return;
			}

			var resulttask = new Func<QueryResult>(() =>
				{
					var result = subq.AsParallel().Aggregate(
						0m,
						(r, q) =>
							{
								var res = q.Item2.Result.GetResult();
								if (null != res) {
									r += res.NumericResult*q.Item1.Multiplicator;
								}
								return r;
							});
					return new QueryResult {IsComplete = true, NumericResult = result};
				});
			query.GetResultTask = _session.RegisterEvalTask(resulttask, false);
		}

		/// <summary>
		/// 	Регистрирует первичный запрос
		/// </summary>
		/// <param name="query"> </param>
		protected virtual void RegisterPrimaryRequest(ZexQuery query) {
			if (_stat) {
				Interlocked.Increment(ref _session.Stat_QueryType_Primary);
			}

			var sqlbuilder = _session.GetSqlBuilder();

			sqlbuilder.PrepareSqlRequest(query);
			query.GetResultTask = _session.RegisterSqlRequest(query);
			query.Result = new QueryResult {IsComplete = true, Error = new Exception("primaries not supproted by now")};
		}

		private readonly ZexSession _session;
		private readonly bool _stat;
		private readonly ZetaVirtualSumHelper _sumh;
	}
}
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
			else if (query.Row.IsSum && query.Col.IsPrimary()) {
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
			query.Result = new QueryResult {IsComplete = true, Error = new Exception("formulas not supproted by now")};
		}

		private void ExpandSum(ZexQuery query) {
			if (_stat) {
				Interlocked.Increment(ref _session.Stat_QueryType_Sum);
			}
			var subqueries = new List<ZexQuery>();
			foreach (var r in query.Row.Native.Children) {
				if (r.IsMarkSeted("0NOSUM")) {
					continue;
				}
				var q = _session.Register(query.ToRow(r, true));
				if (null != q) {
					subqueries.Add(q);
				}
			}
			if (subqueries.Count == 0) {
				query.Result = new QueryResult {IsComplete = true, NumericResult = 0m};
			}

			var resulttask = new Func<QueryResult>(() =>
				{
					var awaits = subqueries.Select(_ => _.GetResultTask).Where(_ => null != _).OfType<Task>().ToArray();
					if (0 != awaits.Length) {
						Task.WaitAll(awaits);
					}
					var result = subqueries.Sum(sq => sq.GetResult().NumericResult);
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
			query.GetResultTask = _session.RegisterForDataRequest(query);
			query.Result = new QueryResult {IsComplete = true, Error = new Exception("primaries not supproted by now")};
		}

		private readonly ZexSession _session;
		private readonly bool _stat;
	}
}
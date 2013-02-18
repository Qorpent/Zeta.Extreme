#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : DefaultPrimarySource.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Comdiv.Application;
using Qorpent.Applications;

namespace Zeta.Extreme {
	/// <summary>
	/// 	Сборщик первичных данных
	/// </summary>
	public class DefaultPrimarySource : IPrimarySource {
		/// <summary>
		/// </summary>
		public DefaultPrimarySource() {}

		/// <summary>
		/// Журнал выполненных SQL
		/// </summary>
		public IList<string> QueryLog { get; private set; }

		/// <summary>
		/// 	Конструктор с учетом сессии
		/// </summary>
		/// <param name="session"> </param>
		public DefaultPrimarySource(Session session) {
			_session = session;
			TraceQuery = session.TraceQuery;
			QueryLog = new List<string>();
		}

		/// <summary>
		/// 	Признак трассировки сессии
		/// </summary>
		private bool TraceQuery { get; set; }

		/// <summary>
		/// 	Объект синхронизации с PrimarySource
		/// </summary>
		public object Sync {
			get { return _syncsqlawait; }
		}

		private bool CollectStatistics {
			get { return null != _session && _session.CollectStatistics; }
		}

		/// <summary>
		/// 	Регистрирует целевой запрос
		/// </summary>
		/// <param name="query"> </param>
		public void Register(Query query) {
			lock (_syncsqlawait) {
				if (DoNotExecuteRealSql) {
					if (null != StubDataGenerator) {
						query.Result = StubDataGenerator(query);
					}
					else {
						query.Result = new QueryResult
							{IsComplete = false, Error = new Exception("no sql or sql stub supported by session")};
					}

					return;
				}
				if (null != query.Result) {
					return;
				}
				_sqlDataAwaiters.Add(query);
				if (null == _currentSqlBatchTask) {
					_currentSqlBatchTask = CreateNewSqlBatchTask();
				}
				if (_sqlDataAwaiters.Count >= BatchSize) {
					RunSqlBatch();
				}
			}
		}


		/// <summary>
		/// 	Выполняет все требуемые запросы в режиме ожидания
		/// </summary>
		public void Wait() {
			RunSqlBatch();
			Task t = null;
			while (_sqltasks.TryTake(out t)) {
				t.Wait();
			}
		}


		/// <summary>
		/// 	Регистрирует заранее подготовленный SQL-запрос
		/// </summary>
		/// <param name="preparedQuery"> </param>
		public void Register(string preparedQuery) {
			throw new NotSupportedException();
		}

		/// <summary>
		/// 	Получает асинхронную задачу сбора текущих данных,
		/// 	завершение задачи означает окончание всех текущих запросов
		/// </summary>
		/// <returns> </returns>
		public Task Collect() {
			return RunSqlBatch();
		}

		/// <summary>
		/// 	Стартует текущую задачу по SQL
		/// </summary>
		public Task<QueryResult> RunSqlBatch() {
			lock (_syncsqlawait) {
				var task = _currentSqlBatchTask;
				if (null == task) {
					task = CreateNewSqlBatchTask();
				}
				_currentSqlBatchTask = null;
				_sqltasks.Add(task);
				task.Start();
				return task;
			} //NOTE: bad design - register something in agenda of session -co-dependency and bad design
		}

		private Task<QueryResult> CreateNewSqlBatchTask() {
			return new Task<QueryResult>(() =>
				{
					Dictionary<long, Query> _myrequests;
					lock (_syncsqlawait) {
						if (_sqlDataAwaiters.IsEmpty) {
							return null;
						}
						_myrequests = _sqlDataAwaiters.ToArray().Distinct().ToDictionary(_ => _.UID, _ => _);
						_sqlDataAwaiters = new ConcurrentBag<Query>();
					}
					if (TraceQuery) {
						foreach (var q in _myrequests.Values) {
							q.TraceList.Add("Session " + _session.Id + " added to primreq ");
						}
					}
					Stopwatch sw = null;
					if (CollectStatistics) {
						sw = Stopwatch.StartNew();
						Interlocked.Increment(ref _session.Stat_Batch_Count);
					}
					var script = GenerateScript(_myrequests.Values.ToArray());

					using (var c = GetConnection()) {
						c.Open();
						var cmd = c.CreateCommand();
						cmd.CommandText = script;
						using (var r = cmd.ExecuteReader()) {
							bool _nr = false;
							while (r.Read()||(_nr =r.NextResult())) {
								if(_nr) {
									_nr = false;
									continue;
								}
								var id = r.GetInt32(0);
								
								if (CollectStatistics) {
									Interlocked.Increment(ref _session.Stat_Primary_Catched);
								}
								var col = r.GetInt32(1);
								var row = r.GetInt32(2);
								var obj = r.GetInt32(3);
								var year = r.GetInt32(4);
								var period = r.GetInt32(5);
								var value = r.GetDecimal(6);
								var target =
									_myrequests.Values.FirstOrDefault(
										_ => _.Row.Id == row && _.Col.Id == col && _.Obj.Id == obj && _.Time.Year == year && _.Time.Period == period);
								if (null != target) {
									if (CollectStatistics) {
										Interlocked.Increment(ref _session.Stat_Primary_Affected);
									}
									if (TraceQuery) {
										target.TraceList.Add("primary found " + value);
									}
									target.HavePrimary = true;
									target.Result = new QueryResult {IsComplete = true, NumericResult = value, CellId = id};
								}
							}
						}
					}


					if (CollectStatistics) {
						sw.Stop();
						
						lock (_syncsqlawait) {
							QueryLog.Add("/* EXECUTE TIME " + sw.Elapsed + " */" + script);
							_session.Stat_Batch_Time += sw.Elapsed;
						}
					}
					foreach (var myrequest in _myrequests.Values.Where(_ => !_.HavePrimary)) {
						if (TraceQuery) {
							myrequest.TraceList.Add("primary not found ");
						}
						myrequest.Result = new QueryResult {IsComplete = true, IsNull = true};
					}

					return null;
				});
		}

		/// <summary>
		/// Генератор скриптов из  переданной коллекции запросов
		/// </summary>
		/// <param name="_myrequests"></param>
		/// <returns></returns>
		public string GenerateScript(Query[] _myrequests) {
			var usualperiods = _myrequests.Where(_ => _.Time.Periods == null).ToArray();
			var sumperiods = _myrequests.Where(_ => _.Time.Periods != null).ToArray();
			var script ="";
			if(0!=usualperiods.Length) {
				var times = usualperiods.Select(_ => new {y = _.Time.Year, p = _.Time.Period}).Distinct();
				var colobj = usualperiods.Select(_ => new {o = _.Obj.Id, c = _.Col.Id}).Distinct();
				var rowids = string.Join(",", usualperiods.Select(_ => _.Row.Id).Distinct());
				
				foreach (var time in times) {
					foreach (var cobj in colobj) {
						script +=
							string.Format(
								@"
								if exists(select top 1 id from cell where period={0} and year={1} and col={2} and obj={3} and row in ({4}))
									select id,col,row,obj,year,period,decimalvalue 
									from cell where period={0} and year={1} and col={2} and obj={3} and row in ({4})",
								time.p, time.y, cobj.c, cobj.o, rowids);
					}
				}
			}
			if(0!=sumperiods.Length) {
				var sptimes =
					sumperiods.Select(_ => new {y = _.Time.Year, p = _.Time.Period, ps = string.Join(",", _.Time.Periods)}).Distinct();
				var spcolobj = sumperiods.Select(_ => new {o = _.Obj.Id, c = _.Col.Id}).Distinct();
				var sprowids = string.Join(",", sumperiods.Select(_ => _.Row.Id).Distinct());
				foreach (var time in sptimes)
				{
					foreach (var cobj in spcolobj)
					{
						script +=
							string.Format(
								"\r\nselect 0,col,row,obj,year,{5},sum(decimalvalue) from cell where period in ({0}) and year={1} and col={2} and obj={3} and row in ({4}) group by col,row,obj,year ",
								time.ps, time.y, cobj.c, cobj.o, sprowids,time.p);
					}
				}
			}
			return script;
		}


		private IDbConnection GetConnection() {
			try {
				var result = Application.Current.DatabaseConnections.GetConnection("Default");
				if (null == result) {
					return myapp.sql.GetConnection("Default");
				}
				return result;
			}
			catch {
				return myapp.sql.GetConnection("Default");
			}
		}

		private readonly Session _session;
		private readonly ConcurrentBag<Task> _sqltasks = new ConcurrentBag<Task>();
		private readonly object _syncsqlawait = new object();

		/// <summary>
		/// 	Размер батча
		/// </summary>
		public int BatchSize = 1000;

		/// <summary>
		/// 	True - отладочный режим, вместо расчета реальных значений будет подставлять стаб-значения
		/// </summary>
		public bool DoNotExecuteRealSql;

		/// <summary>
		/// 	Отладочный делегат для имитации работы БД
		/// </summary>
		public Func<Query, QueryResult> StubDataGenerator;

		private Task<QueryResult> _currentSqlBatchTask;
		private ConcurrentBag<Query> _sqlDataAwaiters = new ConcurrentBag<Query>();
	}
}
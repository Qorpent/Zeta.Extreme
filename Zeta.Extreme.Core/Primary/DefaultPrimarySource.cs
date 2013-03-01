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

namespace Zeta.Extreme.Primary {
	/// <summary>
	/// 	������� ��������� ������
	/// </summary>
	public class DefaultPrimarySource : IPrimarySource {
		/// <summary>
		/// </summary>
		public DefaultPrimarySource() {}

		/// <summary>
		/// 	����������� � ������ ������
		/// </summary>
		/// <param name="session"> </param>
		public DefaultPrimarySource(Session session) {
			_session = session;
			TraceQuery = session.TraceQuery;
			QueryLog = new List<string>();
		}

		/// <summary>
		/// 	������ ����������� SQL
		/// </summary>
		public IList<string> QueryLog { get; private set; }

		/// <summary>
		/// 	������� ����������� ������
		/// </summary>
		private bool TraceQuery { get; set; }

		/// <summary>
		/// 	������ ������������� � PrimarySource
		/// </summary>
		public object Sync {
			get { return _syncsqlawait; }
		}

		private bool CollectStatistics {
			get { return null != _session && _session.CollectStatistics; }
		}

		/// <summary>
		/// 	������������ ������� ������
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
		/// 	��������� ��� ��������� ������� � ������ ��������
		/// </summary>
		public void Wait() {
			RunSqlBatch();
			Task t = null;
			while (_sqltasks.TryTake(out t)) {
				t.Wait();
			}
		}


		/// <summary>
		/// 	������������ ������� �������������� SQL-������
		/// </summary>
		/// <param name="preparedQuery"> </param>
		public void Register(string preparedQuery) {
			throw new NotSupportedException();
		}

		/// <summary>
		/// 	�������� ����������� ������ ����� ������� ������,
		/// 	���������� ������ �������� ��������� ���� ������� ��������
		/// </summary>
		/// <returns> </returns>
		public Task Collect() {
			return RunSqlBatch();
		}

		/// <summary>
		/// 	�������� ������� ������ �� SQL
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
					var sw = Stopwatch.StartNew();
					var myrequests = CollectRequests();

					if (myrequests == null) {
						return null;
					}
					var script = GenerateScript(myrequests.Values.ToArray());

					ExecuteQuery(script, myrequests);


					PostProcessQuery(sw, script, myrequests);

					return null;
				});
		}

		private void PostProcessQuery(Stopwatch sw, string script, Dictionary<long, Query> myrequests) {
			if (CollectStatistics) {
				sw.Stop();

				lock (_syncsqlawait) {
					QueryLog.Add("/* EXECUTE TIME " + sw.Elapsed + " */" + script);
					_session.Stat_Batch_Time += sw.Elapsed;
				}
			}
			foreach (var myrequest in myrequests.Values.Where(_ => !_.HavePrimary)) {
				if (TraceQuery) {
					myrequest.TraceList.Add("primary not found ");
				}
				myrequest.Result = new QueryResult {IsComplete = true, IsNull = true};
			}
		}

		private void ExecuteQuery(string script, Dictionary<long, Query> myrequests) {
			var _grouped = myrequests.Values.GroupBy(_ => _.Row.Id, _ => _).ToDictionary(_ => _.Key, _=>_);
			using (var c = GetConnection()) {
				c.Open();
				var cmd = c.CreateCommand();
				cmd.CommandText = script;
				using (var r = cmd.ExecuteReader()) {
					var _nr = false;
					while (r.Read() || (_nr = r.NextResult())) {
						if (_nr) {
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
						var target =_grouped[row].FirstOrDefault(_ => _.Col.Id == col && _.Obj.Id == obj && _.Time.Year == year && _.Time.Period == period);
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
		}

		private Dictionary<long, Query> CollectRequests() {
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

			if (CollectStatistics) {
				Interlocked.Increment(ref _session.Stat_Batch_Count);
			}
			return _myrequests;
		}

		/// <summary>
		/// 	��������� �������� ��  ���������� ��������� ��������
		/// </summary>
		/// <param name="_myrequests"> </param>
		/// <returns> </returns>
		public string GenerateScript(Query[] _myrequests) {
			var groups = ExplodeToGroups(_myrequests);
			return string.Join("\r\n", groups.Select(_ => _.GenerateSqlScript()));
		}

		private IEnumerable<PrimaryQueryGroup> ExplodeToGroups(Query[] myrequests) {
			yield return new PrimaryQueryGroup {Queries = myrequests, ScriptGenerator = new SimpleObjectDataScriptGenerator()};
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
		/// 	������ �����
		/// </summary>
		public int BatchSize = 1000;

		/// <summary>
		/// 	True - ���������� �����, ������ ������� �������� �������� ����� ����������� ����-��������
		/// </summary>
		public bool DoNotExecuteRealSql;

		/// <summary>
		/// 	���������� ������� ��� �������� ������ ��
		/// </summary>
		public Func<Query, QueryResult> StubDataGenerator;

		private Task<QueryResult> _currentSqlBatchTask;
		private ConcurrentBag<Query> _sqlDataAwaiters = new ConcurrentBag<Query>();
	}
}
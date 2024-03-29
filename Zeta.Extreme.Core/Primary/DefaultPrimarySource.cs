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
// PROJECT ORIGIN: Zeta.Extreme.Core/DefaultPrimarySource.cs
#endregion
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Qorpent.Applications;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.Querying;

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
		public DefaultPrimarySource(ISession session) {
			_session = session;
			QueryLog = new List<string>();
		}

		/// <summary>
		/// 	������ ����������� SQL
		/// </summary>
		public IList<string> QueryLog { get; private set; }



		/// <summary>
		/// 	������ ������������� � PrimarySource
		/// </summary>
		public object Sync {
			get { return _syncsqlawait; }
		}

		private bool CollectStatistics {
			get { return null != _session && _session.IsCollectStatistics(); }
		}

		/// <summary>
		/// 	������������ ������� ������
		/// </summary>
		/// <param name="query"> </param>
		public void Register(IQuery query) {
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
		public void Wait(int timeout =-1) {
			RunSqlBatch();
			Task t;
			while (_sqltasks.TryTake(out t)) {
				t.Wait(timeout);
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
				var task = _currentSqlBatchTask ?? CreateNewSqlBatchTask();
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

		private void PostProcessQuery(Stopwatch sw, string script, Dictionary<long, IQueryWithProcessing> myrequests) {
			if (CollectStatistics) {
				sw.Stop();

				lock (_syncsqlawait) {
					QueryLog.Add("/* EXECUTE TIME " + sw.Elapsed + " */" + script);
					_session.StatIncStatBatchTime(sw.Elapsed);
				}
			}
			foreach (var myrequest in myrequests.Values.Where(_ => !_.HavePrimary)) {
				myrequest.Result = new QueryResult {IsComplete = true, IsNull = true};
			}
		}

		private void ExecuteQuery(string script, Dictionary<long, IQueryWithProcessing> myrequests) {
			var _grouped = myrequests.Values.GroupBy(_ => _.Row.Id, _ => _).ToDictionary(_ => _.Key, _ => _);
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

						_session.StatIncPrimaryCatched();
						var col = r.GetInt32(1);
						var row = r.GetInt32(2);
						var obj = r.GetInt32(3);
						var year = r.GetInt32(4);
						var period = r.GetInt32(5);
						var value = r.GetDecimal(6);
						var otype = r.GetInt32(7);
						var reference = r.GetString(8);
						var target =
							_grouped[row].FirstOrDefault(
								_ =>
								_.Col.Id == col && _.Obj.Id == obj && (int) _.Obj.Type == otype && _.Time.Year == year &&
								_.Time.Period == period && ((_.Reference.Contragents??"")+":"+(_.Reference.Types??""))==reference);
						if (null != target) {
							_session.StatIncPrimaryAffected();
							target.HavePrimary = true;
							target.Result = new QueryResult {IsComplete = true, NumericResult = value, CellId = id};
						}
					}
				}
			}
		}

		private Dictionary<long, IQueryWithProcessing> CollectRequests() {
			Dictionary<long, IQueryWithProcessing> _myrequests;
			lock (_syncsqlawait) {
				if (_sqlDataAwaiters.IsEmpty) {
					return null;
				}
				_myrequests = _sqlDataAwaiters.Cast<IQueryWithProcessing>().ToArray().Distinct().ToDictionary(_ => _.Uid, _ => _);
				_sqlDataAwaiters = new ConcurrentBag<IQuery>();
			}
			_session.StatIncBatchCount();
			return _myrequests;
		}

		/// <summary>
		/// 	��������� �������� ��  ���������� ��������� ��������
		/// </summary>
		/// <param name="_myrequests"> </param>
		/// <returns> </returns>
		public string GenerateScript(IQueryWithProcessing[] _myrequests) {
			var groups = ExplodeToGroups(_myrequests);
			return string.Join("\r\n", groups.Select(_ => _.GenerateSqlScript()));
		}

		private  IEnumerable<PrimaryQueryGroup> ExplodeToGroups(IQueryWithProcessing[] myrequests) {
			return 
				from prototypegroup in myrequests.GroupBy(_ => _.GetPrototype(), _ => _)
				from dgroup in prototypegroup.GroupBy(_ => _.Obj.DetailMode, _ => _) 
				from altobjgroup in dgroup.GroupBy(_ => _.Reference.Contragents+_.Reference.Types)  
				select new PrimaryQueryGroup
					{
						Queries = altobjgroup.ToArray(), 
						ScriptGenerator = new Zeta2SqlScriptGenerator(), 
						Prototype = prototypegroup.Key
					};
		}


		private IDbConnection GetConnection() {
			return Application.Current.DatabaseConnections.GetConnection("Default");
		}

		private readonly ISession _session;
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
		public Func<IQuery, QueryResult> StubDataGenerator;

		private Task<QueryResult> _currentSqlBatchTask;
		private ConcurrentBag<IQuery> _sqlDataAwaiters = new ConcurrentBag<IQuery>();
	}
}
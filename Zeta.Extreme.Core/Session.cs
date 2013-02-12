#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : ZexSession.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Comdiv.Application;

namespace Zeta.Extreme {
	/// <summary>
	/// 	������� ����� ������ �������� Zeta
	/// </summary>
	/// <remarks>
	/// 	������ �������� �������� ��������
	/// 	����� ��������� �������.
	/// 	����� ������� ������ � �������:
	/// 	create session ==- register queries ==- evaluate  ==- collect result
	/// 	������ �������� � ������������ �������������� async - �����������
	/// </remarks>
	public class Session {
		private static int ID;
		/// <summary>
		/// ������� ������ ����������� ��������
		/// </summary>
		public bool TraceQuery = false;

		/// <summary>
		/// 	����������� �� ���������
		/// </summary>
		/// <remarks>
		/// 	���������� �������� ���������
		/// </remarks>
		public Session(bool collectStatistics = false) {
			Id = ++ID;
			CollectStatistics = collectStatistics;
			Registry = new ConcurrentDictionary<string, Query>();
			ActiveSet = new ConcurrentDictionary<string, Query>();
			KeyMap = new ConcurrentDictionary<string, string>();
		}


		private object thissync = new object();
		/// <summary>
		/// 	���������� ������������� ������ � ��������
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// 	������� ������ ��������
		/// </summary>
		/// <remarks>
		/// 	��� ����������� ������� ������� ������������� ��� ���������� UID
		/// 	�����, � MainQueryRegistry �� ����� �� ������ Value ����� ������� ��������
		/// </remarks>
		public ConcurrentDictionary<string, Query> Registry { get; private set; }

		/// <summary>
		/// 	������������ ������
		/// </summary>
		protected internal Session MasterSession { get; set; }


		/// <summary>
		/// 	��������������� ������ ������ ����� ������� � �����������������
		/// 	��������
		/// </summary>
		protected internal ConcurrentDictionary<string, string> KeyMap { get; private set; }

		/// <summary>
		/// 	����� ���� ����������, ��� �� ������������ �������� (������)
		/// 	���� - ������
		/// </summary>
		protected internal ConcurrentDictionary<string, Query> ActiveSet { get; private set; }

		/// <summary>
		/// 	��������� �������� ��������� (�������� ��� ������)
		/// 	�������� ������ ����� ������ � ���� ��������,
		/// 	�� ������ ��������� ���� �������� ��������� ������� �� �����
		/// </summary>
		/// <returns> </returns>
		public ISerialSession GetSubSession() {
			lock(thissync) {
				ISerialSession result;
				if (_subsessionpool.TryPop(out result)) {
					result.GetUnderlinedSession()._preEvalTaskAgenda.Clear();
					result.GetUnderlinedSession()._evalTaskAgenda.Clear();
					return result;
				}
				var copy = new Session(CollectStatistics)
					{
						Registry = Registry,
						ActiveSet = ActiveSet,
						KeyMap = KeyMap,
						MasterSession = this,
						TraceQuery = TraceQuery,
						thissync = thissync,
						syncsqlawait = syncsqlawait
					};
				if (CollectStatistics) {
					Stat_SubSession_Count ++;
				}
				//share query cache
				//but not task queues
				result = copy.AsSerial(); //we not allow use it on non-serial way
				_subsessions[copy.Id] = copy;
				return result;
			}
		}

		private IDictionary<int, Session> _subsessions = new Dictionary<int, Session>();

		/// <summary>
		/// 	��������� ������� �������������� ��������� � ���
		/// </summary>
		/// <param name="session"> </param>
		protected internal void Return(ISerialSession session) {
			_subsessionpool.Push(session);
		}


		/// <summary>
		/// 	���������� ��������� � ���������� ������
		/// </summary>
		/// <returns> </returns>
		public string GetStatisticString() {
			var sb = new StringBuilder();
			foreach (var source in GetType().GetFields().Where(x => x.Name.StartsWith("Stat_")).OrderBy(x => x.Name)) {
				sb.Append(source.Name.Substring(5));
				sb.Append(' ');
				sb.Append(source.GetValue(this));
				sb.Append(Environment.NewLine);
			}
			sb.AppendLine("regpool:" + _registryhelperpool.Count);
			sb.AppendLine("pppool:" + _preloadprocesspool.Count);
			sb.AppendLine("preppool:" + _preparators.Count);
			
			if (!_subsessionpool.IsEmpty) {
				sb.AppendLine("==================================");
				sb.AppendLine("Sub-Sessions:");
				var subs = _subsessionpool.ToArray();
				foreach (var s in subs) {
					var ses = s.GetUnderlinedSession();
					sb.AppendLine("Subsession: " + ses.Id);
					sb.Append(ses.GetStatisticString());
					sb.AppendLine("--------------------------");
				}
			}
			return sb.ToString();
		}

		/// <summary>
		/// 	���������� ����������� ������� � ������
		/// </summary>
		/// <param name="query"> �������� ������ </param>
		/// <param name="uid"> ��������� ���� ������� ��������� ��� ��� ����������� ���������������� ��������� �������� </param>
		/// <returns> ������ �� ������ ����������� � ������ </returns>
		/// <remarks>
		/// 	��� ����������� �������, �� �������� �������������� ����������� � �������� �� ������,
		/// 	������������ ������ �������� ������
		/// </remarks>
		/// <exception cref="NotImplementedException"></exception>
		public Query Register(Query query, string uid = null) {
			lock(thissync) {
				var helper = GetRegistryHelper();
				var result = helper.Register(query, uid);
				ReturnRegistryHelper(helper);
				return result;
			}
		}

		/// <summary>
		/// 	����������� ����������� ������� � ������
		/// </summary>
		/// <param name="query"> �������� ������ </param>
		/// <param name="uid"> ��������� ���� ������� ��������� ��� ��� ����������� ���������������� ��������� �������� </param>
		/// <returns> ������, �� ����������� ������� ������������ ������ �� ������ ����������� � ������ </returns>
		/// <remarks>
		/// 	��� ����������� �������, �� �������� �������������� ����������� � �������� �� ������,
		/// 	������������ ������ �������� ������
		/// </remarks>
		public Task<Query> RegisterAsync(Query query, string uid = null) {
			lock(thissync) {
				var id = _preEvalTaskCounter++;
				var task = new Task<Query>(() =>
					{
						try {
							var helper = GetRegistryHelper();
							var result = helper.Register(query, uid);
							ReturnRegistryHelper(helper);

							return result;
						}
						finally {
							Task t;
							_preEvalTaskAgenda.TryRemove(id, out t);
						}
					});
				_preEvalTaskAgenda[id] = task;
				//������ ������ � �������� ������, ����� 
				//WaitRegistry ����� ������ ����������
				task.Start();
				return task;
			}
		}

		/// <summary>
		/// 	���������� ����������� ���������� ������� � ����������
		/// 	���������� �� �� ������, ��� � �����������
		/// </summary>
		/// <param name="query"> </param>
		/// <returns> </returns>
		protected internal Task PrepareAsync(Query query) {
			lock(thissync) {
				var id = _preEvalTaskCounter++;
				var task = new Task(() =>
					{
						try {
							var preparator = GetPreparator();
							preparator.Prepare(query);
							Return(preparator);
						}
						finally {
							Task t;
							_preEvalTaskAgenda.TryRemove(id, out t);
						}
					});
				_preEvalTaskAgenda[id] = task;
				//������ ������ � �������� ������, ����� 
				//WaitRegistry ����� ������ ����������
				task.Start();
				return task;
			}
		}


		/// <summary>
		/// 	������� ��������� ���� ��������� ����������� �����������
		/// </summary>
		protected internal void WaitPreparation() {
			
			while (!_preEvalTaskAgenda.IsEmpty) {
				SyncPreEval();
				Thread.Sleep(20);
			}

		
		}
		/// <summary>
		/// ����� ������������� � SQL
		/// </summary>
		protected internal void WaitSql() {
			
			if(null!=MasterSession) {
				MasterSession.WaitSql();
				return;
			}

			RunSqlBatch(); // ��������� ���������� �������
			
			
			
		}

		/// <summary>
		/// 	������� ��������� ���� ��������� ����������� �����������
		/// </summary>
		protected internal void WaitEvaluation() {
			WaitSql();
			//	Thread.Sleep(20);
			Task.WaitAll(_evalTaskAgenda.Values.Where(_ => _.Status != TaskStatus.Created).ToArray());
			while (_evalTaskAgenda.Any()) {
				// ��� ��� ��� ������� ������ � �� ���� �� ����������,
				// �� �� ��������� �� ��� �������, ��� �����������������
				// ��� ���� ��������� �������� ������������ ���� �� ��������
				// � ������ ����� WaitResult �� ������
				var task = _evalTaskAgenda.FirstOrDefault().Value;
				if (null != task) {
					if (task.Status == TaskStatus.Created) {
						try {
							task.Start();
						}
						catch {}
					}
					task.Wait();
				}
			}

			while (!_evalTaskAgenda.IsEmpty) {
				Task.WaitAll(_evalTaskAgenda.Values.ToArray());
				//	Thread.Sleep(20);
			}

		
		}

		/// <summary>
		/// 	���������� ������ ���������������� ������ �����������
		/// </summary>
		/// <returns> </returns>
		/// <exception cref="NotImplementedException"></exception>
		public IQueryPreparator GetPreparator() {
			lock(thissync) {
				IQueryPreparator result;
				if (_preparators.TryPop(out result)) {
					return result;
				}
				if (null != CustomPreparatorClass) {
					return Activator.CreateInstance(CustomPreparatorClass, this) as IQueryPreparator;
				}
				return new DefaultQueryPreparator(this);
			}
		}

		/// <summary>
		/// 	���������� ������������ � ���
		/// </summary>
		/// <param name="preparator"> </param>
		private void Return(IQueryPreparator preparator) {
			_preparators.Push(preparator);
		}


		/// <summary>
		/// 	���������� ������ ���������������� ������ �����������
		/// </summary>
		/// <returns> </returns>
		/// <exception cref="NotImplementedException"></exception>
		private IRegistryHelper GetRegistryHelper() {
			lock(thissync) {
				IRegistryHelper result;
				if (_registryhelperpool.TryPop(out result)) {
					return result;
				}
				if (null != CustomRegistryHelperClass) {
					return Activator.CreateInstance(CustomRegistryHelperClass, this) as IRegistryHelper;
				}
				return new DefaultRegistryHelper(this);
			}
		}

		/// <summary>
		/// 	���������� ������������ � ���
		/// </summary>
		/// <param name="helper"> </param>
		private void ReturnRegistryHelper(IRegistryHelper helper) {
			_registryhelperpool.Push(helper);
		}

		/// <summary>
		/// 	���������� ������ �������������
		/// </summary>
		/// <returns> </returns>
		/// <exception cref="NotImplementedException"></exception>
		protected internal IPreloadProcessor GetPreloadProcessor() {
			lock(thissync) {
				IPreloadProcessor result;
				if (_preloadprocesspool.TryPop(out result)) {
					return result;
				}
				if (null != CustomPreloadProcessorClass) {
					return Activator.CreateInstance(CustomRegistryHelperClass, this) as IPreloadProcessor;
				}
				return new DefaultPreloadProcessor(this);
			}
		}

		/// <summary>
		/// 	���������� ������������ � ���
		/// </summary>
		/// <param name="processor"> </param>
		protected internal void Return(IPreloadProcessor processor) {
			_preloadprocesspool.Push(processor);
		}


		/// <summary>
		/// 	���������� ������ �������������
		/// </summary>
		/// <returns> </returns>
		/// <exception cref="NotImplementedException"></exception>
		protected internal IPeriodEvaluator GetPeriodEvaluator() {
			lock(thissync) {
				IPeriodEvaluator result;
				if (_periodevalpool.TryPop(out result)) {
					return result;
				}
				if (null != CustomPeriodEvaluatorClass) {
					return Activator.CreateInstance(CustomPeriodEvaluatorClass, this) as IPeriodEvaluator;
				}
				return new DefaultPeriodEvaluator();
			}
		}

		/// <summary>
		/// 	���������� ������������ � ���
		/// </summary>
		/// <param name="periodEvaluator"> </param>
		protected internal void Return(IPeriodEvaluator periodEvaluator) {
			_periodevalpool.Push(periodEvaluator);
		}

		/// <summary>
		/// 	����������� � ������ ������ �� ����������
		/// </summary>
		/// <param name="resulttask"> </param>
		/// <param name="hot"> ����������� ������ </param>
		/// <returns> </returns>
		protected internal Task<QueryResult> RegisterEvalTask(Func<QueryResult> resulttask, bool hot) {
			lock(thissync) {
				var id = _evalTaskCounter++;
				var task = new Task<QueryResult>(() =>
					{
						try {
							return resulttask();
						}
						finally {
							Task t;
							_evalTaskAgenda.TryRemove(id, out t);
						}
					});
				_evalTaskAgenda[id] = task;
				//������ ������ � �������� ������, ����� 
				//WaitRegistry ����� ������ ����������

				if (hot) {
					task.Start();
				}
				return task;
			}
		}

		/// <summary>
		/// 	������ �������������� ���������� ����� � �������� �������� ����������
		/// </summary>
		protected internal void SyncPreEval() {
			Task.WaitAll(_preEvalTaskAgenda.Values.ToArray());
		}

	

		/// <summary>
		/// 	����������� ������ � ���������� SQL �������� � ������� �� ���������� ��������
		/// </summary>
		/// <param name="query"> </param>
		/// <returns> </returns>
		/// <exception cref="NotImplementedException"></exception>
		protected internal Task<QueryResult> RegisterSqlRequest(Query query) {
			lock (syncsqlawait) {
				if(null!=MasterSession) {
					return MasterSession.RegisterSqlRequest(query);
				}
				_sqlDataAwaiters.Add(query);
				if (null == _currentSqlBatchTask) {
					_currentSqlBatchTask = CreateNewSqlBatchTask();
				}
				var result = _currentSqlBatchTask;
				if (_sqlDataAwaiters.Count >= BatchSize) {
					return RunSqlBatch();
				}
				return result;
			}
		}

		private Task<QueryResult> CreateNewSqlBatchTask() {
			return new Task<QueryResult>(() =>
				{
					Dictionary<long, Query> _myrequests;
					lock (syncsqlawait) {
						if (_sqlDataAwaiters.IsEmpty) {
							return null;
						}
						_myrequests = _sqlDataAwaiters.ToArray().Distinct().ToDictionary(_ => _.UID, _ => _);
						_sqlDataAwaiters = new ConcurrentBag<Query>();
					}
					if(TraceQuery) {
						foreach(var q in _myrequests.Values) {
							q.TraceList.Add("Session " +Id + " added to primreq ");
						}
					}
					Stopwatch sw = null;
					if (CollectStatistics) {
						sw = Stopwatch.StartNew();
						Interlocked.Increment(ref Stat_Batch_Count);
					}
					var times = _myrequests.Values.Select(_ => new {y = _.Time.Year, p = _.Time.Period}).Distinct();
					var colobj = _myrequests.Values.Select(_ => new {o = _.Obj.Id, c = _.Col.Id}).Distinct();
					var rowids = string.Join(",", _myrequests.Values.Select(_ => _.Row.Id).Distinct());
					var script =
						"select 0 as id, 0 as col, 0 as row, 0 as obj, 0 as year, 0 as period, cast(0 as decimal(18,6)) as value";
					foreach (var time in times) {
						foreach (var cobj in colobj) {
							script +=
								string.Format(
									"\r\nunion\r\nselect id,col,row,obj,year,period,decimalvalue from cell where period={0} and year={1} and col={2} and obj={3} and row in ({4})",
									time.p, time.y, cobj.c, cobj.o, rowids);
						}
					}
					using (var c = myapp.sql.GetConnection("Default")) {
						c.Open();
						var cmd = c.CreateCommand();
						cmd.CommandText = script;
						using (var r = cmd.ExecuteReader()) {
							while (r.Read()) {
								var id = r.GetInt32(0);
								if (0 == id) {
									continue;
								}
								if (CollectStatistics) {
									Interlocked.Increment(ref Stat_Primary_Catched);
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
										Interlocked.Increment(ref Stat_Primary_Affected);
									}
									if(TraceQuery) {
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
						lock (syncsqlawait) {
							Stat_Batch_Time += sw.Elapsed;
						}
					}
					foreach (var myrequest in _myrequests.Values.Where(_ => !_.HavePrimary)) {
						if (TraceQuery)
						{
							myrequest.TraceList.Add("primary not found ");
						}
						myrequest.Result = new QueryResult {IsComplete = true, IsNull = true};
					}

					return null;
				});
		}

		/// <summary>
		/// 	�������� ������� ������ �� SQL
		/// </summary>
		public Task<QueryResult> RunSqlBatch() {
			lock (syncsqlawait)
			{
				if(null!=MasterSession) {
					return MasterSession.RunSqlBatch();
				}
				var task = _currentSqlBatchTask;
				if (null == task) {
					task = CreateNewSqlBatchTask();
				}
				_currentSqlBatchTask = null;
				var id = _evalTaskCounter++;
				task.ContinueWith(t_ =>
					{
						Task t;
						_evalTaskAgenda.TryRemove(id, out t);
					});
				_evalTaskAgenda[id] = task;
				task.Start();
				return task;
			}
		}

		/// <summary>
		/// 	��������� ������������� � ������ �������� � ������
		/// </summary>
		public void Execute() {
			lock (syncexecute) {
				WaitPreparation();
				WaitEvaluation();
			}
		}

		/// <summary>
		/// 	���� ��������, ������ ����������� �������������� ������ �� ������ ������
		/// </summary>
		public readonly bool CollectStatistics;

		private readonly ConcurrentDictionary<int, Task> _evalTaskAgenda = new ConcurrentDictionary<int, Task>();

		private readonly ConcurrentStack<IPeriodEvaluator> _periodevalpool = new ConcurrentStack<IPeriodEvaluator>();
		private readonly ConcurrentDictionary<int, Task> _preEvalTaskAgenda = new ConcurrentDictionary<int, Task>();

		private readonly ConcurrentStack<IPreloadProcessor> _preloadprocesspool =
			new ConcurrentStack<IPreloadProcessor>();

		private readonly ConcurrentStack<IQueryPreparator> _preparators = new ConcurrentStack<IQueryPreparator>();

		private readonly ConcurrentStack<IRegistryHelper> _registryhelperpool = new ConcurrentStack<IRegistryHelper>();
		private readonly ConcurrentStack<ISerialSession> _subsessionpool = new ConcurrentStack<ISerialSession>();
		private readonly object syncexecute = new object();
		private object syncsqlawait = new object();

		/// <summary>
		/// 	������ �����
		/// </summary>
		public int BatchSize = 1000;

		/// <summary>
		/// 	��������� �������������� ��� ������� �����������
		/// </summary>
		public Type CustomPeriodEvaluatorClass;

		/// <summary>
		/// 	��������� �������������� ��� ������� �����������
		/// </summary>
		public Type CustomPreloadProcessorClass;

		/// <summary>
		/// 	������������� ��� �����������
		/// </summary>
		public Type CustomPreparatorClass;

		/// <summary>
		/// 	��������� �������������� ��� ������� �����������
		/// </summary>
		public Type CustomRegistryHelperClass;

		/// <summary>
		/// 	���������������� ��� SQL - ����������
		/// </summary>
		public Type CustomSqlBuilderClass;

		/// <summary>
		/// 	���������� ������
		/// </summary>
		public int Stat_Batch_Count;

		/// <summary>
		/// 	���������� ������� ������
		/// </summary>
		public TimeSpan Stat_Batch_Time;

		/// <summary>
		/// 	���������� �������������� ��������
		/// </summary>
		public int Stat_Primary_Affected;

		/// <summary>
		/// 	���������� ����������� �����
		/// </summary>
		public int Stat_Primary_Catched;

		/// <summary>
		/// 	������� ������
		/// </summary>
		public int Stat_QueryType_Formula;

		/// <summary>
		/// 	������� ��������� ��������
		/// </summary>
		public int Stat_QueryType_Primary;

		/// <summary>
		/// 	������� ����
		/// </summary>
		public int Stat_QueryType_Sum;

		/// <summary>
		/// 	������� ������������ ��������
		/// </summary>
		public int Stat_Registry_Ignored;

		/// <summary>
		/// 	���������� ������������� ���������� �����������
		/// </summary>
		public int Stat_Registry_New;

		/// <summary>
		/// 	���������� ������� �������������
		/// </summary>
		public int Stat_Registry_Preprocessed;

		/// <summary>
		/// 	���������� ����������� �� ����������� �����
		/// </summary>
		public int Stat_Registry_Resolved_By_Key;

		/// <summary>
		/// 	���������� ���������� ������������� �������� ��� ��������������
		/// </summary>
		public int Stat_Registry_Resolved_By_Map_Key;

		/// <summary>
		/// 	���������� ����������� �� ������� � ����
		/// </summary>
		public int Stat_Registry_Resolved_By_Uid;

		/// <summary>
		/// 	���������� ���������� ������� �����������
		/// </summary>
		public int Stat_Registry_Started;

		/// <summary>
		/// 	���������� ��������������� �����������
		/// </summary>
		public int Stat_Registry_Started_User;

		/// <summary>
		/// 	������� �������������� ���������� ��������
		/// </summary>
		public int Stat_Registry_User;

		/// <summary>
		/// 	������� ��������� ������
		/// </summary>
		public int Stat_Row_Redirections;

		/// <summary>
		/// 	���������� ��������� ���-������
		/// </summary>
		public int Stat_SubSession_Count;

		/// <summary>
		/// 	���������� ������ ������� ����������
		/// </summary>
		public TimeSpan Stat_Time_Total;

		/// <summary>
		/// 	������ �������� ������������ ����������������� �������
		/// </summary>
		protected internal Task<QueryResult> _async_serial_acess_task;

		private Task<QueryResult> _currentSqlBatchTask;

		/// <summary>
		/// 	������� ������� ����������
		/// </summary>
		private int _evalTaskCounter;

		private int _preEvalTaskCounter;

		private ConcurrentBag<Query> _sqlDataAwaiters = new ConcurrentBag<Query>();

		/// <summary>
		/// 	������ ���������� ��� ����������������� �������
		/// </summary>
		protected internal object _sync_serial_access_lock = new object();
	
	}
}
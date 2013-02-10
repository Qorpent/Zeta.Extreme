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
using System.Data;
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
	public class ZexSession {
		/// <summary>
		/// 	����������� �� ���������
		/// </summary>
		/// <remarks>
		/// 	���������� �������� ���������
		/// </remarks>
		public ZexSession(bool collectStatistics = false) {
			CollectStatistics = collectStatistics;
			MainQueryRegistry = new ConcurrentDictionary<string, ZexQuery>();
			ActiveSet = new ConcurrentDictionary<string, ZexQuery>();
			ProcessedSet = new ConcurrentDictionary<string, ZexQuery>();
			KeyMap = new ConcurrentDictionary<string, string>();
		}

		/// <summary>
		/// 	������� ������ ��������
		/// </summary>
		/// <remarks>
		/// 	��� ����������� ������� ������� ������������� ��� ���������� UID
		/// 	�����, � MainQueryRegistry �� ����� �� ������ Value ����� ������� ��������
		/// </remarks>
		public ConcurrentDictionary<string, ZexQuery> MainQueryRegistry { get; private set; }


		/// <summary>
		/// 	��������������� ������ ������ ����� ������� � �����������������
		/// 	��������
		/// </summary>
		public ConcurrentDictionary<string, string> KeyMap { get; private set; }

		/// <summary>
		/// 	����� ���� ����������, ��� �� ������������ �������� (������)
		/// 	���� - ������
		/// </summary>
		public ConcurrentDictionary<string, ZexQuery> ActiveSet { get; private set; }

		/// <summary>
		/// 	����� ���� ����������, ���  ������������ ��������
		/// 	���� - ������
		/// </summary>
		public ConcurrentDictionary<string, ZexQuery> ProcessedSet { get; private set; }

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
			sb.AppendLine("sqlpool:" + _sqlbuilders.Count);
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
		public ZexQuery Register(ZexQuery query, string uid = null) {
			lock (this) {
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
		public Task<ZexQuery> RegisterAsync(ZexQuery query, string uid = null) {
			lock (this) {
				var id = _preEvalTaskCounter++;
				var task = new Task<ZexQuery>(() =>
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
		public Task PrepareAsync(ZexQuery query) {
			lock (this) {
				var id = _preEvalTaskCounter++;
				var task = new Task(() =>
					{
						try {
							var preparator = GetPreparator();
							preparator.Prepare(query);
							ReturnPreparator(preparator);
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
		public void WaitPreparation() {
			while (!_preEvalTaskAgenda.IsEmpty) {
				SyncPreEval();
				Thread.Sleep(20);
			}
		}

		/// <summary>
		/// 	������� ��������� ���� ��������� ����������� �����������
		/// </summary>
		public void WaitEvaluation() {
			RunSqlBatch(); // ��������� ���������� �������
			
			Task.WaitAll(_evalTaskAgenda.Values.Where(_=>_.Status!=TaskStatus.Created).ToArray());
				//	Thread.Sleep(20);
			
			while (_evalTaskAgenda.Any(_ => _.Value.Status == TaskStatus.Created)) {
				var tasks = _evalTaskAgenda.Take(3).Select(_=>_.Value).ToArray();
				foreach (var t in tasks) {
					if(t.Status==TaskStatus.Created) {
						try{t.Start();}catch{}
					}
				}
				Task.WaitAll(tasks);
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
		public IZexQueryPreparator GetPreparator() {
			lock (this) {
				IZexQueryPreparator result;
				if (_preparators.TryPop(out result)) {
					return result;
				}
				if (null != CustomPreparatorClass) {
					return Activator.CreateInstance(CustomPreparatorClass, this) as IZexQueryPreparator;
				}
				return new DefaultZexQueryPreparator(this);
			}
		}

		/// <summary>
		/// 	���������� ������������ � ���
		/// </summary>
		/// <param name="preparator"> </param>
		private void ReturnPreparator(IZexQueryPreparator preparator) {
			_preparators.Push(preparator);
		}


		/// <summary>
		/// 	���������� ������ ���������������� ������ �����������
		/// </summary>
		/// <returns> </returns>
		/// <exception cref="NotImplementedException"></exception>
		private IZexRegistryHelper GetRegistryHelper() {
			lock (this) {
				IZexRegistryHelper result;
				if (_registryhelperpool.TryPop(out result)) {
					return result;
				}
				if (null != CustomRegistryHelperClass) {
					return Activator.CreateInstance(CustomRegistryHelperClass, this) as IZexRegistryHelper;
				}
				return new DefaultZexRegistryHelper(this);
			}
		}

		/// <summary>
		/// 	���������� ������������ � ���
		/// </summary>
		/// <param name="helper"> </param>
		private void ReturnRegistryHelper(IZexRegistryHelper helper) {
			_registryhelperpool.Push(helper);
		}

		/// <summary>
		/// 	���������� ������ �������������
		/// </summary>
		/// <returns> </returns>
		/// <exception cref="NotImplementedException"></exception>
		public IZexPreloadProcessor GetPreloadProcessor() {
			lock (this) {
				IZexPreloadProcessor result;
				if (_preloadprocesspool.TryPop(out result)) {
					return result;
				}
				if (null != CustomPreloadProcessorClass) {
					return Activator.CreateInstance(CustomRegistryHelperClass, this) as IZexPreloadProcessor;
				}
				return new DefaultZexPreloadProcessor(this);
			}
		}

		/// <summary>
		/// 	���������� ������������ � ���
		/// </summary>
		/// <param name="processor"> </param>
		public void ReturnPreloadPreprocessor(IZexPreloadProcessor processor) {
			_preloadprocesspool.Push(processor);
		}


		/// <summary>
		/// 	���������� ������ �������������
		/// </summary>
		/// <returns> </returns>
		/// <exception cref="NotImplementedException"></exception>
		public IPeriodEvaluator GetPeriodEvaluator() {
			lock (this) {
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
		public void ReturnPeriodEvaluator(IPeriodEvaluator periodEvaluator) {
			_periodevalpool.Push(periodEvaluator);
		}

		/// <summary>
		/// 	����������� � ������ ������ �� ����������
		/// </summary>
		/// <param name="resulttask"> </param>
		/// <param name="hot"> ����������� ������ </param>
		/// <returns> </returns>
		public Task<QueryResult> RegisterEvalTask(Func<QueryResult> resulttask, bool hot) {
			lock (this) {
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
		public void SyncPreEval() {
			Task.WaitAll(_preEvalTaskAgenda.Values.ToArray());
		}

		/// <summary>
		/// 	���������� ������ �������������
		/// </summary>
		/// <returns> </returns>
		/// <exception cref="NotImplementedException"></exception>
		public IZexSqlBuilder GetSqlBuilder() {
			lock (this) {
				IZexSqlBuilder result;
				if (_sqlbuilders.TryPop(out result)) {
					return result;
				}
				if (null != CustomSqlBuilderClass) {
					return Activator.CreateInstance(CustomSqlBuilderClass, this) as IZexSqlBuilder;
				}
				return new DefaultZexSqlBuilder(this);
			}
		}

		/// <summary>
		/// 	���������� ������������ � ���
		/// </summary>
		/// <param name="sqlbuilder"> </param>
		public void ReturnPreloadPreprocessor(IZexSqlBuilder sqlbuilder) {
			_sqlbuilders.Push(sqlbuilder);
		}

		/// <summary>
		/// 	���� ��������, ������ ����������� �������������� ������ �� ������ ������
		/// </summary>
		public readonly bool CollectStatistics;

		private readonly ConcurrentDictionary<int, Task> _evalTaskAgenda = new ConcurrentDictionary<int, Task>();

		private readonly ConcurrentStack<IPeriodEvaluator> _periodevalpool = new ConcurrentStack<IPeriodEvaluator>();
		private readonly ConcurrentDictionary<int, Task> _preEvalTaskAgenda = new ConcurrentDictionary<int, Task>();

		private readonly ConcurrentStack<IZexPreloadProcessor> _preloadprocesspool =
			new ConcurrentStack<IZexPreloadProcessor>();

		private readonly ConcurrentStack<IZexQueryPreparator> _preparators = new ConcurrentStack<IZexQueryPreparator>();

		private readonly ConcurrentStack<IZexRegistryHelper> _registryhelperpool = new ConcurrentStack<IZexRegistryHelper>();
		private readonly ConcurrentStack<IZexSqlBuilder> _sqlbuilders = new ConcurrentStack<IZexSqlBuilder>();

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
		/// 	������� ������� ����������
		/// </summary>
		private int _evalTaskCounter;

		private int _preEvalTaskCounter;

		/// <summary>
		/// ����������� ������ � ���������� SQL �������� � ������� �� ���������� ��������
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public Task<QueryResult> RegisterSqlRequest(ZexQuery query) {
			lock(syncsqlawait) {
				
				_sqlDataAwaiters.Add(query);
				
				if(null==_currentSqlBatchTask) {
					_currentSqlBatchTask = CreateNewSqlBatchTask();
				}
				
				if(_sqlDataAwaiters.Count>=BatchSize) {
					return RunSqlBatch();
				}
				return _currentSqlBatchTask;
			}
		}

		/// <summary>
		/// ������ �����
		/// </summary>
		public int BatchSize = 1000;

		object syncsqlawait = new object();
		private Task<QueryResult> CreateNewSqlBatchTask() {
			return new Task<QueryResult>(() =>
				{
					
					
					Dictionary<long, ZexQuery> _myrequests;
					lock(syncsqlawait) {
						if(_sqlDataAwaiters.IsEmpty) return null;
						_myrequests = _sqlDataAwaiters.ToArray().Distinct().ToDictionary(_ => _.UID, _ => _);
						_sqlDataAwaiters = new ConcurrentBag<ZexQuery>();
					}
					Stopwatch sw = null;
					if (CollectStatistics)
					{
						sw = Stopwatch.StartNew();
						Interlocked.Increment(ref Stat_Batch_Count);
					}
					var times = _myrequests.Values.Select(_ => new {  y = _.Time.Year, p = _.Time.Period }).Distinct();
					var colobj = _myrequests.Values.Select(_ => new {o = _.Obj.Id,c = _.Col.Id}).Distinct();
					var rowids = string.Join(",", _myrequests.Values.Select(_ => _.Row.Id).Distinct());
					var script = "select 0 as id, 0 as col, 0 as row, 0 as obj, 0 as year, 0 as period, cast(0 as decimal(18,6)) as value";
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
							while(r.Read()) {
								var id = r.GetInt32(0);
								if(0==id)continue;
								if (CollectStatistics)
								{
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
								if(null!=target) {
									if(CollectStatistics) {
										Interlocked.Increment(ref Stat_Primary_Affected);
									}
									target.Result = new QueryResult {IsComplete = true, NumericResult = value, CellId = id};
								}
							}
						}
					}


					if(CollectStatistics) {
						sw.Stop();
						lock (syncsqlawait) {
							Stat_Batch_Time += sw.Elapsed;
						}
					}
					foreach (var myrequest in _myrequests.Values.Where(_=>null==_.Result)) {
						myrequest.Result = new QueryResult {IsComplete = true, IsNull = true};

					}

					return null;
				});
		}

		/// <summary>
		/// �������� ������� ������ �� SQL
		/// </summary>
		public Task<QueryResult> RunSqlBatch() {
			lock (this) {
				var task = _currentSqlBatchTask;
				if(null==task) {
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

		ConcurrentBag<ZexQuery> _sqlDataAwaiters  = new ConcurrentBag<ZexQuery>();
		private Task<QueryResult> _currentSqlBatchTask;
		/// <summary>
		/// ���������� ������
		/// </summary>
		public int Stat_Batch_Count;
		/// <summary>
		/// ���������� ������� ������
		/// </summary>
		public TimeSpan Stat_Batch_Time;
		/// <summary>
		/// ���������� ������ ������� ����������
		/// </summary>
		public TimeSpan Stat_Time_Total;
		/// <summary>
		/// ���������� ����������� �����
		/// </summary>
		public int Stat_Primary_Catched;

	/// <summary>
	/// ���������� �������������� ��������
	/// </summary>
		public int Stat_Primary_Affected;
	}
}
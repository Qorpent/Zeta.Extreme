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
	public class Session {
		private static int ID;
		/// <summary>
		/// ������� ������ ����������� ��������
		/// </summary>
		public bool TraceQuery = false;



		/// <summary>
		/// ��������� ��������� ������
		/// </summary>
		public IPrimarySource PrimarySource {
			get { return _primarySource; }
			set { _primarySource = value; }
		}


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
			PrimarySource = new DefaultPrimarySource(this);
		}

		/// <summary>
		/// ��������� ��� ��������� ������
		/// </summary>
		public IMetaCache MetaCache {
			get {
				lock (this) {
					if(null!=MasterSession) {
						return MasterSession.MetaCache;
					}
					return _metaCache ?? (_metaCache = Extreme.MetaCache.Default);
				}
				
			}
			set {
				lock(this) {
					if (null != MasterSession) {
						throw new Exception("cannot set on child session");
					}
					_metaCache = value;
				}
			}
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
					return result;
				}
				var copy = new Session(CollectStatistics)
					{
						Registry = Registry,
						ActiveSet = ActiveSet,
						KeyMap = KeyMap,
						MasterSession = this,
						PrimarySource = PrimarySource,
						TraceQuery = TraceQuery,
						thissync = thissync,
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
		/// <param name="timeout"> </param>
		protected internal void WaitPreparation(int timeout=-1) {
			
			while (!_preEvalTaskAgenda.IsEmpty) {
				SyncPreEval(timeout);
				Thread.Sleep(20);
			}

		
		}
		

		/// <summary>
		/// 	������� ��������� ���� ��������� ����������� �����������
		/// </summary>
		/// <param name="timeout"> </param>
		protected internal void WaitEvaluation(int timeout=-1) {
			PrimarySource.Wait();
			ActiveSet.Values.AsParallel().Where(_=>null==_.Result).ForAll(_=>_.GetResult());
			ActiveSet.Clear();
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
				return new QueryProcessor(this);
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
				return new QuerySessionRegistrator(this);
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
				return new QueryLoader(this);
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
		/// 	������ �������������� ���������� ����� � �������� �������� ����������
		/// </summary>
		/// <param name="timeout"> </param>
		protected internal void SyncPreEval(int timeout) {
			if(timeout>0) {
				Task.WaitAll(_preEvalTaskAgenda.Values.ToArray(), timeout);
			}else {
				Task.WaitAll(_preEvalTaskAgenda.Values.ToArray());
			}
		}

	

		

	

		/// <summary>
		/// 	��������� ������������� � ������ �������� � ������
		/// </summary>
		/// <param name="timeout"> </param>
		public void Execute(int timeout =-1) {
			lock (syncexecute) {
				WaitPreparation(timeout);
				WaitEvaluation(timeout);
			}
		}

		/// <summary>
		/// 	���� ��������, ������ ����������� �������������� ������ �� ������ ������
		/// </summary>
		public readonly bool CollectStatistics;

		

		private readonly ConcurrentStack<IPeriodEvaluator> _periodevalpool = new ConcurrentStack<IPeriodEvaluator>();
		private readonly ConcurrentDictionary<int, Task> _preEvalTaskAgenda = new ConcurrentDictionary<int, Task>();

		private readonly ConcurrentStack<IPreloadProcessor> _preloadprocesspool =
			new ConcurrentStack<IPreloadProcessor>();

		private readonly ConcurrentStack<IQueryPreparator> _preparators = new ConcurrentStack<IQueryPreparator>();

		private readonly ConcurrentStack<IRegistryHelper> _registryhelperpool = new ConcurrentStack<IRegistryHelper>();
		private readonly ConcurrentStack<ISerialSession> _subsessionpool = new ConcurrentStack<ISerialSession>();
		private readonly object syncexecute = new object();
		

		

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

		

		

		private int _preEvalTaskCounter;

		

		/// <summary>
		/// 	������ ���������� ��� ����������������� �������
		/// </summary>
		protected internal object _sync_serial_access_lock = new object();

		private IMetaCache _metaCache;
		private IPrimarySource _primarySource;
	}
}
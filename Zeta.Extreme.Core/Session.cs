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
// PROJECT ORIGIN: Zeta.Extreme.Core/Session.cs

#endregion

using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.Querying;
using Zeta.Extreme.Primary;

namespace Zeta.Extreme {
	/// <summary>
	///     ������� ����� ������ �������� Zeta
	/// </summary>
	/// <remarks>
	///     ������ �������� �������� �������� ����� ��������� �������. ����� �������
	///     ������ � �������: create session ==- register queries ==- evaluate ==-
	///     collect result ������ �������� � ������������ �������������� async -
	///     �����������
	/// </remarks>
	public class Session : ISerializableSession, IWithSessionStatistics, IWithDataServices, IWithQueryRegistry,
	                       ISessionWithExtendedServices {
		private static int ID;

		/// <summary>
		///     ����������� �� ���������
		/// </summary>
		/// <remarks>
		///     ���������� �������� ���������
		/// </remarks>
		public Session(bool collectStatistics = false) {
			Id = ++ID;
			CollectStatistics = collectStatistics;
			if (CollectStatistics) {
				Statistics = new SessionStatistics();
			}
			Registry = new ConcurrentDictionary<string, IQuery>();
			ActiveSet = new ConcurrentDictionary<string, IQuery>();
			KeyMap = new ConcurrentDictionary<string, string>();
			PrimarySource = new DefaultPrimarySource(this);
		}

		/// <summary>
		///     ���������� ������������� ������ � ��������
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		///     ������������ ������
		/// </summary>
		protected internal ISession MasterSession { get; set; }


		/// <summary>
		///     ���������� �������������
		/// </summary>
		public object SerialSync {
			get { return _sync_serial_access_lock; }
		}

		/// <summary>
		///     ������ ��� ���������� � ����������� ������ �� ���������������� �������
		/// </summary>
		public Task<QueryResult> SerialTask {
			get { return _async_serial_acess_task; }
			set { _async_serial_acess_task = value; }
		}

		/// <summary>
		/// </summary>
		/// <param name="key"></param>
		/// <param name="timeout"></param>
		/// <returns>
		/// </returns>
		public QueryResult Get(string key, int timeout = -1) {
			IQuery query;
			Registry.TryGetValue(key, out query);
			if (null == query) {
				return new QueryResult {IsComplete = false};
			}
			var processable = query as IQueryWithProcessing;
			if (null != processable) {
				return processable.GetResult(timeout);
			}
			return query.Result ?? new QueryResult();
		}

		/// <summary>
		///     ���������� ����������� ������� � ������
		/// </summary>
		/// <remarks>
		///     ��� ����������� �������, �� �������� �������������� ����������� � ��������
		///     �� ������, ������������ ������ �������� ������
		/// </remarks>
		/// <param name="query">�������� ������</param>
		/// <param name="uid">
		///     ��������� ���� ������� ��������� ��� ��� ����������� ����������������
		///     ��������� ��������
		/// </param>
		/// <exception cref="NotImplementedException" />
		/// <returns>
		///     ������ �� ������ ����������� � ������
		/// </returns>
		public IQuery Register(IQuery query, string uid = null) {
			//lock (thissync) {
			var helper = GetRegistryHelper();
			var result = helper.Register(query, uid);
			ReturnRegistryHelper(helper);
			return result;
			//}
		}

		/// <summary>
		///     ����������� ����������� ������� � ������
		/// </summary>
		/// <remarks>
		///     ��� ����������� �������, �� �������� �������������� ����������� � ��������
		///     �� ������, ������������ ������ �������� ������
		/// </remarks>
		/// <param name="query">�������� ������</param>
		/// <param name="uid">
		///     ��������� ���� ������� ��������� ��� ��� ����������� ����������������
		///     ��������� ��������
		/// </param>
		/// <returns>
		///     ������, �� ����������� ������� ������������ ������ �� ������ ����������� �
		///     ������
		/// </returns>
		public Task<IQuery> RegisterAsync(IQuery query, string uid = null) {
			lock (thissync) {
				var id = _preEvalTaskCounter++;
				var task = new Task<IQuery>(() =>
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
		///     ��������� ������������� � ������ �������� � ������
		/// </summary>
		/// <param name="timeout"></param>
		public void Execute(int timeout = -1) {
			lock (syncexecute) {
				WaitPreparation(timeout);
				WaitEvaluation(timeout);
			}
		}

		/// <summary>
		/// �������� �������������� ����������
		/// </summary>
		public ISessionPropertySource PropertySource { get; set; }


		/// <summary>
		///     ���������� ����������� ���������� ������� � ���������� ���������� �� ��
		///     ������, ��� � �����������
		/// </summary>
		/// <param name="query"></param>
		/// <returns>
		/// </returns>
		public Task PrepareAsync(IQuery query) {
			lock (thissync) {
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
		///     ������� ��������� ���� ��������� ����������� �����������
		/// </summary>
		/// <param name="timeout"></param>
		public void WaitPreparation(int timeout = -1) {
			while (!_preEvalTaskAgenda.IsEmpty) {
				SyncPreEval(timeout);
				Thread.Sleep(20);
			}
		}


		/// <summary>
		///     ������� ��������� ���� ��������� ����������� �����������
		/// </summary>
		/// <param name="timeout"></param>
		public void WaitEvaluation(int timeout = -1) {
			PrimarySource.Wait();
			ActiveSet.Values.Cast<IQueryWithProcessing>().AsParallel().Where(_ => null == _.Result).ForAll(_ => _.GetResult());
			ActiveSet.Clear();
		}

		/// <summary>
		///     ���������� ������ ���������������� ������ �����������
		/// </summary>
		/// <exception cref="NotImplementedException" />
		/// <returns>
		/// </returns>
		public IQueryPreparator GetPreparator() {
			//lock (thissync) {
			IQueryPreparator result;
			if (_preparators.TryPop(out result)) {
				return result;
			}
			if (null != CustomPreparatorClass) {
				return Activator.CreateInstance(CustomPreparatorClass, this) as IQueryPreparator;
			}
			return new QueryProcessor(this);
			//}
		}

		/// <summary>
		///     ���������� ������������ � ���
		/// </summary>
		/// <param name="preparator"></param>
		public void Return(IQueryPreparator preparator) {
			_preparators.Push(preparator);
		}


		/// <summary>
		///     ���������� ������ ���������������� ������ �����������
		/// </summary>
		/// <exception cref="NotImplementedException" />
		/// <returns>
		/// </returns>
		public IRegistryHelper GetRegistryHelper() {
			//	lock (thissync) {
			IRegistryHelper result;
			if (_registryhelperpool.TryPop(out result)) {
				return result;
			}
			if (null != CustomRegistryHelperClass) {
				return Activator.CreateInstance(CustomRegistryHelperClass, this) as IRegistryHelper;
			}
			return new QuerySessionRegistrator(this);
			//}
		}


		/// <summary>
		///     ���������� ������������ � ���
		/// </summary>
		/// <param name="helper"></param>
		public void ReturnRegistryHelper(IRegistryHelper helper) {
			_registryhelperpool.Push(helper);
		}

		/// <summary>
		///     ���������� ������ �������������
		/// </summary>
		/// <exception cref="NotImplementedException" />
		/// <returns>
		/// </returns>
		public IPreloadProcessor GetPreloadProcessor() {
			//	lock (thissync) {
			IPreloadProcessor result;
			if (_preloadprocesspool.TryPop(out result)) {
				return result;
			}
			if (null != CustomPreloadProcessorClass) {
				return Activator.CreateInstance(CustomRegistryHelperClass, this) as IPreloadProcessor;
			}
			return new QueryLoader(this);
			//	}
		}

		/// <summary>
		///     ���������� ������������ � ���
		/// </summary>
		/// <param name="processor"></param>
		public void Return(IPreloadProcessor processor) {
			_preloadprocesspool.Push(processor);
		}


		/// <summary>
		///     ������ �������������� ���������� ����� � �������� �������� ����������
		/// </summary>
		/// <param name="timeout"></param>
		public void SyncPreEval(int timeout) {
			if (timeout > 0) {
				Task.WaitAll(_preEvalTaskAgenda.Values.ToArray(), timeout);
			}
			else {
				Task.WaitAll(_preEvalTaskAgenda.Values.ToArray());
			}
		}

		/// <summary>
		///     ��������� ��������� ������
		/// </summary>
		public IPrimarySource PrimarySource { get; set; }

		/// <summary>
		///     ������� ���������� �����, ��������� � ���������� �������
		/// </summary>
		/// <param name="timeout"></param>
		public void WaitPrimarySource(int timeout = -1) {
			PrimarySource.Wait();
		}

		/// <summary>
		///     ��������� ��� ��������� ������
		/// </summary>
		public IMetaCache MetaCache {
			get {
				lock (this) {
					if (null != MasterSession) {
						return MasterSession.GetMetaCache();
					}
					return _metaCache ?? (_metaCache = new MetaCache {Parent = Model.MetaCache.Default});
				}
			}
			set {
				lock (this) {
					if (null != MasterSession) {
						throw new Exception("cannot set on child session");
					}
					_metaCache = value;
				}
			}
		}

		/// <summary>
		///     ������� ������ ��������
		/// </summary>
		/// <remarks>
		///     ��� ����������� ������� ������� ������������� ��� ���������� UID �����, �
		///     MainQueryRegistry �� ����� �� ������ Value ����� ������� ��������
		/// </remarks>
		public ConcurrentDictionary<string, IQuery> Registry { get; private set; }

		/// <summary>
		///     ��������������� ������ ������ ����� ������� � ����������������� ��������
		/// </summary>
		public ConcurrentDictionary<string, string> KeyMap { get; private set; }

		/// <summary>
		///     ����� ���� ����������, ��� �� ������������ �������� (������) ���� - ������
		/// </summary>
		public ConcurrentDictionary<string, IQuery> ActiveSet { get; private set; }

		/// <summary>
		///     ������ � ���������� ������
		/// </summary>
		public SessionStatistics Statistics { get; set; }


		/// <summary>
		///     ���� ��������, ������ ����������� �������������� ������ �� ������ ������
		/// </summary>
		public bool CollectStatistics { get; private set; }

		/// <summary>
		///     ��������� ������� �������������� ��������� � ���
		/// </summary>
		/// <param name="session"></param>
		public void Return(ISerialSession session) {
			_subsessionpool.Push(session);
		}


		/// <summary>
		///     ���������� ��������� � ���������� ������
		/// </summary>
		/// <returns>
		/// </returns>
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
					sb.AppendLine("Subsession: " + ((Session) ses).Id);
					sb.Append(((Session) ses).GetStatisticString());
					sb.AppendLine("--------------------------");
				}
			}
			return sb.ToString();
		}

		/// <summary>
		///     ���������� ������������ � ���
		/// </summary>
		/// <param name="periodEvaluator"></param>
		public void Return(IPeriodEvaluator periodEvaluator) {
			_periodevalpool.Push(periodEvaluator);
		}


		private readonly ConcurrentStack<IPeriodEvaluator> _periodevalpool = new ConcurrentStack<IPeriodEvaluator>();
		private readonly ConcurrentDictionary<int, Task> _preEvalTaskAgenda = new ConcurrentDictionary<int, Task>();

		private readonly ConcurrentStack<IPreloadProcessor> _preloadprocesspool =
			new ConcurrentStack<IPreloadProcessor>();

		private readonly ConcurrentStack<IQueryPreparator> _preparators = new ConcurrentStack<IQueryPreparator>();

		private readonly ConcurrentStack<IRegistryHelper> _registryhelperpool = new ConcurrentStack<IRegistryHelper>();
		private readonly ConcurrentStack<ISerialSession> _subsessionpool = new ConcurrentStack<ISerialSession>();
		private readonly object syncexecute = new object();
		private readonly object thissync = new object();


		/// <summary>
		///     ��������� �������������� ��� ������� �����������
		/// </summary>
		public Type CustomPeriodEvaluatorClass;

		/// <summary>
		///     ��������� �������������� ��� ������� �����������
		/// </summary>
		public Type CustomPreloadProcessorClass;

		/// <summary>
		///     ������������� ��� �����������
		/// </summary>
		public Type CustomPreparatorClass;

		/// <summary>
		///     ��������� �������������� ��� ������� �����������
		/// </summary>
		public Type CustomRegistryHelperClass;

		/// <summary>
		///     ���������������� ��� SQL - ����������
		/// </summary>
		public Type CustomSqlBuilderClass;


		/// <summary>
		///     ������� ������ ����������� ��������
		/// </summary>
		public bool TraceQuery = false;

		/// <summary>
		///     ������ �������� ������������ ����������������� �������
		/// </summary>
		protected internal Task<QueryResult> _async_serial_acess_task;

		private IMetaCache _metaCache;


		private int _preEvalTaskCounter;


		/// <summary>
		///     ������ ���������� ��� ����������������� �������
		/// </summary>
		protected internal object _sync_serial_access_lock = new object();
	}
}
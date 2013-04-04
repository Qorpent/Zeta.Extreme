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
	///     Базовый класс сессии расчетов Zeta
	/// </summary>
	/// <remarks>
	///     Сессия является ключевым объектом Новой расчетной системы. Общий паттерн
	///     работы с сессией: create session ==- register queries ==- evaluate ==-
	///     collect result Сессия работает с максимальным использованием async -
	///     оптимизации
	/// </remarks>
	public class Session : ISerializableSession, IWithSessionStatistics, IWithDataServices, IWithQueryRegistry,
	                       ISessionWithExtendedServices {
		private static int ID;

		/// <summary>
		///     Конструктор по умолчанию
		/// </summary>
		/// <remarks>
		///     Инициирует основные коллекции
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
		///     Уникальный идентификатор сессии в процессе
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		///     Родительская сессия
		/// </summary>
		protected internal ISession MasterSession { get; set; }


		/// <summary>
		///     Сериальная синхронизация
		/// </summary>
		public object SerialSync {
			get { return _sync_serial_access_lock; }
		}

		/// <summary>
		///     Задача для выполнения в асинхронном режиме из сериализованного доступа
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
		///     Синхронная регистрация запроса в сессии
		/// </summary>
		/// <remarks>
		///     При регистрации запроса, он проходит дополнительную оптимизацию и проверку
		///     на дубляж, возвращается именно итоговый запрос
		/// </remarks>
		/// <param name="query">исходный запрос</param>
		/// <param name="uid">
		///     позволяет явно указать словарный код для составления синхронизируемой
		///     коллекции запросов
		/// </param>
		/// <exception cref="NotImplementedException" />
		/// <returns>
		///     запрос по итогам регистрации в сессии
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
		///     Асинхронная регистрация запроса в сессии
		/// </summary>
		/// <remarks>
		///     При регистрации запроса, он проходит дополнительную оптимизацию и проверку
		///     на дубляж, возвращается именно итоговый запрос
		/// </remarks>
		/// <param name="query">исходный запрос</param>
		/// <param name="uid">
		///     позволяет явно указать словарный код для составления синхронизируемой
		///     коллекции запросов
		/// </param>
		/// <returns>
		///     задачу, по результатам которой возвращается запрос по итогам регистрации в
		///     сессии
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
				//должны делать в основном потоке, иначе 
				//WaitRegistry может раньше отработать
				task.Start();
				return task;
			}
		}

		/// <summary>
		///     Выполняет синхронизацию и расчет значений в сессии
		/// </summary>
		/// <param name="timeout"></param>
		public void Execute(int timeout = -1) {
			lock (syncexecute) {
				WaitPreparation(timeout);
				WaitEvaluation(timeout);
			}
		}

		/// <summary>
		/// Источник дополнительных параметров
		/// </summary>
		public ISessionPropertySource PropertySource { get; set; }


		/// <summary>
		///     Производит асинхронную подготовку запроса к выполнению использует ту же
		///     агенду, что и регистрация
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
				//должны делать в основном потоке, иначе 
				//WaitRegistry может раньше отработать
				task.Start();
				return task;
			}
		}


		/// <summary>
		///     Ожидает окончания всех процессов асинхронной регистрации
		/// </summary>
		/// <param name="timeout"></param>
		public void WaitPreparation(int timeout = -1) {
			while (!_preEvalTaskAgenda.IsEmpty) {
				SyncPreEval(timeout);
				Thread.Sleep(20);
			}
		}


		/// <summary>
		///     Ожидает окончания всех процессов асинхронной регистрации
		/// </summary>
		/// <param name="timeout"></param>
		public void WaitEvaluation(int timeout = -1) {
			PrimarySource.Wait();
			ActiveSet.Values.Cast<IQueryWithProcessing>().AsParallel().Where(_ => null == _.Result).ForAll(_ => _.GetResult());
			ActiveSet.Clear();
		}

		/// <summary>
		///     Возвращает объект вспомогательного класса регистрации
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
		///     Возвращает препроцессор в пул
		/// </summary>
		/// <param name="preparator"></param>
		public void Return(IQueryPreparator preparator) {
			_preparators.Push(preparator);
		}


		/// <summary>
		///     Возвращает объект вспомогательного класса регистрации
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
		///     Возвращает препроцессор в пул
		/// </summary>
		/// <param name="helper"></param>
		public void ReturnRegistryHelper(IRegistryHelper helper) {
			_registryhelperpool.Push(helper);
		}

		/// <summary>
		///     Возвращает объект препроцессора
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
		///     Возвращает препроцессор в пул
		/// </summary>
		/// <param name="processor"></param>
		public void Return(IPreloadProcessor processor) {
			_preloadprocesspool.Push(processor);
		}


		/// <summary>
		///     Быстро синхронизирует вызывающий поток с текущими задачами подготовки
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
		///     Расчетчик первичных данных
		/// </summary>
		public IPrimarySource PrimarySource { get; set; }

		/// <summary>
		///     Ожидает завершения задач, связанных с первичными данными
		/// </summary>
		/// <param name="timeout"></param>
		public void WaitPrimarySource(int timeout = -1) {
			PrimarySource.Wait();
		}

		/// <summary>
		///     Локальный кэш объектных данных
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
		///     Главный реестр запросов
		/// </summary>
		/// <remarks>
		///     При регистрации каждому запросу присваивается или передается UID здесь, в
		///     MainQueryRegistry мы можем на уровне Value иметь дубляжи запросов
		/// </remarks>
		public ConcurrentDictionary<string, IQuery> Registry { get; private set; }

		/// <summary>
		///     Оптимизационный мапинг ключей между входным и отпрепроцессорным запросом
		/// </summary>
		public ConcurrentDictionary<string, string> KeyMap { get; private set; }

		/// <summary>
		///     Набор всех уникальных, еще не обработанных запросов (агенда) ключ - хэшкей
		/// </summary>
		public ConcurrentDictionary<string, IQuery> ActiveSet { get; private set; }

		/// <summary>
		///     Доступ к статистике сессии
		/// </summary>
		public SessionStatistics Statistics { get; set; }


		/// <summary>
		///     Если включено, службы накапливают статистические данные по работе сессии
		/// </summary>
		public bool CollectStatistics { get; private set; }

		/// <summary>
		///     Позволяет вернуть использованную подсессию в пул
		/// </summary>
		/// <param name="session"></param>
		public void Return(ISerialSession session) {
			_subsessionpool.Push(session);
		}


		/// <summary>
		///     Возвращает сообщение о статистике работы
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
		///     Возвращает препроцессор в пул
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
		///     Позволяет переопределить тип хелпера регистрации
		/// </summary>
		public Type CustomPeriodEvaluatorClass;

		/// <summary>
		///     Позволяет переопределить тип хелпера регистрации
		/// </summary>
		public Type CustomPreloadProcessorClass;

		/// <summary>
		///     Нестандартный тип препаратора
		/// </summary>
		public Type CustomPreparatorClass;

		/// <summary>
		///     Позволяет переопределить тип хелпера регистрации
		/// </summary>
		public Type CustomRegistryHelperClass;

		/// <summary>
		///     Пользовательский тип SQL - генератора
		/// </summary>
		public Type CustomSqlBuilderClass;


		/// <summary>
		///     Ведение полной трассировки запросов
		/// </summary>
		public bool TraceQuery = false;

		/// <summary>
		///     Задача текущего асинхронного последовательного доступа
		/// </summary>
		protected internal Task<QueryResult> _async_serial_acess_task;

		private IMetaCache _metaCache;


		private int _preEvalTaskCounter;


		/// <summary>
		///     Объект блокировки для последовательного доступа
		/// </summary>
		protected internal object _sync_serial_access_lock = new object();
	}
}
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
	/// 	Базовый класс сессии расчетов Zeta
	/// </summary>
	/// <remarks>
	/// 	Сессия является ключевым объектом
	/// 	Новой расчетной системы.
	/// 	Общий паттерн работы с сессией:
	/// 	create session ==- register queries ==- evaluate  ==- collect result
	/// 	Сессия работает с максимальным использованием async - оптимизации
	/// </remarks>
	public class ZexSession {
		private static int ID;
		/// <summary>
		/// Ведение полной трассировки запросов
		/// </summary>
		public bool TraceQuery = false;

		/// <summary>
		/// 	Конструктор по умолчанию
		/// </summary>
		/// <remarks>
		/// 	Инициирует основные коллекции
		/// </remarks>
		public ZexSession(bool collectStatistics = false) {
			Id = ++ID;
			CollectStatistics = collectStatistics;
			Registry = new ConcurrentDictionary<string, ZexQuery>();
			ActiveSet = new ConcurrentDictionary<string, ZexQuery>();
			KeyMap = new ConcurrentDictionary<string, string>();
		}

		

		/// <summary>
		/// 	Уникальный идентификатор сессии в процессе
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// 	Главный реестр запросов
		/// </summary>
		/// <remarks>
		/// 	При регистрации каждому запросу присваивается или передается UID
		/// 	здесь, в MainQueryRegistry мы можем на уровне Value иметь дубляжи запросов
		/// </remarks>
		public ConcurrentDictionary<string, ZexQuery> Registry { get; private set; }

		/// <summary>
		/// 	Родительская сессия
		/// </summary>
		protected internal ZexSession MasterSession { get; set; }


		/// <summary>
		/// 	Оптимизационный мапинг ключей между входным и отпрепроцессорным
		/// 	запросом
		/// </summary>
		protected internal ConcurrentDictionary<string, string> KeyMap { get; private set; }

		/// <summary>
		/// 	Набор всех уникальных, еще не обработанных запросов (агенда)
		/// 	ключ - хэшкей
		/// </summary>
		protected internal ConcurrentDictionary<string, ZexQuery> ActiveSet { get; private set; }

		/// <summary>
		/// 	Формирует дочернюю подсессию (например для формул)
		/// 	Дочерняя сессия имеет доступ к кэшу запросов,
		/// 	но задача обработки этих запросов полностью ложится на дочку
		/// </summary>
		/// <returns> </returns>
		public ISerialSession GetSubSession() {
			lock (this) {
				ISerialSession result;
				if (_subsessionpool.TryPop(out result)) {
					result.GetUnderlinedSession()._preEvalTaskAgenda.Clear();
					result.GetUnderlinedSession()._evalTaskAgenda.Clear();
					return result;
				}
				var copy = new ZexSession(CollectStatistics)
					{
						Registry = Registry,
						ActiveSet = ActiveSet,
						KeyMap = KeyMap,
						MasterSession = this,
						TraceQuery = TraceQuery,
						//_register_lock =  this._register_lock
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

		private IDictionary<int, ZexSession> _subsessions = new Dictionary<int, ZexSession>();

		/// <summary>
		/// 	Позволяет вернуть использованную подсессию в пул
		/// </summary>
		/// <param name="session"> </param>
		protected internal void Return(ISerialSession session) {
			_subsessionpool.Push(session);
		}


		/// <summary>
		/// 	Возвращает сообщение о статистике работы
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
		/// 	Синхронная регистрация запроса в сессии
		/// </summary>
		/// <param name="query"> исходный запрос </param>
		/// <param name="uid"> позволяет явно указать словарный код для составления синхронизируемой коллекции запросов </param>
		/// <returns> запрос по итогам регистрации в сессии </returns>
		/// <remarks>
		/// 	При регистрации запроса, он проходит дополнительную оптимизацию и проверку на дубляж,
		/// 	возвращается именно итоговый запрос
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
		/// 	Асинхронная регистрация запроса в сессии
		/// </summary>
		/// <param name="query"> исходный запрос </param>
		/// <param name="uid"> позволяет явно указать словарный код для составления синхронизируемой коллекции запросов </param>
		/// <returns> задачу, по результатам которой возвращается запрос по итогам регистрации в сессии </returns>
		/// <remarks>
		/// 	При регистрации запроса, он проходит дополнительную оптимизацию и проверку на дубляж,
		/// 	возвращается именно итоговый запрос
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
				//должны делать в основном потоке, иначе 
				//WaitRegistry может раньше отработать
				task.Start();
				return task;
			}
		}

		/// <summary>
		/// 	Производит асинхронную подготовку запроса к выполнению
		/// 	использует ту же агенду, что и регистрация
		/// </summary>
		/// <param name="query"> </param>
		/// <returns> </returns>
		protected internal Task PrepareAsync(ZexQuery query) {
			lock (this) {
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
		/// 	Ожидает окончания всех процессов асинхронной регистрации
		/// </summary>
		protected internal void WaitPreparation() {
			
			while (!_preEvalTaskAgenda.IsEmpty) {
				SyncPreEval();
				Thread.Sleep(20);
			}

		
		}

		/// <summary>
		/// 	Ожидает окончания всех процессов асинхронной регистрации
		/// </summary>
		protected internal void WaitEvaluation() {
			RunSqlBatch(); // выполняем остаточные запросы

			Task.WaitAll(_evalTaskAgenda.Values.Where(_ => _.Status != TaskStatus.Created).ToArray());
			//	Thread.Sleep(20);

			while (_evalTaskAgenda.Any()) {
				// так как это поздние задачи и по идее не длительные,
				// то мы разбираем их как очередь, без распаралелливания
				// при этом подзадачи каскадом активируются сами по формулам
				// и суммам через WaitResult на дочках
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
		/// 	Возвращает объект вспомогательного класса регистрации
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
		/// 	Возвращает препроцессор в пул
		/// </summary>
		/// <param name="preparator"> </param>
		private void Return(IZexQueryPreparator preparator) {
			_preparators.Push(preparator);
		}


		/// <summary>
		/// 	Возвращает объект вспомогательного класса регистрации
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
		/// 	Возвращает препроцессор в пул
		/// </summary>
		/// <param name="helper"> </param>
		private void ReturnRegistryHelper(IZexRegistryHelper helper) {
			_registryhelperpool.Push(helper);
		}

		/// <summary>
		/// 	Возвращает объект препроцессора
		/// </summary>
		/// <returns> </returns>
		/// <exception cref="NotImplementedException"></exception>
		protected internal IZexPreloadProcessor GetPreloadProcessor() {
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
		/// 	Возвращает препроцессор в пул
		/// </summary>
		/// <param name="processor"> </param>
		protected internal void Return(IZexPreloadProcessor processor) {
			_preloadprocesspool.Push(processor);
		}


		/// <summary>
		/// 	Возвращает объект препроцессора
		/// </summary>
		/// <returns> </returns>
		/// <exception cref="NotImplementedException"></exception>
		protected internal IPeriodEvaluator GetPeriodEvaluator() {
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
		/// 	Возвращает препроцессор в пул
		/// </summary>
		/// <param name="periodEvaluator"> </param>
		protected internal void Return(IPeriodEvaluator periodEvaluator) {
			_periodevalpool.Push(periodEvaluator);
		}

		/// <summary>
		/// 	Регистриует в агенде задачу на вычисление
		/// </summary>
		/// <param name="resulttask"> </param>
		/// <param name="hot"> немедленный запуск </param>
		/// <returns> </returns>
		protected internal Task<QueryResult> RegisterEvalTask(Func<QueryResult> resulttask, bool hot) {
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
				//должны делать в основном потоке, иначе 
				//WaitRegistry может раньше отработать

				if (hot) {
					task.Start();
				}
				return task;
			}
		}

		/// <summary>
		/// 	Быстро синхронизирует вызывающий поток с текущими задачами подготовки
		/// </summary>
		protected internal void SyncPreEval() {
			Task.WaitAll(_preEvalTaskAgenda.Values.ToArray());
		}

		/// <summary>
		/// 	Возвращает объект препроцессора
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
		/// 	Возвращает препроцессор в пул
		/// </summary>
		/// <param name="sqlbuilder"> </param>
		protected internal void Return(IZexSqlBuilder sqlbuilder) {
			_sqlbuilders.Push(sqlbuilder);
		}

		/// <summary>
		/// 	Регистриует задачу с уникальным SQL запросом в очередь на выполнение запросов
		/// </summary>
		/// <param name="query"> </param>
		/// <returns> </returns>
		/// <exception cref="NotImplementedException"></exception>
		protected internal Task<QueryResult> RegisterSqlRequest(ZexQuery query) {
			lock (syncsqlawait) {
				_sqlDataAwaiters.Add(query);

				if (null == _currentSqlBatchTask) {
					_currentSqlBatchTask = CreateNewSqlBatchTask();
				}

				if (_sqlDataAwaiters.Count >= BatchSize) {
					return RunSqlBatch();
				}
				return _currentSqlBatchTask;
			}
		}

		private Task<QueryResult> CreateNewSqlBatchTask() {
			return new Task<QueryResult>(() =>
				{
					Dictionary<long, ZexQuery> _myrequests;
					lock (syncsqlawait) {
						if (_sqlDataAwaiters.IsEmpty) {
							return null;
						}
						_myrequests = _sqlDataAwaiters.ToArray().Distinct().ToDictionary(_ => _.UID, _ => _);
						_sqlDataAwaiters = new ConcurrentBag<ZexQuery>();
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
					foreach (var myrequest in _myrequests.Values.Where(_ => null == _.Result)) {
						myrequest.Result = new QueryResult {IsComplete = true, IsNull = true};
					}

					return null;
				});
		}

		/// <summary>
		/// 	Стартует текущую задачу по SQL
		/// </summary>
		public Task<QueryResult> RunSqlBatch() {
			lock (this) {
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
		/// 	Выполняет синхронизацию и расчет значений в сессии
		/// </summary>
		public void Execute() {
			lock (syncexecute) {
				WaitPreparation();
				WaitEvaluation();
			}
		}

		/// <summary>
		/// 	Если включено, службы накапливают статистические данные по работе сессии
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
		private readonly ConcurrentStack<ISerialSession> _subsessionpool = new ConcurrentStack<ISerialSession>();
		private readonly object syncexecute = new object();
		private readonly object syncsqlawait = new object();

		/// <summary>
		/// 	Размер батча
		/// </summary>
		public int BatchSize = 1000;

		/// <summary>
		/// 	Позволяет переопределить тип хелпера регистрации
		/// </summary>
		public Type CustomPeriodEvaluatorClass;

		/// <summary>
		/// 	Позволяет переопределить тип хелпера регистрации
		/// </summary>
		public Type CustomPreloadProcessorClass;

		/// <summary>
		/// 	Нестандартный тип препаратора
		/// </summary>
		public Type CustomPreparatorClass;

		/// <summary>
		/// 	Позволяет переопределить тип хелпера регистрации
		/// </summary>
		public Type CustomRegistryHelperClass;

		/// <summary>
		/// 	Пользовательский тип SQL - генератора
		/// </summary>
		public Type CustomSqlBuilderClass;

		/// <summary>
		/// 	Статистика батчей
		/// </summary>
		public int Stat_Batch_Count;

		/// <summary>
		/// 	Статистика времени батчей
		/// </summary>
		public TimeSpan Stat_Batch_Time;

		/// <summary>
		/// 	Статистика использованных значений
		/// </summary>
		public int Stat_Primary_Affected;

		/// <summary>
		/// 	Статистика возвращеных ячеек
		/// </summary>
		public int Stat_Primary_Catched;

		/// <summary>
		/// 	Счетчик формул
		/// </summary>
		public int Stat_QueryType_Formula;

		/// <summary>
		/// 	Счетчик первичных запросов
		/// </summary>
		public int Stat_QueryType_Primary;

		/// <summary>
		/// 	Счетчик сумм
		/// </summary>
		public int Stat_QueryType_Sum;

		/// <summary>
		/// 	Счетчик игнорируемых запросов
		/// </summary>
		public int Stat_Registry_Ignored;

		/// <summary>
		/// 	Статистика действительно уникальных регистраций
		/// </summary>
		public int Stat_Registry_New;

		/// <summary>
		/// 	Статистика вызовов препроцессора
		/// </summary>
		public int Stat_Registry_Preprocessed;

		/// <summary>
		/// 	Статистика резольвинга по внутреннему ключу
		/// </summary>
		public int Stat_Registry_Resolved_By_Key;

		/// <summary>
		/// 	Статистика количества дублированных запросов без препроцессинга
		/// </summary>
		public int Stat_Registry_Resolved_By_Map_Key;

		/// <summary>
		/// 	Статистика резольвинга по наличию в кэше
		/// </summary>
		public int Stat_Registry_Resolved_By_Uid;

		/// <summary>
		/// 	Статистика количества вызовов регистрации
		/// </summary>
		public int Stat_Registry_Started;

		/// <summary>
		/// 	Статистика пользовтельских регистраций
		/// </summary>
		public int Stat_Registry_Started_User;

		/// <summary>
		/// 	Счетчик результативных клиентских запросов
		/// </summary>
		public int Stat_Registry_User;

		/// <summary>
		/// 	Счетчик переводов строки
		/// </summary>
		public int Stat_Row_Redirections;

		/// <summary>
		/// 	Статистика созданных под-сессий
		/// </summary>
		public int Stat_SubSession_Count;

		/// <summary>
		/// 	Статистика общего времени выполнения
		/// </summary>
		public TimeSpan Stat_Time_Total;

		/// <summary>
		/// 	Задача текущего асинхронного последовательного доступа
		/// </summary>
		protected internal Task<QueryResult> _async_serial_acess_task;

		private Task<QueryResult> _currentSqlBatchTask;

		/// <summary>
		/// 	Счетчик очереди выполнения
		/// </summary>
		private int _evalTaskCounter;

		private int _preEvalTaskCounter;

		private ConcurrentBag<ZexQuery> _sqlDataAwaiters = new ConcurrentBag<ZexQuery>();

		/// <summary>
		/// 	Объект блокировки для последовательного доступа
		/// </summary>
		protected internal object _sync_serial_access_lock = new object();
	
	}
}
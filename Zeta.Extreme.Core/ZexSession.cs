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
		/// <summary>
		/// 	Конструктор по умолчанию
		/// </summary>
		/// <remarks>
		/// 	Инициирует основные коллекции
		/// </remarks>
		public ZexSession(bool collectStatistics = false) {
			CollectStatistics = collectStatistics;
			MainQueryRegistry = new ConcurrentDictionary<string, ZexQuery>();
			ActiveSet = new ConcurrentDictionary<string, ZexQuery>();
			ProcessedSet = new ConcurrentDictionary<string, ZexQuery>();
			KeyMap = new ConcurrentDictionary<string, string>();
		}

		/// <summary>
		/// 	Главный реестр запросов
		/// </summary>
		/// <remarks>
		/// 	При регистрации каждому запросу присваивается или передается UID
		/// 	здесь, в MainQueryRegistry мы можем на уровне Value иметь дубляжи запросов
		/// </remarks>
		public ConcurrentDictionary<string, ZexQuery> MainQueryRegistry { get; private set; }


		/// <summary>
		/// 	Оптимизационный мапинг ключей между входным и отпрепроцессорным
		/// 	запросом
		/// </summary>
		public ConcurrentDictionary<string, string> KeyMap { get; private set; }

		/// <summary>
		/// 	Набор всех уникальных, еще не обработанных запросов (агенда)
		/// 	ключ - хэшкей
		/// </summary>
		public ConcurrentDictionary<string, ZexQuery> ActiveSet { get; private set; }

		/// <summary>
		/// 	Набор всех уникальных, уже  обработанных запросов
		/// 	ключ - хэшкей
		/// </summary>
		public ConcurrentDictionary<string, ZexQuery> ProcessedSet { get; private set; }

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
				//должны делать в основном потоке, иначе 
				//WaitRegistry может раньше отработать
				task.Start();
				return task;
			}
		}


		/// <summary>
		/// 	Ожидает окончания всех процессов асинхронной регистрации
		/// </summary>
		public void WaitPreparation() {
			while (!_preEvalTaskAgenda.IsEmpty) {
				SyncPreEval();
				Thread.Sleep(20);
			}
		}

		/// <summary>
		/// 	Ожидает окончания всех процессов асинхронной регистрации
		/// </summary>
		public void WaitEvaluation() {
			RunSqlBatch(); // выполняем остаточные запросы
			while (_evalTaskAgenda.Any(_ => _.Value.Status == TaskStatus.Created)) {
				foreach (var task in _evalTaskAgenda) {
					if (task.Value.Status == TaskStatus.Created) {
						task.Value.Start();
					}
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
		private void ReturnPreparator(IZexQueryPreparator preparator) {
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
		/// 	Возвращает препроцессор в пул
		/// </summary>
		/// <param name="processor"> </param>
		public void ReturnPreloadPreprocessor(IZexPreloadProcessor processor) {
			_preloadprocesspool.Push(processor);
		}


		/// <summary>
		/// 	Возвращает объект препроцессора
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
		/// 	Возвращает препроцессор в пул
		/// </summary>
		/// <param name="periodEvaluator"> </param>
		public void ReturnPeriodEvaluator(IPeriodEvaluator periodEvaluator) {
			_periodevalpool.Push(periodEvaluator);
		}

		/// <summary>
		/// 	Регистриует в агенде задачу на вычисление
		/// </summary>
		/// <param name="resulttask"> </param>
		/// <param name="hot"> немедленный запуск </param>
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
		public void SyncPreEval() {
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
		public void ReturnPreloadPreprocessor(IZexSqlBuilder sqlbuilder) {
			_sqlbuilders.Push(sqlbuilder);
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
		/// 	Счетчик очереди выполнения
		/// </summary>
		private int _evalTaskCounter;

		private int _preEvalTaskCounter;

		/// <summary>
		/// Регистриует задачу с уникальным SQL запросом в очередь на выполнение запросов
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public Task<QueryResult> RegisterForDataRequest(ZexQuery query) {
			lock(this) {
				_sqlDataAwaiters.Add(query);
				if(null==_currentSqlBatchTask) {
					_currentSqlBatchTask = CreateNewSqlBatchTask();
				}
				
				if(_sqlDataAwaiters.Count>=10000) {
					return RunSqlBatch();
				}
				return _currentSqlBatchTask;
			}
		}
		object syncsqlawait = new object();
		private Task<QueryResult> CreateNewSqlBatchTask() {
			return new Task<QueryResult>(() =>
				{
					
					
					Dictionary<long, ZexQuery> _myrequests;
					lock(syncsqlawait) {
						if(_sqlDataAwaiters.IsEmpty) return null;
						_myrequests = _sqlDataAwaiters.ToDictionary(_ => _.UID, _ => _);
						_sqlDataAwaiters = new ConcurrentBag<ZexQuery>();
					}
					Stopwatch sw = null;
					if (CollectStatistics)
					{
						sw = Stopwatch.StartNew();
						Interlocked.Increment(ref Stat_Batch_Count);
					}
					var script = string.Join("\r\nunion\r\n", _myrequests.Values.Select(_ => _.SqlRequest));
					using(var c = myapp.sql.GetConnection("Default")) {
						c.Open();
						var t = c.BeginTransaction(IsolationLevel.ReadUncommitted);
						var cmd = c.CreateCommand();
						cmd.Transaction = t;
						cmd.CommandText = script;
						var reader = cmd.ExecuteReader();
						while (reader.Read()) {
							var key = Convert.ToInt64(reader[0]);
							var cellid = reader.GetInt32(1);
							var val = reader.GetDecimal(2);
							_myrequests[key].Result = new QueryResult {IsComplete = true, CellId = cellid, NumericResult = val};
						}
						reader.Close();
						t.Rollback();
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
		/// Стартует текущую задачу по SQL
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
		/// Статистика батчей
		/// </summary>
		public int Stat_Batch_Count;
		/// <summary>
		/// Статистика времени батчей
		/// </summary>
		public TimeSpan Stat_Batch_Time;
	}
}
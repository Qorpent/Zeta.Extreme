#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : ZexSession.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
		public ZexSession() {
			MainQueryRegistry = new Dictionary<string, ZexQuery>();
			ActiveSet = new Dictionary<string, ZexQuery>();
			ProcessedSet = new Dictionary<string, ZexQuery>();
		}

		/// <summary>
		/// 	Главный реестр запросов
		/// </summary>
		/// <remarks>
		/// 	При регистрации каждому запросу присваивается или передается UID
		/// 	здесь, в MainQueryRegistry мы можем на уровне Value иметь дубляжи запросов
		/// </remarks>
		public IDictionary<string, ZexQuery> MainQueryRegistry { get; private set; }

		/// <summary>
		/// 	Набор всех уникальных, еще не обработанных запросов (агенда)
		/// 	ключ - хэшкей
		/// </summary>
		public IDictionary<string, ZexQuery> ActiveSet { get; private set; }

		/// <summary>
		/// 	Набор всех уникальных, уже  обработанных запросов
		/// 	ключ - хэшкей
		/// </summary>
		public IDictionary<string, ZexQuery> ProcessedSet { get; private set; }

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
				var id = rcount++;
				var task = new Task<ZexQuery>(() =>
					{
						try {
							var helper = GetRegistryHelper();
							var result = helper.Register(query, uid);
							ReturnRegistryHelper(helper);
							return result;
						}
						finally {
							lock(_regq)_regq.Remove(id);
						}
					});
				lock(_regq)_regq.Add(id,task);
				//должны делать в основном потоке, иначе 
				//WaitRegistry может раньше отработать
				task.Start();
				return task;
			}
		}

		/// <summary>
		/// 	Ожидает окончания всех процессов асинхронной регистрации
		/// </summary>
		public void WaitRegistration() {
			while (_regq.Count != 0) {
				Task.WaitAll(_regq.Values.ToArray().Where(x => null != x).ToArray());
			}
		}


		

		/// <summary>
		/// 	Возвращает объект вспомогательного класса регистрации
		/// </summary>
		/// <returns> </returns>
		/// <exception cref="NotImplementedException"></exception>
		public IZexRegistryHelper GetRegistryHelper()
		{
			lock (_registryhelperpool)
			{
				if (_registryhelperpool.Count != 0)
				{
					return _registryhelperpool.Pop();
				}
			}
			lock (this)
			{
				if (null != CustomRegistryHelperClass)
				{
					return Activator.CreateInstance(CustomRegistryHelperClass, this) as IZexRegistryHelper;
				}
				return new DefaultZexRegistryHelper(this);
			}
		}

		/// <summary>
		/// 	Возвращает препроцессор в пул
		/// </summary>
		/// <param name="helper"> </param>
		public void ReturnRegistryHelper(IZexRegistryHelper helper)
		{
			lock (_registryhelperpool)
			{
				if (_registryhelperpool.Count < 99)
				{
					_registryhelperpool.Push(helper);
				}
			}
		}

		/// <summary>
		/// 	Возвращает объект препроцессора
		/// </summary>
		/// <returns> </returns>
		/// <exception cref="NotImplementedException"></exception>
		public IZexPreloadProcessor GetPreloadProcessor() {
			lock (_preloadprocesspool) {
				if (_preloadprocesspool.Count != 0) {
					return _preloadprocesspool.Pop();
				}
			}
			lock (this) {
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
			lock (_preloadprocesspool) {
				if (_preloadprocesspool.Count < 99) {
					_preloadprocesspool.Push(processor);
				}
			}
		}


		/// <summary>
		/// 	Возвращает объект препроцессора
		/// </summary>
		/// <returns> </returns>
		/// <exception cref="NotImplementedException"></exception>
		public IPeriodEvaluator GetPeriodEvaluator()
		{
			lock (_periodevalpool)
			{
				if (_periodevalpool.Count != 0)
				{
					return _periodevalpool.Pop();
				}
			}
			lock (this)
			{
				if (null != CustomPeriodEvaluatorClass)
				{
					return Activator.CreateInstance(CustomPeriodEvaluatorClass, this) as IPeriodEvaluator;
				}
				return new DefaultPeriodEvaluator();
			}
		}

		/// <summary>
		/// 	Возвращает препроцессор в пул
		/// </summary>
		/// <param name="periodEvaluator"> </param>
		public void ReturnPeriodEvaluator(IPeriodEvaluator periodEvaluator)
		{
			lock (_periodevalpool)
			{
				if (_periodevalpool.Count < 99)
				{
					_periodevalpool.Push(periodEvaluator);
				}
			}
		}

		private readonly Stack<IPeriodEvaluator> _periodevalpool = new Stack<IPeriodEvaluator>(100);
		private readonly Stack<IZexRegistryHelper> _registryhelperpool = new Stack<IZexRegistryHelper>(100);
		private readonly Stack<IZexPreloadProcessor> _preloadprocesspool = new Stack<IZexPreloadProcessor>(100);
		private readonly IDictionary<int, Task<ZexQuery>> _regq = new Dictionary<int, Task<ZexQuery>>();

		/// <summary>
		/// 	Позволяет переопределить тип хелпера регистрации
		/// </summary>
		public Type CustomPreloadProcessorClass;

		/// <summary>
		/// 	Позволяет переопределить тип хелпера регистрации
		/// </summary>
		public Type CustomRegistryHelperClass;

		/// <summary>
		/// 	Позволяет переопределить тип хелпера регистрации
		/// </summary>
		public Type CustomPeriodEvaluatorClass;

		private int rcount;

		
	}
}
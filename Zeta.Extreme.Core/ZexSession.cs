using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zeta.Extreme.Core
{
	
	/// <summary>
	/// Базовый класс сессии расчетов Zeta
	/// </summary>
	/// <remarks>
	/// Сессия является ключевым объектом
	/// Новой расчетной системы.
	/// Общий паттерн работы с сессией:
	/// create session ==- register queries ==- evaluate  ==- collect result
	/// Сессия работает с максимальным использованием async - оптимизации
	/// </remarks>
	public class ZexSession {
		/// <summary>
		/// Конструктор по умолчанию
		/// </summary>
		/// <remarks>
		/// Инициирует основные коллекции
		/// </remarks>
		public ZexSession() {
			MainQueryRegistry = new Dictionary<string, ZexQuery>();
			ActiveSet = new Dictionary<string, ZexQuery>();
			ProcessedSet = new Dictionary<string, ZexQuery>();
		}
		/// <summary>
		/// Синхронная регистрация запроса в сессии
		/// </summary>
		/// <param name="query">исходный запрос</param>
		/// <param name="uid">позволяет явно указать словарный код для 
		/// составления синхронизируемой коллекции запросов </param>
		/// <returns>запрос по итогам регистрации в сессии</returns>
		/// <remarks>При регистрации запроса, он проходит дополнительную оптимизацию и проверку на дубляж,
		/// возвращается именно итоговый запрос</remarks>
		/// <exception cref="NotImplementedException"></exception>
		public ZexQuery Register(ZexQuery query, string uid = null) {
			lock(this) {
				var helper = GetRegistryHelper();
				var result = helper.Register(query, uid);
				_currentHelper = helper;
				return result;
			}
		}

		/// <summary>
		/// Асинхронная регистрация запроса в сессии
		/// </summary>
		/// <param name="query">исходный запрос</param>
		/// <param name="uid">позволяет явно указать словарный код для 
		/// составления синхронизируемой коллекции запросов </param>
		/// <returns>задачу, по результатам которой возвращается запрос по итогам регистрации в сессии</returns>
		/// <remarks>При регистрации запроса, он проходит дополнительную оптимизацию и проверку на дубляж,
		/// возвращается именно итоговый запрос</remarks>
		public  Task<ZexQuery> RegisterAsync(ZexQuery query, string uid = null) {
			lock(this) {
				var id = rcount++;
				var task = new Task<ZexQuery>(() =>
					{
						try {
							var helper = GetRegistryHelper();
							var result = helper.Register(query, uid);
							_currentHelper = helper;
							return result;
						}finally {
							lock(_regq)_regq.Remove(id);
						}
					});
				lock(_regq)_regq[id] = task;
				task.Start();
				return  task;
			}
		}

		IDictionary<int,Task<ZexQuery>> _regq = new Dictionary<int, Task<ZexQuery>>();  
		private   int rcount = 0;
		/// <summary>
		/// Ожидает окончания всех процессов асинхронной регистрации
		/// </summary>
		public void WaitRegistration() {
			while (_regq.Count!=0) {
				Task.WaitAll(_regq.Values.ToArray().Where(x => null != x).ToArray());
			}
		}

		

		/// <summary>
		/// Возвращает инстанцию хелпера для регистрации в сессии с частичным кэшем
		/// </summary>
		/// <returns></returns>
		protected IZexRegistryHelper GetRegistryHelper() {
			lock(this) {
				if(null!=_currentHelper) {
					var result = _currentHelper;
					_currentHelper = null;
					return result;
				}
				if(null!=CustomRegistryHelperClass) {
					return Activator.CreateInstance(CustomRegistryHelperClass, this) as IZexRegistryHelper;
				}
				return new DefaultZexRegistryHelper(this);
			}
		}

		/// <summary>
		/// Позволяет переопределить тип хелпера регистрации
		/// </summary>
		public Type CustomRegistryHelperClass;


		/// <summary>
		/// Позволяет переопределить тип хелпера регистрации
		/// </summary>
		public Type CustomPreloadProcessorClass;

		/// <summary>
		/// Главный реестр запросов
		/// </summary>
		/// <remarks>При регистрации каждому запросу присваивается или передается UID
		/// здесь, в MainQueryRegistry мы можем на уровне Value иметь дубляжи запросов</remarks>
		public IDictionary<string, ZexQuery> MainQueryRegistry { get; private set; }
		/// <summary>
		/// Набор всех уникальных, еще не обработанных запросов (агенда)
		/// ключ - хэшкей
		/// </summary>
		public IDictionary<string,ZexQuery> ActiveSet { get; private set; }
		/// <summary>
		/// Набор всех уникальных, уже  обработанных запросов
		/// ключ - хэшкей
		/// </summary>
		public IDictionary<string,ZexQuery> ProcessedSet { get; private set; }

		private IZexRegistryHelper _currentHelper; //one-instance cache

		/// <summary>
		/// Возвращает объект препроцессора
		/// </summary>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public IZexPreloadProcessor GetPreloadProcessor() {
			lock (_preloadprocesspool) {
				if(_preloadprocesspool.Count!=0) {
					return _preloadprocesspool.Pop();
				}
			}
			lock(this) {
				if(null!=CustomPreloadProcessorClass) {
					return Activator.CreateInstance(CustomRegistryHelperClass, this) as IZexPreloadProcessor;
				}
				return new DefaultZexPreloadProcessor(this);
			}
		}
		Stack<IZexPreloadProcessor> _preloadprocesspool = new Stack<IZexPreloadProcessor>(100);
		/// <summary>
		/// Возвращает препроцессор в пул
		/// </summary>
		/// <param name="processor"></param>
		public void ReturnPreloadPreprocessor(IZexPreloadProcessor processor) {
			lock(_preloadprocesspool) {
				if(_preloadprocesspool.Count<99)_preloadprocesspool.Push(processor);
			}	
		}
	}
}

using System;
using System.Threading.Tasks;

namespace Zeta.Extreme {
	/// <summary>
	/// Асинхронная, Zeta.Extrem cecсия, базовый интерфейс
	/// </summary>
	public interface ISession {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="timeout"> </param>
		/// <returns></returns>
		QueryResult Get(string key,int timeout =-1);

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
		Query Register(Query query, string uid = null);

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
		Task<Query> RegisterAsync(Query query, string uid = null);

		/// <summary>
		/// 	Выполняет синхронизацию и расчет значений в сессии
		/// </summary>
		/// <param name="timeout"> </param>
		void Execute(int timeout = -1);


		/// <summary>
		/// 	Локальный кэш объектных данных
		/// </summary>
		IMetaCache MetaCache { get;  }
	}
}
using System.Collections.Concurrent;

namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// Интерфейс индексатора запроса
	/// </summary>
	public interface IWithQueryRegistry {
		/// <summary>
		/// 	Главный реестр запросов
		/// </summary>
		/// <remarks>
		/// 	При регистрации каждому запросу присваивается или передается UID
		/// 	здесь, в MainQueryRegistry мы можем на уровне Value иметь дубляжи запросов
		/// </remarks>
		ConcurrentDictionary<string, IQuery> Registry { get; }

		/// <summary>
		/// 	Оптимизационный мапинг ключей между входным и отпрепроцессорным
		/// 	запросом
		/// </summary>
		ConcurrentDictionary<string, string> KeyMap { get; }

		/// <summary>
		/// 	Набор всех уникальных, еще не обработанных запросов (агенда)
		/// 	ключ - хэшкей
		/// </summary>
		ConcurrentDictionary<string, IQueryWithProcessing> ActiveSet { get; }
	}
}
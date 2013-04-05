using System.Collections.Generic;

namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// Простая имплементация источника параметров сессии
	/// </summary>
	public class DictionarySessionPropertySource:ISessionPropertySource {
		private IDictionary<string, object> _dictionary;

		/// <summary>
		/// Создает источник параметров сессии из словаря
		/// </summary>
		/// <param name="dictionary"></param>
		public DictionarySessionPropertySource(IDictionary<string, object> dictionary) {
			_dictionary = dictionary;
		}

		/// <summary>
		/// Метод получения параметра по имени
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public object Get(string name) {
			if (_dictionary.ContainsKey(name)) return _dictionary[name];
			return null;
		}
	}
}
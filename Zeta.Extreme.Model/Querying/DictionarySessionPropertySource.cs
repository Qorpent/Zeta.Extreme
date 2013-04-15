using System.Collections.Generic;

namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// ������� ������������� ��������� ���������� ������
	/// </summary>
	public class DictionarySessionPropertySource:ISessionPropertySource {
		private IDictionary<string, object> _dictionary;

		/// <summary>
		/// ������� �������� ���������� ������ �� �������
		/// </summary>
		/// <param name="dictionary"></param>
		public DictionarySessionPropertySource(IDictionary<string, object> dictionary) {
			_dictionary = dictionary;
		}

		/// <summary>
		/// ����� ��������� ��������� �� �����
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public object Get(string name) {
			if (_dictionary.ContainsKey(name)) return _dictionary[name];
			return null;
		}
	}
}
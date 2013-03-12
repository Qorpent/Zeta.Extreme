using System.Collections.Concurrent;

namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// ��������� ����������� �������
	/// </summary>
	public interface IWithQueryRegistry {
		/// <summary>
		/// 	������� ������ ��������
		/// </summary>
		/// <remarks>
		/// 	��� ����������� ������� ������� ������������� ��� ���������� UID
		/// 	�����, � MainQueryRegistry �� ����� �� ������ Value ����� ������� ��������
		/// </remarks>
		ConcurrentDictionary<string, IQuery> Registry { get; }

		/// <summary>
		/// 	��������������� ������ ������ ����� ������� � �����������������
		/// 	��������
		/// </summary>
		ConcurrentDictionary<string, string> KeyMap { get; }

		/// <summary>
		/// 	����� ���� ����������, ��� �� ������������ �������� (������)
		/// 	���� - ������
		/// </summary>
		ConcurrentDictionary<string, IQuery> ActiveSet { get; }
	}
}
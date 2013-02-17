using System.Threading.Tasks;

namespace Zeta.Extreme {
	/// <summary>
	/// ������������� ������
	/// </summary>
	public interface ISerializableSession:ISession {
		/// <summary>
		/// ���������� �������������
		/// </summary>
		object SerialSync { get; }

		/// <summary>
		/// ������ ��� ���������� � ����������� ������ �� ���������������� �������
		/// </summary>
		Task<QueryResult> SerialTask { get; set; }
	}
}
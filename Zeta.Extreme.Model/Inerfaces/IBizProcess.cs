using Qorpent.Model;

namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// �������� ������-�������� (���������� ���)
	/// </summary>
	public interface IBizProcess:IEntity {
		/// <summary>
		/// �������� ��������
		/// </summary>
		string InProcess { get; set; }
		/// <summary>
		///��������� ����
		/// </summary>
		string Role { get; set; }
		/// <summary>
		/// ������� ��������� �����
		/// </summary>
		bool IsFinal { get; set; }
		/// <summary>
		/// �������� ������
		/// </summary>
		string RootRows { get; set; }
		/// <summary>
		/// ������� ��������
		/// </summary>
		string Process { get; set; }
	}
}
using Qorpent.Serialization;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// ������ � ������� �������
	/// </summary>
	[Serialize]
	public class ObjectRecord
	{
		/// <summary>
		/// �� ������� (ClassicId)
		/// </summary>
		public int id;
		/// <summary>
		/// �������� �������
		/// </summary>
		public string name;
		/// <summary>
		/// ��������
		/// </summary>
		public string div;
		/// <summary>
		/// ������ ������� � ������ ����
		/// </summary>
		public int idx;
	}
}
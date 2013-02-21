using Qorpent.Serialization;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// ������ � �������
	/// </summary>
	[Serialize]
	public class PeriodRecord {
		/// <summary>
		/// �� ������� (ClassicId)
		/// </summary>
		public int id;
		/// <summary>
		/// �������� �������
		/// </summary>
		public string name;
		/// <summary>
		/// ��� �������
		/// </summary>
		public PeriodType type;
		/// <summary>
		/// ������ ������� � ������ ����
		/// </summary>
		public int idx;
	}
}
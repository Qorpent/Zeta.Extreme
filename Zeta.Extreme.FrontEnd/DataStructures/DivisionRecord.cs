using Qorpent.Serialization;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// ������ � ���������
	/// </summary>
	[Serialize]
	public class DivisionRecord
	{
		/// <summary>
		/// ��� ���������
		/// </summary>
		public string code;
		/// <summary>
		/// �������� ���������
		/// </summary>
		public string name;

		/// <summary>
		/// ������ ���������
		/// </summary>
		public int idx;
	}
}
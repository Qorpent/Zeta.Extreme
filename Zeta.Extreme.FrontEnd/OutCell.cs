using Qorpent.Serialization;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// ��������� ������ ������
	/// </summary>
	[Serialize]
	public class OutCell {
		/// <summary>
		/// ���������� �� ������
		/// </summary>
		public string i;
		/// <summary>
		/// �������� ������
		/// </summary>
		[SerializeNotNullOnly]
		public string v;

		/// <summary>
		/// ������ �� Id ������ � ��
		/// </summary>
		[SerializeNotNullOnly]
		public int c;
	}
}
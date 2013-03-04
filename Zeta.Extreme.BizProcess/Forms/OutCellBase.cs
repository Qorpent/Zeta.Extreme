using Qorpent.Serialization;

namespace Zeta.Extreme.Form {
	/// <summary>
	/// ���������, ��� ������������� ������ � ����� �����
	/// </summary>
	public class OutCellBase {
		/// <summary>
		/// 	������ �� Id ������ � ��
		/// </summary>
		[SerializeNotNullOnly] public int c;

		/// <summary>
		/// 	������� ��������, ������� ����� ���� ����� ����������
		/// </summary>
		[IgnoreSerialize] public bool canbefilled;

		/// <summary>
		/// 	���������� �� ������
		/// </summary>
		public string i;

		/// <summary>
		/// 	��������� ������� ��� ������ � ������ �������
		/// </summary>
		[IgnoreSerialize] public OutCellBase linkedcell;

		/// <summary>
		/// 	�������� ������
		/// </summary>
		[SerializeNotNullOnly] public string v;

		/// <summary>
		/// �������� ���� ������
		/// </summary>
		[SerializeNotNullOnly]public string ri;
	}
}
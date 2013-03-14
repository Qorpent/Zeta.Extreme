using Qorpent.Serialization;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// ��������� �������, ��������������� �� ����
	/// </summary>
	[Serialize]
	public class PeriodTypeGroup {
		/// <summary>
		/// ���
		/// </summary>
		public PeriodType type;
		/// <summary>
		/// ����� ��������
		/// </summary>
		public PeriodRecord[] periods;
	}
}
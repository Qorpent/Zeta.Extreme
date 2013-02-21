using Qorpent.Serialization;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// ��������� ��������� ������� ��������
	/// </summary>
	[Serialize]
	public class AccessibleObjects {
		/// <summary>
		/// ��������� ���������
		/// </summary>
		[Serialize] public DivisionRecord[] divs { get; set; }
		/// <summary>
		/// ��������� �������
		/// </summary>
		[Serialize] public ObjectRecord[] objs { get; set; }
	}
}
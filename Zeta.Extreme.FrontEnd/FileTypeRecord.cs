using Qorpent.Serialization;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// ��������� ������������� �����
	/// </summary>
	[Serialize]
	public class FileTypeRecord {
		/// <summary>
		/// ���
		/// </summary>
		public string code { get; set; }
		/// <summary>
		/// ���
		/// </summary>
		public string name { get; set; }

	}
}
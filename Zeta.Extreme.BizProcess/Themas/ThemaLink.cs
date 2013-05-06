namespace Zeta.Extreme.BizProcess.Themas {
	/// <summary>
	/// ����� ����� ������
	/// </summary>
	public class ThemaLink {
		/// <summary>
		///	��������
		/// </summary>
		public IThema Source { get; set; }
		/// <summary>
		/// ����
		/// </summary>
		public IThema Target { get; set; }
		/// <summary>
		/// ���
		/// </summary>
		public string Type { get; set; }
		/// <summary>
		/// ��������
		/// </summary>
		public string Value { get; set; }
	}
}
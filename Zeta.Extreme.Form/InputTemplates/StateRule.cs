namespace Comdiv.Zeta.Web.InputTemplates {
	/// <summary>
	/// ������� �� ������
	/// </summary>
	public class StateRule   {
		/// <summary>
		/// ������� �����
		/// </summary>
		public string Current { get; set; }
		/// <summary>
		/// ������� ����� (?)
		/// </summary>
		public string Target { get; set; }
		/// <summary>
		/// ���
		/// </summary>
		public string Type { get; set; }
		/// <summary>
		/// ��������
		/// </summary>
		public string Action { get; set; }
		/// <summary>
		/// ������� ������
		/// </summary>
		public string CurrentState { get; set; }
		/// <summary>
		/// �������������� ������
		/// </summary>
		public string ResultState { get; set; }
	}
}
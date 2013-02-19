namespace Zeta.Extreme.FrontEnd.Session {
	/// <summary>
	/// ���������� � ������� �������� �����
	/// </summary>
	public class LockStateInfo {
		/// <summary>
		/// ����������������� ������� �������� �����
		/// </summary>
		public bool isopen;
		/// <summary>
		/// ������� ������
		/// </summary>
		public string state;
		/// <summary>
		/// ������� ����������� ����������
		/// </summary>
		public bool cansave;
		/// <summary>
		/// ��������� �� ������ ����������
		/// </summary>
		public string message;
		/// <summary>
		/// ����������� ���������� �����
		/// </summary>
		public bool canblock;
	}
}
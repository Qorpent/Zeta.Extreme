namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// ��������������� ��������� ������ � ��������� �� ������
	/// </summary>
	public interface IWithSession {
		

		/// <summary>
		/// 	�������� ������ �� ������
		/// </summary>
		ISession Session { get; set; }
	}

	
}
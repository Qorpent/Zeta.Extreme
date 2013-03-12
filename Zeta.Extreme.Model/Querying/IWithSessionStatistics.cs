namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// ��������������� ��������� ��� ������ ��� ������� � ����������
	/// </summary>
	public interface IWithSessionStatistics {
		/// <summary>
		/// ������ � ���������� ������
		/// </summary>
		SessionStatistics Statistics { get; set; }

		/// <summary>
		/// 	���� ��������, ������ ����������� �������������� ������ �� ������ ������
		/// </summary>
		bool CollectStatistics { get; }
	}
}
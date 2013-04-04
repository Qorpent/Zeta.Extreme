namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// ��������� ��������� �������������� ���������� ������
	/// </summary>
	public interface ISessionPropertySource {
		/// <summary>
		/// ����� ��������� ��������� �� �����
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		object Get(string name);
	}
}
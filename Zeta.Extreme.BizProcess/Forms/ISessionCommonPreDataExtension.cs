namespace Zeta.Extreme.BizProcess.Forms {
	/// <summary>
	/// ����� ��������� ���������� ��� ������������ ��������� ������ ����� �����, �� ������������ � ������������ ����� (����� ������)
	/// </summary>
	public interface ISessionCommonPreDataExtension
	{
		/// <summary>
		/// ���������� ������� ����� ����� ��� �����
		/// </summary>
		/// <param name="session"></param>
		/// <returns></returns>
		void Prepare(IFormSession session);
	}
}
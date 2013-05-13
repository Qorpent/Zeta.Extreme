namespace Zeta.Extreme.BizProcess.Forms {
	/// <summary>
	/// ����� ��������� ���������� ��� ������������ ��������� ������ ����� �����
	/// </summary>
	public interface ISessionPreDataExtension
	{
		/// <summary>
		/// ���������� ������� ����� ����� ��� �����
		/// </summary>
		/// <param name="session"></param>
		/// <returns></returns>
		void Prepare(IFormSession session);
	}
}
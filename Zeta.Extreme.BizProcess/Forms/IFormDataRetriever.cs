namespace Zeta.Extreme.BizProcess.Forms {
	/// <summary>
	/// ����� ��������� ���������� ��� ������������ ��������� ������ ����� �����
	/// </summary>
	public interface IFormDataRetriever
	{
		/// <summary>
		/// ���������� ������� ����� ����� ��� �����
		/// </summary>
		/// <param name="session"></param>
		/// <returns></returns>
		void RetrieveData(IFormSession session);
	}
}
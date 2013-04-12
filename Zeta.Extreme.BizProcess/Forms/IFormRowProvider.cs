using Zeta.Extreme.FrontEnd;

namespace Zeta.Extreme.BizProcess.Forms {
	/// <summary>
	/// ����� ��������� ���������� ��� ������������ ��������� ������ ����� �����
	/// </summary>
	public interface IFormRowProvider {
		/// <summary>
		/// ���������� ������� ����� ����� ��� �����
		/// </summary>
		/// <param name="session"></param>
		/// <returns></returns>
		FormStructureRow[] GetRows(IFormSession session);
	}
}
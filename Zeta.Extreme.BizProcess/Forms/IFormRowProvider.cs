using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.FrontEnd {
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
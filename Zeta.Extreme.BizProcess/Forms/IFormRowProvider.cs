using Zeta.Extreme.FrontEnd;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.BizProcess.Forms {
	/// <summary>
	/// ����� ��������� ���������� ��� ������������ ��������� ������ ����� �����
	/// </summary>
	public interface IFormRowProvider {
		/// <summary>
		/// ���������� ������� ����� ����� ��� �����
		/// </summary>
		/// <param name="session"></param>
		/// <param name="contextRow"></param>
		/// <param name="baseLevel"></param>
		/// <returns></returns>
		FormStructureRow[] GetRows(IFormSession session, IZetaRow contextRow = null, int baseLevel=0);
	}
}
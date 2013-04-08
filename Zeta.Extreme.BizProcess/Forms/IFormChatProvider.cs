using System.Collections.Generic;

namespace Zeta.Extreme.BizProcess.Forms {
	/// <summary>
	/// ��������� ���� �����
	/// </summary>
	public interface IFormChatProvider {
		/// <summary>
		/// ����� ��������� � ������� ��������� ����
		/// </summary>
		/// <param name="session"></param>
		/// <returns></returns>
		IEnumerable<FormChatItem> GetSessionItems(IFormSession session);
		/// <summary>
		/// ���������� ������ ���������
		/// </summary>
		/// <param name="session"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		FormChatItem AddMessage(IFormSession session, string message);
	}
}
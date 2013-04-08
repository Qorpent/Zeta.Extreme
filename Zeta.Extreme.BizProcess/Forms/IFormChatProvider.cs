using System.Collections.Generic;

namespace Zeta.Extreme.BizProcess.Forms {
	/// <summary>
	/// Интерфейс чата формы
	/// </summary>
	public interface IFormChatProvider {
		/// <summary>
		/// Поиск связанных с сессией сообщений чата
		/// </summary>
		/// <param name="session"></param>
		/// <returns></returns>
		IEnumerable<FormChatItem> GetSessionItems(IFormSession session);
		/// <summary>
		/// Добавление нового сообщения
		/// </summary>
		/// <param name="session"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		FormChatItem AddMessage(IFormSession session, string message);
	}
}
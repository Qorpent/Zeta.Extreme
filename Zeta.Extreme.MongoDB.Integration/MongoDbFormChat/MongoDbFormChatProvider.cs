using System;
using System.Collections.Generic;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.MongoDB.Integration {
	/// <summary>
	/// Mongo - реализация чата формы
	/// </summary>
	public class MongoDbFormChatProvider : IFormChatProvider {
		/// <summary>
		/// Поиск связанных с сессией сообщений чата
		/// </summary>
		/// <param name="session"></param>
		/// <returns></returns>
		public IEnumerable<FormChatItem> GetSessionItems(IFormSession session) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Добавление нового сообщения
		/// </summary>
		/// <param name="session"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		public FormChatItem AddMessage(IFormSession session, string message) {
			throw new NotImplementedException();
		}
	}
}
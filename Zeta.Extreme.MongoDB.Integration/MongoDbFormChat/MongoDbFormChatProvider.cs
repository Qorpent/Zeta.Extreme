using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Qorpent.IoC;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.MongoDB.Integration {
	/// <summary>
	/// Mongo - реализация чата формы
	/// </summary>
	[ContainerComponent(Lifestyle.Transient,ServiceType = typeof(IFormChatProvider))]
	public class MongoDbFormChatProvider : MongoAttachedProviderBase, IFormChatProvider {
		/// <summary>
		/// Поиск связанных с сессией сообщений чата
		/// </summary>
		/// <param name="session"></param>
		/// <returns></returns>
		public IEnumerable<FormChatItem> GetSessionItems(IFormSession session) {
			SetupConnection();
			var search = MongoDbFormChatSerializer.SessionToSearchDocument(session);
			return Collection.Find(new QueryDocument(search)).Select(_ => MongoDbFormChatSerializer.BsonToChatItem(_));
		}

		/// <summary>
		/// Добавление нового сообщения
		/// </summary>
		/// <param name="session"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		public FormChatItem AddMessage(IFormSession session, string message) {
			SetupConnection();
			var item = new FormChatItem {Text = message};
			var bson = MongoDbFormChatSerializer.ChatItemToBson(session,item);
			Collection.Save(bson);
			return item;
		}


	}
}
using System;
using MongoDB.Bson;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.MongoDB.Integration {
	/// <summary>
	/// The representation of FormChatItem
	/// </summary>
	public static class MongoDbFormChatSerializer {
		/// <summary>
		/// Converts chat item to BSON
		/// </summary>
		/// <param name="session"></param>
		/// <param name="chatItem"></param>
		/// <returns></returns>
		public static BsonDocument ChatItemToBson(IFormSession session,FormChatItem chatItem) {
			Normalize(session,chatItem);
			var document = new BsonDocument();
			document.Set("_id", chatItem.Id);
			document.Set("form", chatItem.FormCode);
			document.Set("year", chatItem.Year);
			document.Set("period", chatItem.Period);
			document.Set("obj", chatItem.ObjId);
			document.Set("user", chatItem.User);
			document.Set("text", chatItem.Text);
			document.Set("time", chatItem.Time);
			document.Set("type", chatItem.Type);
			return document;
		}

		/// <summary>
		/// Converts BSON to formchatitem
		/// </summary>
		/// <param name="document"></param>
		/// <returns></returns>
		public static FormChatItem BsonToChatItem(BsonDocument document) {
			var result = new FormChatItem();
			result.Id = document["_id"].AsString;
			result.FormCode = document["form"].AsString;
			result.Year = document["year"].AsInt32;
			result.Period = document["period"].AsInt32;
			result.ObjId = document["obj"].AsInt32;
			result.User = document["user"].AsString;
			result.Text = document["text"].AsString;
			result.Time = document["time"].ToLocalTime();
			return result;
		}
		/// <summary>
		/// Формирует объект для поиска чата по сессии
		/// </summary>
		/// <param name="session"></param>
		/// <returns></returns>
		public static BsonDocument SessionToSearchDocument(IFormSession session) {
			var result = new BsonDocument();
			result["year"] = session.Year;
			result["period"] = session.Period;
			result["obj"] = session.Object.Id;
			result["form"] = session.Template.Code;
			return result;
		}

		/// <summary>
		/// Нормализовать исходный FormChatItem
		/// </summary>
		/// <param name="session"></param>
		/// <param name="chatItem"></param>
		public static void Normalize(IFormSession session,FormChatItem chatItem) {
			if (string.IsNullOrWhiteSpace(chatItem.Id)) {
				chatItem.Id = ObjectId.GenerateNewId().ToString();
			}
			if (null != session) {
				chatItem.FormCode = session.Template.Code;
				chatItem.Year = session.Year;
				chatItem.Period = session.Period;
				chatItem.ObjId = session.Object.Id;
				if (string.IsNullOrWhiteSpace(chatItem.User)) {
					chatItem.User = session.Usr;
				}
			}
			if (chatItem.Time.Year <= 1990) {
				chatItem.Time = DateTime.Now;
			}
			if (string.IsNullOrWhiteSpace(chatItem.Type)) {
				chatItem.Type = "default";
			}
		}

        /// <summary>
        ///     Sets Archived mark
        /// </summary>
        /// <param name="document"></param>
        public static void SetArchived(BsonDocument document) {
            document.Set("archive", true);
        }
	}
}
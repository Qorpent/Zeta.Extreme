using System;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Wrappers;
using Qorpent.Applications;
using Qorpent.Wiki;

namespace Zeta.Extreme.MongoDB.Integration.WikiStorage {

	/// <summary>
	/// Класс сериализации Wiki страниц в Mongo
	/// </summary>
	public class MongoWikiSerializer {
		/// <summary>
		/// Конвертирует BSON в страницу
		/// </summary>
		/// <param name="doc"></param>
		/// <returns></returns>
		public WikiPage ToPage(BsonDocument doc) {
			var result = new WikiPage
				{
					Code = doc["_id"].AsString,
					Text = doc["text"].AsString,
					LastWriteTime = doc["ver"].ToLocalTime(),
					Owner = doc["owner"].AsString,
					Editor = doc["editor"].AsString,
					Title = doc["title"].AsString,
					Existed = true,
				};
			foreach (var e in doc) {
				if (e.Name == "_id") continue;
				if (e.Name == "text") continue;
				if (e.Name == "ver") continue;
				if (e.Name == "owner") continue;
				if (e.Name == "editor") continue;
				if(e.Name=="title")continue;
				result.Propeties[e.Name] = e.Value.AsString;
			}
			return result;
		}
		/// <summary>
		/// Формирует запрос по кодам
		/// </summary>
		/// <param name="codes"></param>
		/// <returns></returns>
		public  IMongoQuery GetQueryFromCodes(string[] codes)
		{
			var query = new BsonDocument();
			var idcond = query["_id"] = new BsonDocument();
			idcond["$in"] = new BsonArray(codes);
			var qdoc = new QueryDocument(query);
			return qdoc;
		}
		/// <summary>
		/// Конвертирует BSON в страницу
		/// </summary>
		/// <param name="page"></param>
		/// <returns></returns>
		public object NewFormPage(WikiPage page) {
			var result = new BsonDocument();
			result["_id"] = page.Code;
			result["text"] = page.Text;
			result["ver"] = DateTime.Now;
			result["owner"] = Application.Current.Principal.CurrentUser.Identity.Name;
			result["editor"] = Application.Current.Principal.CurrentUser.Identity.Name;
			result["title"] = page.Title;
			foreach (var propety in page.Propeties) {
				result[propety.Key] = propety.Value ?? "";
			}
			return result;
		}

		/// <summary>
		/// Конвертирует страницу в команду обновления
		/// </summary>
		/// <param name="page"></param>
		/// <returns></returns>
		public IMongoUpdate UpdateFromPage(WikiPage page) {
			var updateBuilder = new UpdateBuilder();
			if (!string.IsNullOrEmpty(page.Text)) {
				updateBuilder.Set("text", page.Text);
			}
			if (!string.IsNullOrEmpty(page.Title))
			{
				updateBuilder.Set("title", page.Title);
			}
			updateBuilder.Set("ver", DateTime.Now);
			updateBuilder.Set("editor", Application.Current.Principal.CurrentUser.Identity.Name);
			foreach (var propety in page.Propeties)
			{
				updateBuilder.Set(propety.Key , propety.Value ?? "");
			}
			return updateBuilder;
		}
	}
}
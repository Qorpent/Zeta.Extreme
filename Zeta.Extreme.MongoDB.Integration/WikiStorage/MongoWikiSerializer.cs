using System;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
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
					Code = doc["code"].AsString,
					Text = doc["text"].AsString,
					Owner = doc["owner"].AsString,
					Editor = doc["editor"].AsString,
					Title = doc["title"].AsString,
                    Published = doc["published"].ToUniversalTime(),
                    Version = doc["version"].AsString,
					Existed = true,
				};
			foreach (var e in doc) {
			    if (e.Name == "_id") continue;
				if (e.Name == "code") continue;
				if (e.Name == "text") continue;
				if (e.Name == "owner") continue;
				if (e.Name == "editor") continue;
				if(e.Name=="title")continue;
			    if (e.Name == "published") continue;
                if (e.Name == "version") continue;
				result.Propeties[e.Name] = e.Value.AsString;
			}
			return result;
		}

		/// <summary>
		/// Конвертирует описатель MongoFile в WikiBinary
		/// </summary>
		/// <param name="info"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public  WikiBinary ConvertToWikiBinary(MongoGridFSFileInfo info, byte[] data)
		{
			var result = new WikiBinary
			{
				Code = info.Id.AsString,
				Data = data,
				MimeType = info.ContentType,
				Size = info.Length,
				Title = info.Metadata["title"].AsString,
				Owner = info.Metadata["owner"].AsString,
				Editor = info.Metadata["editor"].AsString,
				LastWriteTime = info.Metadata["lastwrite"].ToLocalTime()
			};
			return result;
		}




		/// <summary>
		/// Формирует запрос по кодам и версиям
		/// </summary>
		/// <param name="codes"></param>
        /// <param name="versions"></param>
		/// <returns></returns>
		public  IMongoQuery GetQueryFromCodes(string[] codes, string[] versions)
		{
			var query = new BsonDocument();
			var idcond = query["code"] = new BsonDocument();
		    var versionCond = query["version"] = new BsonDocument();
			idcond["$in"] = new BsonArray(codes);
            versionCond["$in"] = new BsonArray(versions);
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
			result["code"] = page.Code;
			result["text"] = page.Text;
			result["owner"] = Application.Current.Principal.CurrentUser.Identity.Name;
			result["editor"] = Application.Current.Principal.CurrentUser.Identity.Name;
			result["title"] = page.Title ?? "";
		    result["version"] = page.Version ?? Guid.NewGuid().ToString();
		    result["published"] = DateTime.Now;
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

			updateBuilder.Set("editor", Application.Current.Principal.CurrentUser.Identity.Name);
			foreach (var propety in page.Propeties)
			{
				updateBuilder.Set(propety.Key , propety.Value ?? "");
			}
			return updateBuilder;
		}
	}
}
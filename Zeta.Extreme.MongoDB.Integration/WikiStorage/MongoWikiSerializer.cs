using System;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using Qorpent.Applications;
using Qorpent.Wiki;

namespace Zeta.Extreme.MongoDB.Integration.WikiStorage {
	/// <summary>
	///     Класс сериализации Wiki страниц в MongoDB
	/// </summary>
	public class MongoWikiSerializer {
		/// <summary>
		///     Конвертирует BSON в страницу
		/// </summary>
		/// <param name="doc">Документ</param>
		/// <returns>Страница wiki</returns>
		public WikiPage ToPage(BsonDocument doc) {
			var result = new WikiPage {
                Code = doc["code"].AsString,
                Text = doc["text"].AsString,
                Owner = doc["owner"].AsString,
                Editor = doc["editor"].AsString,
                Title = doc["title"].AsString,
                Published = doc["published"].ToUniversalTime(),
                Version = doc["version"].AsString,
                Existed = true
            };

			foreach (var e in doc) {
			    if (IsProperty(e.Name)) {
			        result.Propeties[e.Name] = e.Value.AsString;
			    }
			}

			return result;
		}

		/// <summary>
		///     Конвертирует описатель MongoFile в WikiBinary
		/// </summary>
		/// <param name="info"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public  WikiBinary ConvertToWikiBinary(MongoGridFSFileInfo info, byte[] data) {
			return new WikiBinary {
				Code = info.Id.AsString,
				Data = data,
				MimeType = info.ContentType,
				Size = info.Length,
				Title = info.Metadata["title"].AsString,
				Owner = info.Metadata["owner"].AsString,
				Editor = info.Metadata["editor"].AsString,
				LastWriteTime = info.Metadata["lastwrite"].ToLocalTime()
			};
		}

		/// <summary>
		/// Формирует запрос по кодам и версиям
		/// </summary>
		/// <param name="codes"></param>
        /// <param name="versions"></param>
		/// <returns></returns>
		public  IMongoQuery GetQueryFromCodes(string[] codes, string[] versions) {
			var query = new BsonDocument();
			var idcond = query["code"] = new BsonDocument();
		    var versionCond = query["version"] = new BsonDocument();
			idcond["$in"] = new BsonArray(codes);
            versionCond["$in"] = new BsonArray(versions);
			var qdoc = new QueryDocument(query);
			return qdoc;
		}

		/// <summary>
		///     Конвертирует BSON в страницу
		/// </summary>
		/// <param name="page">Страница Wiki</param>
		/// <returns>Конвертированный BSON документ</returns>
		public object NewFormPage(WikiPage page) {
		    var document = new BsonDocument {
                {"code", page.Code},
                {"text", page.Text},
                {"owner", Application.Current.Principal.CurrentUser.Identity.Name},
                {"editor", Application.Current.Principal.CurrentUser.Identity.Name},
                {"title", page.Title ?? ""}
		    };

            UpdateWikiPageVersion(page);
            CopyWikiPageVersionToBson(page, document);

			foreach (var propety in page.Propeties) {
                document[propety.Key] = propety.Value ?? "";
			}

            return document;
		}

		/// <summary>
		///     Конвертирует страницу в команду обновления
		/// </summary>
		/// <param name="page"></param>
		/// <returns></returns>
		public IMongoUpdate UpdateFromPage(WikiPage page) {
			var updateBuilder = new UpdateBuilder();

			if (!string.IsNullOrEmpty(page.Text)) {
				updateBuilder.Set("text", page.Text);
			}

			if (!string.IsNullOrEmpty(page.Title)) {
				updateBuilder.Set("title", page.Title);
			}

		    updateBuilder.Set("version", page.Version);
		    updateBuilder.Set("published", page.Published);
			updateBuilder.Set("editor", Application.Current.Principal.CurrentUser.Identity.Name);

			foreach (var propety in page.Propeties) {
				updateBuilder.Set(propety.Key, propety.Value ?? "");
			}

			return updateBuilder;
		}

        /// <summary>
        ///     Обновление версии и даты отправки страницы в БД
        /// </summary>
        /// <param name="page">Страница</param>
        public void UpdateWikiPageVersion(WikiPage page) {
            page.Version = Guid.NewGuid().ToString();
            page.Published = DateTime.Now;
        }

        /// <summary>
        ///     Копирует данные о версии и дате занесения в базу страницы Wiki
        /// </summary>
        /// <param name="page">Страница Wiki</param>
        /// <param name="document">Целевой документ</param>
        public void CopyWikiPageVersionToBson(WikiPage page, BsonDocument document) {
            document["version"] = page.Version;
            document["published"] = page.Published;
        }

        /// <summary>
        ///     Проверяет, не является ли поле в документе мета-свойством
        /// </summary>
        /// <param name="fieldName">Имя поля</param>
        /// <returns>True — является, false — не явялется</returns>
        private bool IsProperty(string fieldName) {
            if (fieldName == "_id") {
                return false;
            }

            if (fieldName == "code") {
                return false;
            }

            if (fieldName == "text") {
                return false;
            }

            if (fieldName == "owner") {
                return false;
            }

            if (fieldName == "editor") {
                return false;
            }

            if (fieldName == "title") {
                return false;
            }

            if (fieldName == "published") {
                return false;
            }

            if (fieldName == "version") {
                return false;
            }

            return true;
        }
	}
}
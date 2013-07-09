using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Qorpent.Wiki;

namespace Zeta.Extreme.MongoDB.Integration.WikiStorage {
	/// <summary>
	///     Реализация персистера Wiki на Mongo
	/// </summary>
	public class MongoWikiPersister : MongoBasedServiceBase, IWikiPersister {
        /// <summary>
        ///     Компонент сериализации WikiPage - BSON
        /// </summary>
        protected MongoWikiSerializer Serializer { get; set; }

		/// <summary>
		///     Создание персистера
		/// </summary>
		public MongoWikiPersister() {
			Serializer = new MongoWikiSerializer();
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codes"></param>
        /// <returns></returns>
	    public IEnumerable<WikiPage> Get(params string[] codes) {
	        throw new NotImplementedException();
	    }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codes"></param>
        /// <returns></returns>
	    public IEnumerable<WikiPage> Exists(params string[] codes) {
	        throw new NotImplementedException();
	    }

	    /// <summary>
		/// Возвращает полностью загруженные страницы Wiki
		/// </summary>
        /// <param name="codeVersion"></param>
		/// <returns></returns>
		public IEnumerable<WikiPage> Get(IEnumerable<KeyValuePair<string, string>> codeVersion) {
		    var clause = PrepareCodeVersionFindClause(codeVersion);
            var found = Collection.Find(clause);

            foreach (var doc in found) {
				yield return Serializer.ToPage(doc);
			}
		}

        /// <summary>
        ///     Возвращает страницы, только с загруженным признаком хранения в БД
        /// </summary>
        /// <param name="codeVersion"></param>
        /// <returns></returns>
        public IEnumerable<WikiPage> Exists(IEnumerable<KeyValuePair<string, string>> codeVersion) {
            var clause = PrepareCodeVersionFindClause(codeVersion);
            foreach (var doc in Collection.Find(clause).SetFields("code")) {
				yield return new WikiPage {Code = doc["code"].AsString};
			}
		}

        private IMongoQuery PrepareCodeVersionFindClause(IEnumerable<KeyValuePair<string, string>> codeVersion) {
            var rawQueryCodes = "";
            var rawQueryVersions = "";

            foreach (var keyValuePair in codeVersion) {
                rawQueryCodes += "'" + keyValuePair.Key + "', ";
                rawQueryVersions += "'" + keyValuePair.Value + "', ";
            }

            rawQueryCodes = "[" + rawQueryCodes.Substring(0, rawQueryCodes.Length - 2) + "]";
            rawQueryVersions = "[" + rawQueryVersions.Substring(0, rawQueryVersions.Length - 2) + "]";

            var rawQuery = "{code : {$in : " + rawQueryCodes + "}, version : {$in : " + rawQueryVersions + "}}";

            return new QueryDocument(
                BsonDocument.Parse(rawQuery)
            );
        }

		/// <summary>
		/// Метод сохранения изменений в страницу
		/// </summary>
		/// <param name="pages"></param>
		public void Save(params WikiPage[] pages) {
			foreach (var page in pages) {
				
				var existsQuery = Serializer.GetQueryFromCodes(new[] {page.Code}, new[] {page.Version});
				var exists = Collection.Find(existsQuery).Count() != 0;
				if (exists) {
					var update = Serializer.UpdateFromPage(page);
					Collection.Update(existsQuery, update, UpdateFlags.Upsert);
				}
				else {
					var doc = Serializer.NewFormPage(page);
					Collection.Insert(doc);
				}
			}
		}

		/// <summary>
		/// Сохраняет в Wiki файл с указанным кодом
		/// </summary>
		public void SaveBinary(WikiBinary binary) {
			using (var s = CreateStream(binary)) {
				s.Write(binary.Data,0,binary.Data.Length);
				s.Flush();
			}
			
		}

		private MongoGridFSStream CreateStream(WikiBinary binary) {
			if (!GridFs.ExistsById(binary.Code)) {
				return ConvertToMongoGridInfo(binary);
			}
			var info = GridFs.FindOne(binary.Code);
			info.Metadata["editor"] = Application.Principal.CurrentUser.Identity.Name;
			info.Metadata["lastwrite"] = DateTime.Now;
			GridFs.SetMetadata(info,info.Metadata);
			return info.OpenWrite();

		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binary"></param>
        /// <returns></returns>
		private MongoGridFSStream ConvertToMongoGridInfo(WikiBinary binary) {
			return GridFs.Create(
				binary.Code,
				new MongoGridFSCreateOptions {
					Id = binary.Code,
                    Metadata = new BsonDocument {
                        {"title", binary.Title},
                        {"editor", Application.Principal.CurrentUser.Identity.Name},
                        {"owner", Application.Principal.CurrentUser.Identity.Name},
                        {"lastwrite", DateTime.Now},
		            },
					UploadDate = DateTime.Now,
					ContentType = binary.MimeType,
				}
            );
		}

		/// <summary>
		/// Загружает бинарный контент
		/// </summary>
		/// <param name="code"></param>
		/// <param name="withData">Флаг, что требуется подгрузка бинарных данных</param>
		/// <returns></returns>
		public WikiBinary LoadBinary(string code, bool withData = true) {

			if (!GridFs.ExistsById(code)) return null;
			var info = GridFs.FindOne(code);
			if (!info.Metadata.Contains("lastwrite")) {
				info.Metadata["lastwrite"] = info.UploadDate;
				GridFs.SetMetadata(info,info.Metadata);
			}
			byte[] data = null;
			if (withData) {
				data = new byte[info.Length];
				using (var s = info.OpenRead()) {
					s.Read(data, 0, (int)s.Length);
				}
			}
			return Serializer.ConvertToWikiBinary(info, data);
		}

		

		/// <summary>
		/// Поиск страниц по маске 
		/// </summary>
		/// <param name="search"></param>
		/// <returns></returns>
		public IEnumerable<WikiPage> FindPages(string search) {
			var queryJson = "{$or : [ {code : {$regex : '(?ix)_REGEX_'}},{title:{$regex: '(?ix)_REGEX_'}},{owner:{$regex: '(?ix)_REGEX_'}},{editor:{$regex: '(?ix)_REGEX_'}}]},".Replace("_REGEX_", search);
			var queryDoc = new QueryDocument(BsonDocument.Parse(queryJson));
            foreach (var doc in Collection.Find(queryDoc).SetFields("code", "title", "ver", "editor", "owner")) {
				var page = new WikiPage
					{
                        Code = doc["code"].AsString,
						Title = doc["title"].AsString,
						LastWriteTime = doc["ver"].ToLocalTime()
					};
				yield return page;
			}
		}

		/// <summary>
		/// Поиск файлов по маске
		/// </summary>
		/// <param name="search"></param>
		/// <returns></returns>
		public IEnumerable<WikiBinary> FindBinaries(string search) {
            var queryJson = "{$or : [{code : {$regex : '(?ix)_REGEX_'}},{'metadata.title':{$regex:'(?ix)_REGEX_'}},{'metadata.owner':{$regex:'(?ix)_REGEX_'}},{contentType:{$regex:'(?ix)_REGEX_'}}]}".Replace("_REGEX_", search);
			var queryDoc = new QueryDocument(BsonDocument.Parse(queryJson));
            foreach (var doc in GridFs.Find(queryDoc).SetFields("code", "metadata.title", "metadata.owner", "length", "contentType", "metadata.lastwrite", "uploadDate", "chunkSize"))
			{
				var file = new WikiBinary()
				{
					Code = doc.Id.AsString,
					Title = doc.Metadata["title"].AsString,
					LastWriteTime = doc.UploadDate,
					Size = doc.Length,
					MimeType = doc.ContentType
				};
				if (doc.Metadata.Contains("lastwrite")) {
					file.LastWriteTime = doc.Metadata["lastwrite"].ToLocalTime();
				}
				yield return file;
			}
		}

		/// <summary>
		/// Возвращает версию файла
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		public DateTime GetBinaryVersion(string code) {
			var existed =
                GridFs.Files.Find(new QueryDocument(BsonDocument.Parse("{code:'" + code + "'}")))
				         .SetFields("metadata.lastwrite").FirstOrDefault();
			if (null == existed) {
				return DateTime.MinValue;
			}
			return existed["metadata"]["lastwrite"].ToLocalTime();
		}

		/// <summary>
		/// Возвращает версию страницы
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		public DateTime GetPageVersion(string code) {
			
			var existed =
                Collection.Find(new QueryDocument(BsonDocument.Parse("{code:'" + code + "'}")))
						 .SetFields("ver").FirstOrDefault();
			if (null == existed) {
				return DateTime.MinValue;
			}
			return existed["ver"].ToLocalTime();
		}

        /// <summary>
        ///     Создание версии страницы (копии последней с комментарием)
        /// </summary>
        /// <param name="code">Код страницы</param>
        /// <param name="comment">Комментарий</param>
        /// <returns></returns>
        public WikiVersionCreateResult CreateVersion(string code, string comment) {
            var lastVersion = GetLatestVersion(code);

            if (lastVersion == null) {
                return new WikiVersionCreateResult(false, null, DateTime.MinValue, "There is no page to clone");
            }

            var cloned = CloneWikiPage(code, lastVersion.Version);

            if (cloned != null) {
                Collection.Update(
                    new QueryDocument(
                        BsonDocument.Parse("{version : '" + cloned.Version + "'}")
                    ),
                    new UpdateDocument(
                        BsonDocument.Parse("{$set : {comment : '" + comment + "'}}")    
                    ),
                    UpdateFlags.Upsert
                );

                return new WikiVersionCreateResult(true, cloned.Version, cloned.Published, "The page was cloned successful");
            }

            return new WikiVersionCreateResult(false, null, DateTime.MinValue, "Can not clone the page");
        }

        /// <summary>
        ///     Восстановление состояние страницы на момент определённой версии
        /// </summary>
        /// <param name="code">Код страницы</param>
        /// <param name="version">Идентификатор версии</param>
        /// <returns></returns>
        public WikiVersionCreateResult RestoreVersion(string code, string version) {
            var restored = GetWikiPageByVersion(code, version);
            Serializer.UpdateWikiPageVersion(restored);

            if (restored == null) {
                return new WikiVersionCreateResult(false, null, DateTime.MinValue, "There is no page to restore");
            }

            Save(restored);

            return new WikiVersionCreateResult(true, restored.Version, restored.Published, "The page was restored successful");
        }

        /// <summary>
        ///     Получить страницу Wiki определённой версии по коду страницы
        /// </summary>
        /// <param name="code">Код страницы</param>
        /// <param name="version">Код версии страницы</param>
        /// <returns></returns>
        private WikiPage GetWikiPageByVersion(string code, string version) {
            var rawDocument = Collection.Find(
                new QueryDocument (
                    BsonDocument.Parse("{'code' : '" + code + "', 'version' : '" + version + "'}")
                )
            );

            if (rawDocument == null) {
                return null;
            }

            return Serializer.ToPage(
                rawDocument.FirstOrDefault()
            );
        }

        /// <summary>
        ///     Метод клонирования страницы Wiki
        /// </summary>
        /// <param name="code"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        private WikiPage CloneWikiPage(string code, string version) {
            var currentPage = GetWikiPageByVersion(code, version);
            
            currentPage.Version = Guid.NewGuid().ToString();
            currentPage.Published = DateTime.Now;

            Save(currentPage);

            return currentPage;
        }

        /// <summary>
        ///     Метод лля получения наиболее поздней версии страницы Wiki
        /// </summary>
        /// <param name="code">Код страницы</param>
        /// <returns>Страница Wiki</returns>
        public WikiPage GetLatestVersion(string code) {
            var rawWikiPage = Collection.Find(
                new QueryDocument(
                    BsonDocument.Parse("{code : '" + code + "', published : {$exists : true}}")
                )
            ).SetSortOrder(
                new SortByDocument(
                    BsonDocument.Parse("{published : -1}")
                )
            ).SetLimit(1);

            if (rawWikiPage == null) {
                return null;
            }

            return Serializer.ToPage(
                rawWikiPage.FirstOrDefault().ToBsonDocument()
            );
        }
	}
}

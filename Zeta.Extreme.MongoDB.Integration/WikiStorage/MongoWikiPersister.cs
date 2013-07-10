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
	    private MongoDbConnector _versionsStorage;

	    private  MongoDbConnector VersionsStorage {
	        get {
	            if (_versionsStorage == null) {
	                _versionsStorage = new MongoDbConnector {
	                    CollectionName = CollectionName + "Versions",
	                    DatabaseName = DatabaseName,
	                    ConnectionString = ConnectionString
	                };
	            }

                return _versionsStorage;
	        }
	    }

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
            foreach (var code in codes) {
                var latest = GetLatestVersion(code);
                
                if (latest != null) {
                    yield return latest;
                }
            }
	    }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codes"></param>
        /// <returns></returns>
	    public IEnumerable<WikiPage> Exists(params string[] codes) {
            foreach (var code in codes) {
                var latest = GetLatestVersion(code);

                if (latest != null) {
                    yield return new WikiPage {Code = latest.Code};
                }
            }
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
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
	    public DateTime GetPageVersion(string code) {
            var latest = GetLatestVersion(code);
            
            if (latest == null) {
                return DateTime.MinValue;
            }

            return latest.Published;
        }

        /// <summary>
        ///     Push a Wiki page to the history
        /// </summary>
        /// <param name="wikiPage"></param>
        /// <param name="comment"></param>
        private WikiVersionCreateResult PushHistory(WikiPage wikiPage, string comment) {
            var version = Guid.NewGuid().ToString();
            var published = DateTime.Now;

            var writeResult = VersionsStorage.Collection.Insert(
                Serializer.NewFormPage(wikiPage).Merge(
                    new BsonDocument {
                        {"published", published},
                        {"version", version},
                        {"creator", Application.Principal.CurrentUser.Identity.Name},
                        {"comment", comment ?? "Uncommented version"},
                        {"_id", ObjectId.GenerateNewId()},
                        {"owner", wikiPage.Owner},
                        {"editor", wikiPage.Editor}
                    },

                    true
                )
            );

            if (!writeResult.Ok) {
                return new WikiVersionCreateResult(false, version, published, writeResult.ErrorMessage);
            }

            return new WikiVersionCreateResult(true, version, published, "Version created to with comment: '" + comment + "'");
        }

	    /// <summary>
        ///     Создание версии страницы (копии последней с комментарием)
        /// </summary>
        /// <param name="code">Код страницы</param>
        /// <param name="comment">Комментарий</param>
        /// <returns></returns>
        public object CreateVersion(string code, string comment) {
            var current = Collection.Find(
                new QueryDocument(
                    BsonDocument.Parse("{_id : '" + code + "'}")
                )
            ).SetLimit(1).FirstOrDefault();

            if (current == null) {
                return new WikiVersionCreateResult(false, null, DateTime.MinValue, "There is no page to clone");
            }

	        var wikiPage = Serializer.ToWikiPage.FromMain(current);

            Save(wikiPage);

            return PushHistory(wikiPage, comment);
        }

        /// <summary>
        ///     Восстановление состояние страницы на момент определённой версии
        /// </summary>
        /// <param name="code">Код страницы</param>
        /// <param name="version">Идентификатор версии</param>
        /// <returns></returns>
        public object RestoreVersion(string code, string version) {
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
        public WikiPage GetWikiPageByVersion(string code, string version) {
            var rawDocument = VersionsStorage.Collection.Find(
                new QueryDocument(
                    BsonDocument.Parse("{'code' : '" + code + "', 'version' : '" + version + "'}")
                )
            ).SetLimit(1).FirstOrDefault();

            if (rawDocument == null) {
                return null;
            }

            return Serializer.ToWikiPage.FromHistory(rawDocument);
        }


        /// <summary>
        ///     Метод лля получения наиболее поздней версии страницы Wiki
        /// </summary>
        /// <param name="code">Код страницы</param>
        /// <returns>Страница Wiki</returns>
        public WikiPage GetLatestVersion(string code) {
            var rawWikiPage = VersionsStorage.Collection.Find(
                new QueryDocument(
                    BsonDocument.Parse("{code : '" + code + "', published : {$exists : true}}")
                )
            ).SetSortOrder(
                new SortByDocument(
                    BsonDocument.Parse("{published : -1}")
                )
            ).SetLimit(1).FirstOrDefault();

            if (rawWikiPage == null) {
                rawWikiPage = Collection.Find(
                    new QueryDocument(
                        BsonDocument.Parse("{_id : '" + code + "'}")
                    )
                ).SetLimit(1).FirstOrDefault();

                if (rawWikiPage != null) {
                    return Serializer.ToWikiPage.FromMain(rawWikiPage);
                }
                
                return new WikiPage();
            }

            return Serializer.ToWikiPage.FromHistory(rawWikiPage);
        }
	}
}

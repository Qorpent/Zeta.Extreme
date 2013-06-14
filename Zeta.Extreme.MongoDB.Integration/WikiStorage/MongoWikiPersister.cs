using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Qorpent.Wiki;

namespace Zeta.Extreme.MongoDB.Integration.WikiStorage
{
	/// <summary>
	/// Реализация персистера Wiki на Mongo
	/// </summary>
	public class MongoWikiPersister : MongoBasedServiceBase,IWikiPersister
	{
		/// <summary>
		/// Создание персистера
		/// </summary>
		public MongoWikiPersister() {
			this.Serializer = new MongoWikiSerializer();
		}
		/// <summary>
		/// Компонент сериализации WikiPage - BSON
		/// </summary>
		protected MongoWikiSerializer Serializer { get; set; }

		/// <summary>
		/// Возвращает полностью загруженные страницы Wiki
		/// </summary>
		/// <param name="codes"></param>
		/// <returns></returns>
		public IEnumerable<WikiPage> Get(params string[] codes) {
			SetupConnection();			
			var qdoc = Serializer.GetQueryFromCodes(codes);
			foreach (var doc in Connector.Collection.Find(qdoc)) {
				yield return Serializer.ToPage(doc);
			}
		}

		
		/// <summary>
		/// Возвращает страницы, только с загруженным признаком хранения в БД
		/// </summary>
		/// <param name="codes"></param>
		/// <returns></returns>
		public IEnumerable<WikiPage> Exists(params string[] codes) {
			SetupConnection();
			var qdoc = Serializer.GetQueryFromCodes(codes);
			foreach (var doc in Connector.Collection.Find(qdoc).SetFields("_id")) {
				yield return new WikiPage {Code = doc["_id"].AsString};
			}
		}

		/// <summary>
		/// Метод сохранения изменений в страницу
		/// </summary>
		/// <param name="pages"></param>
		public void Save(params WikiPage[] pages) {
			SetupConnection();
			foreach (var page in pages) {
				
				var existsQuery = Serializer.GetQueryFromCodes(new[] {page.Code});
				var exists = Connector.Collection.Find(existsQuery).Count() != 0;
				if (exists) {
					var update = Serializer.UpdateFromPage(page);
					Connector.Collection.Update(existsQuery, update);
				}
				else {
					var doc = Serializer.NewFormPage(page);
					Connector.Collection.Insert(doc);
				}
			}
		}

		/// <summary>
		/// Сохраняет в Wiki файл с указанным кодом
		/// </summary>
		public void SaveBinary(WikiBinary binary) {
			SetupConnection();
			using (var s = CreateStream(binary)) {
				s.Write(binary.Data,0,binary.Data.Length);
				s.Flush();
			}
			
		}

		private MongoGridFSStream CreateStream(WikiBinary binary) {
			if (!Connector.GridFs.ExistsById(binary.Code)) {
				return ConvertToMongoGridInfo(binary);
			}
			var info = Connector.GridFs.FindOne(binary.Code);
			info.Metadata["editor"] = Application.Principal.CurrentUser.Identity.Name;
			info.Metadata["lastwrite"] = DateTime.Now;
			Connector.GridFs.SetMetadata(info,info.Metadata);
			return info.OpenWrite();

		}

		private MongoGridFSStream ConvertToMongoGridInfo(WikiBinary binary)
		{
			var md = new BsonDocument();
			md["title"] = binary.Title;
			md["editor"] = Application.Principal.CurrentUser.Identity.Name;
			md["owner"] = Application.Principal.CurrentUser.Identity.Name;
			md["lastwrite"] = DateTime.Now;
			return Connector.GridFs.Create(
				binary.Code,
				new MongoGridFSCreateOptions
				{
					Id = binary.Code,
					Metadata = md,
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
			SetupConnection();
			if (!Connector.GridFs.ExistsById(code)) return null;
			var info = Connector.GridFs.FindOne(code);
			if (!info.Metadata.Contains("lastwrite")) {
				info.Metadata["lastwrite"] = info.UploadDate;
				Connector.GridFs.SetMetadata(info,info.Metadata);
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
			SetupConnection();
			var queryJson = "{$or : [ {_id : {$regex : '(?ix)_REGEX_'}},{title:{$regex: '(?ix)_REGEX_'}},{owner:{$regex: '(?ix)_REGEX_'}},{editor:{$regex: '(?ix)_REGEX_'}}]},".Replace("_REGEX_", search);
			var queryDoc = new QueryDocument(BsonDocument.Parse(queryJson));
			foreach (var doc in Connector.Collection.Find(queryDoc).SetFields("_id","title","ver","editor","owner")) {
				var page = new WikiPage
					{
						Code = doc["_id"].AsString,
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
			SetupConnection();
			var queryJson = "{$or : [{_id : {$regex : '(?ix)_REGEX_'}},{'metadata.title':{$regex:'(?ix)_REGEX_'}},{'metadata.owner':{$regex:'(?ix)_REGEX_'}},{contentType:{$regex:'(?ix)_REGEX_'}}]}".Replace("_REGEX_", search);
			var queryDoc = new QueryDocument(BsonDocument.Parse(queryJson));
			foreach (var doc in Connector.GridFs.Find(queryDoc).SetFields("_id", "metadata.title", "metadata.owner", "length", "contentType","metadata.lastwrite", "uploadDate", "chunkSize"))
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
			SetupConnection();
			var existed =
				Connector.GridFs.Files.Find(new QueryDocument(BsonDocument.Parse("{_id:'" + code + "'}")))
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
			SetupConnection();
			var existed =
				Connector.Collection.Find(new QueryDocument(BsonDocument.Parse("{_id:'" + code + "'}")))
						 .SetFields("ver").FirstOrDefault();
			if (null == existed)
			{
				return DateTime.MinValue;
			}
			return existed["ver"].ToLocalTime();
		}
	}
}

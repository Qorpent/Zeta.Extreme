using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
			using (var s = CreateStream(binary)) {
				s.Write(binary.Data,0,binary.Data.Length);
				s.Flush();
			}
		}

		private MongoGridFSStream CreateStream(WikiBinary binary) {
			if (!Connector.GridFs.ExistsById(binary.Code)) {
				var md = new BsonDocument();
				md["title"] = binary.Title;
				md["editor"] = Application.Principal.CurrentUser.Identity.Name;
				md["owner"] = Application.Principal.CurrentUser.Identity.Name;
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
			var info = Connector.GridFs.FindOne(binary.Code);
			info.Metadata["editor"] = Application.Principal.CurrentUser.Identity.Name;
			Connector.GridFs.SetMetadata(info,info.Metadata);
			return info.OpenWrite();

		}

		/// <summary>
		/// Загружает бинарный контент
		/// </summary>
		/// <param name="code"></param>
		/// <param name="withData">Флаг, что требуется подгрузка бинарных данных</param>
		/// <returns></returns>
		public WikiBinary LoadBinary(string code, bool withData = true) {
			if (!Connector.GridFs.ExistsById(code)) return null;
			var info = Connector.GridFs.FindOne(code);
			byte[] data = null;
			if (withData) {
				data = new byte[info.Length];
				using (var s = info.OpenRead()) {
					s.Read(data, 0, (int)s.Length);
				}
			}
			var result = new WikiBinary();
			result.Code = code;
			if (withData) result.Data = data;
			result.MimeType = info.ContentType;
			result.Size = info.Length;
			result.Title = info.Metadata["title"].AsString;
			result.Owner = info.Metadata["owner"].AsString;
			result.Editor = info.Metadata["editor"].AsString;
			result.LastWriteTime = info.UploadDate;
			return result;
		}
	}
}

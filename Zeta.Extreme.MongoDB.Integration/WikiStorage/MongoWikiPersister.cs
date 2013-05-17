using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
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
	}
}

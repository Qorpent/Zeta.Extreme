using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
			throw new NotImplementedException();
		}

		/// <summary>
		/// Возвращает страницы, только с загруженным признаком хранения в БД
		/// </summary>
		/// <param name="codes"></param>
		/// <returns></returns>
		public IEnumerable<WikiPage> Exists(params string[] codes) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Метод сохранения изменений в страницу
		/// </summary>
		/// <param name="pages"></param>
		public void Save(params WikiPage[] pages) {
			throw new NotImplementedException();
		}
	}
}

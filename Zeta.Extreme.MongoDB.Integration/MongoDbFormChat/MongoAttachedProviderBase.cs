using System.Collections.ObjectModel;
using MongoDB.Bson;
using MongoDB.Driver;
using Qorpent;

namespace Zeta.Extreme.MongoDB.Integration {
	/// <summary>
	/// Provides core functionality for typical mongo-attached service
	/// </summary>
	public abstract class MongoAttachedProviderBase:ServiceBase {
		/// <summary>
		/// </summary>
		protected MongoAttachedProviderBase() {
			ConnectionString = "mongodb://localhost:27017";
			DatabaseName = "MongoAttachedProviderBase";
			CollectionName = GetType().Name;

		}

		/// <summary>
		/// </summary>
		protected MongoDatabaseSettings DbSettings;

		/// <summary>
		/// </summary>
		public MongoDatabase Database;

		/// <summary>
		/// </summary>
		public MongoCollection<BsonDocument> Collection;
		/// <summary>
		/// Conneciton string for mongo
		/// </summary>
		public string ConnectionString { get; set; }
		/// <summary>
		/// Database in mongo
		/// </summary>
		public string DatabaseName { get; set; }
		/// <summary>
		/// Collection in mongo
		/// </summary>
		public string CollectionName { get; set; }

		/// <summary>
		/// Reset connection
		/// </summary>
		public void SetupConnection()
		{
			DbSettings = new MongoDatabaseSettings();
			var mongoClient = new MongoClient(ConnectionString);
			var mongoServer = mongoClient.GetServer();
			Database = mongoServer.GetDatabase(DatabaseName, DbSettings);
			Collection = Database.GetCollection(CollectionName);
		}
	}
}
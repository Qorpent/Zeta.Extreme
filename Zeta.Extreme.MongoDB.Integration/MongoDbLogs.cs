using MongoDB.Driver;
using MongoDB.Bson;
using Qorpent.Log;
using Qorpent.Bxl;

namespace Zeta.Extreme.MongoDB.Integration {
    /// <summary>
    /// Logs on MongoDB implementation
    /// </summary>
    public class MongoDbLogs : ILogWriter {
        private MongoDatabaseSettings _mongoDatabaseSettings;
        private MongoClientSettings _mongoClientSettings;
        private MongoCollection _mongoLogsCollection;


        private bool _mongoConnected;

        private string _mongoConnectionString;
        private string _mongoDatabaseName;
        private string _mongoLogsCollectionName;

        /// <summary>
        ///     Represents the MongoDB connection string
        /// </summary>
        public string MongoConnectionString {
            get {
                return _mongoConnectionString ??
                       (_mongoConnectionString = MongoLayoutSpecification.DEFAULT_CONNECTION_STRING);
            }

            set {
                _mongoConnectionString = value;
            }
        }

        /// <summary>
        ///     Represents the MongoDB logs database name
        /// </summary>
        public string MongoLogsDatabaseName {
            get {
                return _mongoDatabaseName ?? (_mongoDatabaseName = MongoLayoutSpecification.DEFAULT_LOGS_DB);
            }

            set {
                _mongoDatabaseName = value;
            }
        }

        /// <summary>
        /// Represents the MongoDB logs collection name
        /// </summary>
        public string MongoLogsCollectionName {
            get {
                return _mongoLogsCollectionName ??
                       (_mongoLogsCollectionName = MongoLayoutSpecification.DEFAULT_LOGS_COLLECTION);
            }

            set { _mongoLogsCollectionName = value; }
        }

        /// <summary>
        /// Returns the state of the MongoDB connection
        /// </summary>
        public bool MongoConnected {
            get {
                return _mongoConnected;
            }
            set {
                
            }
        }
        /// <summary>
        ///     Minimal log level of writer
        /// </summary>
        public LogLevel Level {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public MongoDbLogs() {
            ConnectionSetup();
        }

        /// <summary>
        ///     Writes message synchronously on down level
        /// </summary>
        /// <param name="message">a log message</param>
        public void Write(LogMessage message) {
            var document = MongoDbLogsSerializer.LogMessageToBson(message);
            _mongoLogsCollection.Save(document);
        }

        private void ConnectionSetup() {
            _mongoDatabaseSettings = new MongoDatabaseSettings();
            _mongoClientSettings = new MongoClientSettings {
                Server = new MongoServerAddress(MongoConnectionString)
            };

            var mongoClient = new MongoClient(_mongoClientSettings);
            var mongoServer = mongoClient.GetServer();
            var mongoDatabase = mongoServer.GetDatabase(MongoLogsDatabaseName, _mongoDatabaseSettings);

            CollectionSetup(mongoDatabase);

            _mongoConnected = true;
        }

        private void CollectionSetup(MongoDatabase mongoDatabase) {
            _mongoLogsCollection = mongoDatabase.GetCollection(MongoLogsCollectionName);
        }
    }
}
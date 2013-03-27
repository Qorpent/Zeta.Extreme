using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
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

        // collections
        private MongoCollection _mongoUsersCollection;
        private MongoCollection _mongoFormsCollection;
        private MongoCollection _mongoServersCollection;
        private MongoCollection _mongoCompaniesCollection;
        private MongoCollection _mongoPeriodsCollection;

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
        ///     Writes message synchronously on down level
        /// </summary>
        /// <param name="message">a log message</param>
        public void Write(LogMessage message) {
            ConnectionSetup();

            if (message.Level < Level) {
                message.Level = Level;
            }

            var document = MongoDbLogsSerializer.LogMessageToBson(message);
            _mongoLogsCollection.Save(document);

            // get the ObjectId identifier
            var logItemId = document.GetValue("_id").AsObjectId;

            UpdateUsersCollection(logItemId, message);
            UpdateServersCollection(logItemId, message);
        }

        private void UpdateUsersCollection(ObjectId sourceObjectId, LogMessage message) {
            _mongoUsersCollection.Update(
                Query.EQ(
                    "user", message.Name
                ),

                Update.Push(
                   "id", sourceObjectId 
                ),

                UpdateFlags.Upsert
            );
        }

        private void UpdateServersCollection(ObjectId sourceObjectId, LogMessage message) {
            _mongoServersCollection.Update(
                Query.EQ(
                    "server", message.Server
                ),

                Update.Push(
                   "id", sourceObjectId
                ),

                UpdateFlags.Upsert
            );
        }

        private void UpdateFormsCollection(ObjectId sourceObjectId) {
            
        }

        /// <summary>
        /// Sets up connection to the database
        /// </summary>
        private void ConnectionSetup() {
            _mongoDatabaseSettings = new MongoDatabaseSettings();
            _mongoClientSettings = new MongoClientSettings {
                Server = new MongoServerAddress(MongoConnectionString)
            };

            var mongoClient = new MongoClient(_mongoClientSettings);
            var mongoServer = mongoClient.GetServer();
            var mongoDatabase = mongoServer.GetDatabase(MongoLogsDatabaseName, _mongoDatabaseSettings);

            CollectionsSetup(mongoDatabase);

            _mongoConnected = true;
        }

        /// <summary>
        /// Setups collections
        /// </summary>
        /// <param name="mongoDatabase">Mongo database pointer</param>
        private void CollectionsSetup(MongoDatabase mongoDatabase) {
            _mongoLogsCollection = mongoDatabase.GetCollection(MongoLogsCollectionName);
            _mongoUsersCollection = mongoDatabase.GetCollection(MongoLogsCollectionName + ".users");
            _mongoFormsCollection = mongoDatabase.GetCollection(MongoLogsCollectionName + ".forms");
            _mongoServersCollection = mongoDatabase.GetCollection(MongoLogsCollectionName + ".servers");
            _mongoCompaniesCollection = mongoDatabase.GetCollection(MongoLogsCollectionName + ".companies");
            _mongoPeriodsCollection = mongoDatabase.GetCollection(MongoLogsCollectionName + ".periods");
        }
    }
}
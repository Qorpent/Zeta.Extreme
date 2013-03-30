using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using Qorpent.Log;
using Qorpent.Bxl;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.MongoDB.Integration {
    /// <summary>
    /// Logs on MongoDB implementation
    /// </summary>
    public class MongoDbLogs : ILogWriter {
        private const string USERS_COLLECTION_POSTFIX = ".users";
        private const string FORMS_COLLECTION_POSTFIX = ".forms";
        private const string SERVERS_COLLECTION_POSTFIX = ".servers";
        private const string COMPANIES_COLLECTION_POSTFIX = ".companies";
        private const string PERIODS_COLLECTION_POSTFIX = ".periods";

        private MongoDatabaseSettings _mongoDatabaseSettings;
        
        // collections
        private MongoCollection _mongoLogsCollection;
        private MongoCollection _mongoUsersCollection;
        private MongoCollection _mongoFormsCollection;
        private MongoCollection _mongoServersCollection;
        private MongoCollection _mongoCompaniesCollection;
        private MongoCollection _mongoPeriodsCollection;

        /// <summary>
        ///     Connection marker
        /// </summary>
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
                       (_mongoConnectionString = MongoDbLayoutSpecification.DEFAULT_CONNECTION_STRING);
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
                return _mongoDatabaseName ?? (_mongoDatabaseName = MongoDbLayoutSpecification.DEFAULT_LOGS_DB);
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
                       (_mongoLogsCollectionName = MongoDbLayoutSpecification.DEFAULT_LOGS_COLLECTION);
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

            UpdateStatisticsCollections(
                document.GetValue("_id").AsObjectId,
                message
            );
        }

        /// <summary>
        ///     Updates or sets information in statistics collections
        /// </summary>
        /// <param name="sourceObjectId">an ObjectId of an general log element</param>
        /// <param name="message">a message item</param>
        private void UpdateStatisticsCollections(ObjectId sourceObjectId, LogMessage message) {
            UpdateUsersCollection(sourceObjectId, message);
            UpdateServersCollection(sourceObjectId, message);
            UpdateFormsCollection(sourceObjectId, message);
            UpdateCompaniesCollection(sourceObjectId, message);
            UpdatePeriodsCollection(sourceObjectId, message);
        }

        /// <summary>
        ///      Add statistics by users
        /// </summary>
        /// <param name="sourceObjectId">an ObjectId of an general log element</param>
        /// <param name="message">a message item</param>
        private void UpdateUsersCollection(ObjectId sourceObjectId, LogMessage message) {
            _mongoUsersCollection.Update(
                Query.EQ(
                    "user", message.User
                ),

                Update.Push(
                   "id", sourceObjectId 
                ),

                UpdateFlags.Upsert
            );
        }

        /// <summary>
        ///     Add statistics by servers
        /// </summary>
        /// <param name="sourceObjectId">an ObjectId of an general log element</param>
        /// <param name="message">a message item</param>
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

        /// <summary>
        ///     Add statistics by forms
        /// </summary>
        /// <param name="sourceObjectId">an ObjectId of an general log element</param>
        /// <param name="message">a message item</param>
        private void UpdateFormsCollection(ObjectId sourceObjectId, LogMessage message) {
            _mongoFormsCollection.Update(
                Query.EQ(
                    "form", ((IFormSession)message.HostObject).Template.Code
                ),

                Update.Push(
                   "id", sourceObjectId
                ),

                UpdateFlags.Upsert
            );
        }

        /// <summary>
        /// Add statistics by companies
        /// </summary>
        /// <param name="sourceObjectId">an ObjectId of an general log element</param>
        /// <param name="message">a message item</param>
        private void UpdateCompaniesCollection(ObjectId sourceObjectId, LogMessage message) {
            _mongoCompaniesCollection.Update(
                Query.EQ(
                    "company", ((IFormSession)message.HostObject).Object.Name
                ),

                Update.Push(
                   "id", sourceObjectId
                ),

                UpdateFlags.Upsert
            );
        }


        /// <summary>
        ///     Add statistics by periods
        /// </summary>
        /// <param name="sourceObjectId">an ObjectId of an general log element</param>
        /// <param name="message">a message item</param>
        private void UpdatePeriodsCollection(ObjectId sourceObjectId, LogMessage message) {
            _mongoPeriodsCollection.Update(
                Query.EQ(
                    "period", ((IFormSession)message.HostObject).Period
                ),

                Update.Push(
                   "id", sourceObjectId
                ),

                UpdateFlags.Upsert
            );
        }

        /// <summary>
        /// Sets up connection to the database
        /// </summary>
        private void ConnectionSetup() {
            _mongoDatabaseSettings = new MongoDatabaseSettings();

            var mongoClient = new MongoClient(MongoConnectionString);
            var mongoServer = mongoClient.GetServer();
            var mongoDatabase = mongoServer.GetDatabase(
                MongoLogsDatabaseName,
                _mongoDatabaseSettings
            );

            CollectionsSetup(mongoDatabase);

            _mongoConnected = true;
        }

        /// <summary>
        /// Setups collections
        /// </summary>
        /// <param name="mongoDatabase">Mongo database pointer</param>
        private void CollectionsSetup(MongoDatabase mongoDatabase) {
            _mongoLogsCollection = mongoDatabase.GetCollection(MongoLogsCollectionName);
            
            // users collection
            _mongoUsersCollection = mongoDatabase.GetCollection(
                MongoLogsCollectionName + USERS_COLLECTION_POSTFIX
            );

            // forms collection
            _mongoFormsCollection = mongoDatabase.GetCollection(
                MongoLogsCollectionName + FORMS_COLLECTION_POSTFIX
            );

            // servers collection
            _mongoServersCollection = mongoDatabase.GetCollection(
                MongoLogsCollectionName + SERVERS_COLLECTION_POSTFIX
            );

            // companies collection
            _mongoCompaniesCollection = mongoDatabase.GetCollection(
                MongoLogsCollectionName + COMPANIES_COLLECTION_POSTFIX
            );

            // periods collection
            _mongoPeriodsCollection = mongoDatabase.GetCollection(
                MongoLogsCollectionName + PERIODS_COLLECTION_POSTFIX
            );
        }
    }
}
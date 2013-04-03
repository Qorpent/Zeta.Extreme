using System.Collections.Generic;
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
        
        private int _mongoStatCollectionsPartCount;

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
        }

        /// <summary>
        ///     Minimal log level of writer
        /// </summary>
        public LogLevel Level {
            get;
            set;
        }

        /// <summary>
        ///     The count of elements in a single part of statistics collections
        /// </summary>
        public int MongoStatCollectionsPartCount {
            get {
                return
                    (_mongoStatCollectionsPartCount > 0)
                        ? (_mongoStatCollectionsPartCount )
                        : (_mongoStatCollectionsPartCount = MongoDbLayoutSpecification.MONGODBLOGS_STAT_PARTITION_COUNT);
            }

            set { _mongoStatCollectionsPartCount = value; }
        }

        /// <summary>
        ///     Creates an instance of the MongoDbLogs class
        /// </summary>
        public MongoDbLogs() {
            _mongoStatCollectionsPartCount = 0;
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

            UpdateStatisticsCollections(message);
        }

        /// <summary>
        ///     Updates or sets information in statistics collections
        /// </summary>
        /// <param name="message">a message item</param>
        private void UpdateStatisticsCollections(LogMessage message) {
            UpdateUsersCollection(message);
            UpdateServersCollection(message);
            UpdateFormsCollection(message);
            UpdateCompaniesCollection(message);
            UpdatePeriodsCollection(message);
        }

        /// <summary>
        ///      Add statistics by users
        /// </summary>
        /// <param name="message">a message item</param>
        private void UpdateUsersCollection(LogMessage message) {
            _mongoUsersCollection.Update(
                GenerateStatisticsUpdateQuery(
                    "user", message.User
                ),
                GenerateStatisticsUpdateObject(),
                UpdateFlags.Upsert
            );
        }

        /// <summary>
        ///     Add statistics by servers
        /// </summary>
        /// <param name="message">a message item</param>
        private void UpdateServersCollection(LogMessage message) {
            _mongoServersCollection.Update(
                GenerateStatisticsUpdateQuery(
                    "server",
                    message.Server
                ),
                GenerateStatisticsUpdateObject(),
                UpdateFlags.Upsert
            );
        }

        /// <summary>
        ///     Add statistics by forms
        /// </summary>
        /// <param name="message">a message item</param>
        private void UpdateFormsCollection(LogMessage message) {
            _mongoFormsCollection.Update(
                GenerateStatisticsUpdateQuery(
                    "form",
                    ((IFormSession)message.HostObject).Template.Code
                ),
                GenerateStatisticsUpdateObject(),
                UpdateFlags.Upsert
            );
        }

        /// <summary>
        /// Add statistics by companies
        /// </summary>
        /// <param name="message">a message item</param>
        private void UpdateCompaniesCollection(LogMessage message) {
            _mongoCompaniesCollection.Update(
                GenerateStatisticsUpdateQuery(
                    "company",
                    ((IFormSession)message.HostObject).Object.Name
                ),
                GenerateStatisticsUpdateObject(),
                UpdateFlags.Upsert
            );
        }

        /// <summary>
        ///     Add statistics by periods
        /// </summary>
        /// <param name="message">a message item</param>
        private void UpdatePeriodsCollection(LogMessage message) {
            _mongoPeriodsCollection.Update(
                GenerateStatisticsUpdateQuery(
                    "period",
                    ((IFormSession)message.HostObject).Period
                ),
                GenerateStatisticsUpdateObject(),
                UpdateFlags.Upsert
            );
        }

        /// <summary>
        ///     Generates a IMongoUpdate instance to increment elements count and push the source _id value
        /// </summary>
        /// <returns>IMongoUpdate clause</returns>
        private IMongoUpdate GenerateStatisticsUpdateObject() {
            var update = new UpdateBuilder();
            update.Inc(
                MongoDbLayoutSpecification.MONGODBLOGS_STAT_COUNTER_NAME,
                MongoDbLayoutSpecification.MONGODBLOGS_STAT_COUNTER_INC_VAL
            );

            return update;
        }

        /// <summary>
        ///     Generates a IMongoQuery instance to find and update or create a document
        ///     in the statistics collections
        /// </summary>
        /// <param name="element">Name of elemnt to pushing</param>
        /// <param name="value">Value of the element represents a document to pushing, e.g. period : 77</param>
        /// <returns>IMongoQuery clause</returns>
        private IMongoQuery GenerateStatisticsUpdateQuery(string element, BsonValue value) {
            return Query.And(
                new List<IMongoQuery> {
                    Query.EQ(
                        element,
                        value
                    )
                }
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
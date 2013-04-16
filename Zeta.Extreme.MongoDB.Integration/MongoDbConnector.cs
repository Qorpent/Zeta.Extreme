﻿using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;

namespace Zeta.Extreme.MongoDB.Integration {
    /// <summary>
    ///     MongoDB connector
    /// </summary>
    public class MongoDbConnector {
        private MongoClient _client;
        private MongoServer _server;
        private MongoDatabase _database;
        private MongoGridFS _gridFs;

        private string _connectionString;
        private string _databaseName;
        private string _collectionName;

        /// <summary>
        ///     MongoDB settings: db, client and GridFS
        /// </summary>
        private MongoDatabaseSettings _databaseSettings;

        /// <summary>
        ///     MongoDB GridFS settings
        /// </summary>
        private MongoGridFSSettings _gridFsSettings;

        /// <summary>
        ///     MongoDB Collection
        /// </summary>
        private MongoCollection<BsonDocument> _collection;

        /// <summary>
        ///     The database name you want to use to store attachements
        /// </summary>
        public string DatabaseName {
            get { return _databaseName ?? (_databaseName = MongoDbLayoutSpecification.DEF_DATABASE_NAME); }
            set { _databaseName = value; }
        }

        /// <summary>
        ///     connection string
        /// </summary>
        public string ConnectionString {
            get { return _connectionString ?? (_connectionString = MongoDbLayoutSpecification.DEF_CONNECTION_STRING); }
            set { _connectionString = value; }
        }

        /// <summary>
        ///     Collection name
        /// </summary>
        public string CollectionName {
            get { return _collectionName ?? (_collectionName = MongoDbLayoutSpecification.DEF_COLLECTION_NAME); }
            set { _collectionName = value; }
        }

        /// <summary>
        ///     MongoDB database setting
        /// </summary>
        public MongoDatabaseSettings DatabaseSettings {
            get {
                return _databaseSettings ?? (_databaseSettings = new MongoDatabaseSettings());
            }

            set { _databaseSettings = value; }
        }

        /// <summary>
        ///     MongoDB GridFS settings
        /// </summary>
        public MongoGridFSSettings GridFsSettings {
            get { return _gridFsSettings ?? (_gridFsSettings = new MongoGridFSSettings()); }
            set { _gridFsSettings = value; }
        }

        /// <summary>
        ///     MongoDB Client
        /// </summary>
        public MongoClient Client {
            get { return _client ?? (_client = ClientSetup()); }
            protected set { _client = value; }
        }

        /// <summary>
        ///     MongoDB Server
        /// </summary>
        public MongoServer Server {
            get { return _server ?? (_server = ServerSetup()); }
            protected set { _server = value; }
        }

        /// <summary>
        ///     MongoDB Database connection link
        /// </summary>
        public MongoDatabase Database {
            get { return _database ?? (_database = DatabaseSetup()); }
            protected set { _database = value; }
        }

        /// <summary>
        ///     MongoDB Collection link
        /// </summary>
        public MongoCollection<BsonDocument> Collection {
            get { return _collection ?? (_collection = CollectionSetup()); }
            protected set { _collection = value; }
        }

        /// <summary>
        ///     MongoDB GridFS connection link
        /// </summary>
        public MongoGridFS GridFs {
            get { return _gridFs ?? (_gridFs = GridFsSetup()); }
            protected set { _gridFs = value; }
        }

        /// <summary>
        ///     Client setup
        /// </summary>
        /// <returns></returns>
        private MongoClient ClientSetup() {
            return new MongoClient(ConnectionString);
        }

        /// <summary>
        ///     Gets server
        /// </summary>
        /// <returns></returns>
        private MongoServer ServerSetup() {
            return Client.GetServer();
        }

        /// <summary>
        ///     Gets database    
        /// </summary>
        /// <returns></returns>
        private MongoDatabase DatabaseSetup() {
            return Server.GetDatabase(
                DatabaseName,
                DatabaseSettings
            );
        }

        private MongoCollection<BsonDocument> CollectionSetup() {
            return Database.GetCollection(CollectionName);
        }

        /// <summary>
        ///     GridFS connection setup
        /// </summary>
        public MongoGridFS GridFsSetup() {
            var gridFs = new MongoGridFS(
                Database,
                GridFsSettings
            );

            GridFsEnsureIndexesAreActual(
                gridFs,
                new IndexKeysDocument {
                    {"files_id", "n"}
                }
            );

            return gridFs;
        }

        /// <summary>
        ///     MongoDB.Driver «bonus» fixing
        /// </summary>
        public void GridFsEnsureIndexesAreActual(MongoGridFS gridFs, IndexKeysDocument keys) {
            if (!gridFs.Chunks.IndexExists(keys)) {
                gridFs.Chunks.ResetIndexCache();
            }
        }
    }
}
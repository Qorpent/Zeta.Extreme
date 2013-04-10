using System.Collections.Generic;
using System.IO;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.MongoDB.Integration {
    /// <summary>
    ///     Альтернативный класс MongoDbAttachmentSource
    /// </summary>
    public class MongoDbAttachmentSource : IAttachmentStorage {
        // Internal variables to determine names of collection, db and connection string
        private string _collectionName;

        private string _connectionString;
        private string _databaseName;

        // connection links: current db, GridFS and collection 
        private MongoDatabase _db;

        // MongoDB settings: db, client and GridFS
        private MongoDatabaseSettings _dbSettings;
        private MongoGridFS _gridFs;
        private MongoGridFSSettings _gridFsSettings;

        /// <summary>
        ///     The database name you want to use to store attachements
        /// </summary>
        public string Database {
            get { return _databaseName ?? (_databaseName = MongoDbLayoutSpecification.DEFAULT_ATTACHMENTS_DB); }
            set { _databaseName = value; }
        }

        /// <summary>
        ///     The name of collection which you wanna using
        /// </summary>
        public string Collection {
            get { return _collectionName ?? (_collectionName = MongoDbLayoutSpecification.DEFAULT_ATTACHMENT_COLLECTION); }
            set { _collectionName = value; }
        }

        /// <summary>
        ///     connection string
        /// </summary>
        public string ConnectionString {
            get { return _connectionString ?? (_connectionString = MongoDbLayoutSpecification.DEFAULT_CONNECTION_STRING); }
            set { _connectionString = value; }
        }

        /// <summary>
        ///     Search mechanism to find an attachment(s)
        /// </summary>
        /// <param name="query">Запрос в виде частично или полностью заполенных полей класса Attachment</param>
        /// <returns>Перечисление полученных документов</returns>
        public IEnumerable<Attachment> Find(Attachment query) {
            SetupConnection();

            return _gridFs.Files.FindAs<BsonDocument>(
                new QueryDocument(
                    MongoDbAttachmentSourceSerializer.AttachmentToBsonForFind(query)
                )
            ).Select(
                MongoDbAttachmentSourceSerializer.BsonToAttachment
            ).ToList();
        }

        /// <summary>
        ///     Сохранение информации об аттаче в БД
        /// </summary>
        /// <param name="attachment">Описание аттача</param>
        public void Save(Attachment attachment) {
            SetupConnection();

            SetIdToAttachmentIfNull(attachment);
            CreateFileIfNotExists(attachment);

            var doc = _gridFs.Files.FindOneById(attachment.Uid);
            doc.Merge(MongoDbAttachmentSourceSerializer.AttachmentToBson(attachment), true);
            _gridFs.Files.Save(doc);
        }

        /// <summary>
        ///     (псевдо)Удаление аттача из базы
        /// </summary>
        /// <param name="attachment"></param>
        public void Delete(Attachment attachment) {
            SetupConnection();

            _gridFs.Files.Update(
                MongoDbAttachmentSourceSerializer.UpdateByUidClause(attachment), 
                Update.Set("deleted", true),
                UpdateFlags.None
            );
        }

        /// <summary>
        ///     Открытие потока на чтение или запись аттача
        /// </summary>
        /// <param name="attachment">Информация об аттаче</param>
        /// <param name="mode">Режим: чтение или запись</param>
        /// <returns>Дескриптов потока</returns>
        public Stream Open(Attachment attachment, FileAccess mode) {
            SetupConnection();

            return _gridFs.FindOneById(
                attachment.Uid
            ).Open(
                (mode == FileAccess.Write) ? (FileMode.Create) : (FileMode.OpenOrCreate),
                mode
            );
        }

        /// <summary>
        ///     Setup connection to the MongoDB using ConnectionString
        /// </summary>
        private void SetupConnection() {
            _dbSettings = new MongoDatabaseSettings();
            _gridFsSettings = new MongoGridFSSettings {
                Root = Collection
            };

            var mongoClient = new MongoClient(ConnectionString);
            var mongoServer = mongoClient.GetServer();

            _db = mongoServer.GetDatabase(Database, _dbSettings);
            _gridFs = new MongoGridFS(
                _db,
                _gridFsSettings
            );

            EnsureIndexesAreActual();
        }

        /// <summary>
        ///     MongoDB.Driver «bonus» fixing
        /// </summary>
        private void EnsureIndexesAreActual() {
            var keys = new IndexKeysDocument {
                {"files_id", "n"}
            };

            if (!_gridFs.Chunks.IndexExists(keys)) {
                _gridFs.Chunks.ResetIndexCache();
            }
        }

        /// <summary>
        ///     Sets the ObjectId to the Uid field if null
        /// </summary>
        /// <param name="attachment">an Attachment instance</param>
        private void SetIdToAttachmentIfNull(Attachment attachment) {
            if (attachment.Uid == null) {
                attachment.Uid = ObjectId.GenerateNewId().ToString();
            }
        }

        /// <summary>
        ///     Creates a GridFS file if not exists
        /// </summary>
        /// <param name="attachment">an Attachment instance</param>
        private void CreateFileIfNotExists(Attachment attachment) {
            if (_gridFs.ExistsById(attachment.Uid) == false) {
                _gridFs.Create(
                    attachment.Name,
                    new MongoGridFSCreateOptions {
                        Id = attachment.Uid
                    }
                );
            }
        }
    }
}
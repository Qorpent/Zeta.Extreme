using System.Collections.Generic;
using System.IO;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.MongoDB.Integration {
    /// <summary>
    ///     альтернативный класс MongoDbAttachmentSource с перереработанной структурой
    /// </summary>
    public class MongoDbAttachmentSource : IAttachmentStorage {
        // Internal variables to determine names of collection, db and connection string
        private MongoClientSettings _cliSettings;
        private MongoCollection _collection;
        private string _collectionName;

        /// <summary>
        ///     Connection marker
        /// </summary>
        private bool _connected;

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
            get { return _databaseName ?? (_databaseName = MongoLayoutSpecification.DEFAULT_ATTACHMENTS_DB); }
            set { _databaseName = value; }
        }

        /// <summary>
        ///     The name of collection which you wanna using
        /// </summary>
        public string Collection {
            get { return _collectionName ?? (_collectionName = MongoLayoutSpecification.DEFAULT_ATTACHMENT_COLLECTION); }
            set { _collectionName = value; }
        }

        /// <summary>
        ///     connection string
        /// </summary>
        public string ConnectionString {
            get { return _connectionString ?? (_connectionString = MongoLayoutSpecification.DEFAULT_CONNECTION_STRING); }
            set { _connectionString = value; }
        }

        /// <summary>
        ///     Search mechanism to find an attachment(s)
        /// </summary>
        /// <param name="query">Запрос в виде частично или полностью заполенных полей класса Attachment</param>
        /// <returns>Перечисление полученных документов</returns>
        public IEnumerable<Attachment> Find(Attachment query) {
            SetupConnection();

            return _collection.FindAs<BsonDocument>(
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

            attachment.Uid = attachment.Uid ?? (attachment.Uid = ObjectId.GenerateNewId().ToString());

            // creates a file view in the files collection
            _gridFs.Create(
                attachment.Name,
                new MongoGridFSCreateOptions {
                    Id = attachment.Uid
                }
            );

            // now we gonna get this description from the collection
            var doc = _collection.FindOneByIdAs<BsonDocument>(attachment.Uid);
            // and add our delta
            doc.Merge(MongoDbAttachmentSourceSerializer.AttachmentToBson(attachment));
            doc.AddRange(MongoDbAttachmentSourceSerializer.AttachmentToBsonForSave(attachment));

            _collection.Save(doc);
        }

        /// <summary>
        ///     (псевдо)Удаление аттача из базы
        /// </summary>
        /// <param name="attachment"></param>
        public void Delete(Attachment attachment) {
            SetupConnection();

            foreach (
                var document in Find(attachment).Select(
                    MongoDbAttachmentSourceSerializer.AttachmentToBson
                )
            ) {
                MongoDbAttachmentSourceSerializer.AttachmentSetDeleted(document);
                _collection.Save(document);
            }
        }

        /// <summary>
        ///     Открытие потока на чтение или запись аттача
        /// </summary>
        /// <param name="attachment">Информация об аттаче</param>
        /// <param name="mode">Режим: чтение или запись</param>
        /// <returns>Дескриптов потока</returns>
        public Stream Open(Attachment attachment, FileAccess mode) {
            SetupConnection();

            if (mode == FileAccess.Write) {
                FlushBinary(attachment);
            }

            return _gridFs.FindOneById(
                attachment.Uid
            ).Open(
                FileMode.OpenOrCreate,
                mode
            );
        }

        private void FlushBinary(Attachment attachment) {
            _collection.Save(
                MongoDbAttachmentSourceSerializer.FlushBinary(
                    _collection.FindOneByIdAs<BsonDocument>(attachment.Uid)
                )    
            );

            _db.GetCollection<BsonDocument>(Collection + ".chunks").Remove(
                MongoDbAttachmentSourceSerializer.FlushChunksByUidQuery(attachment)
            );
        }

        private void SetupConnection() {
            if (!_connected) {
                _dbSettings = new MongoDatabaseSettings();
                _gridFsSettings = new MongoGridFSSettings {
                    Root = Collection // set the collection prefix
                };

                _cliSettings = new MongoClientSettings {
                    Server = new MongoServerAddress(ConnectionString)
                };

                _db = new MongoClient(_cliSettings).GetServer().GetDatabase(Database, _dbSettings);
                _collection = _db.GetCollection<BsonDocument>(Collection + ".files");
                    // because we have to work with files collection
                _gridFs = new MongoGridFS(_db, _gridFsSettings);

                _connected = true;
            }
        }
    }
}
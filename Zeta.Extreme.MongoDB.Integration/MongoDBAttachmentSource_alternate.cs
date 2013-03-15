using System;
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
    public class MongoDbAttachmentSourceAlternate : IAttachmentStorage {
        private MongoClientSettings _cliSettings;
        private MongoCollection _collection;

        private string _collectionName;
        private string _connectionString;
        private string _databaseName;

        private MongoDatabase _db;
        private MongoGridFS _gridFs;
        private MongoDatabaseSettings _dbSettings;
        private MongoGridFSSettings _gridFsSettings;

        private bool _connected;

        /// <summary>
        ///     The database name you want to use to store the attachements
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
        ///     Поиск
        /// </summary>
        /// <param name="query">Запрос в виде частично или полностью заполенных полей класса Attachment</param>
        /// <returns>Перечисление полученных документов</returns>
        public IEnumerable<Attachment> Find(Attachment query) {
            var clause = MongoDbAttachmentSourceSerializer.AttachmentToBsonForFind(query);
            SetupConnection();

            _collection.FindAs<BsonDocument>(new QueryDocument(clause));

            return null;
        }

        /// <summary>
        ///     Сохранение информации об аттаче в БД
        /// </summary>
        /// <param name="attachment">Описание аттача</param>
        public void Save(Attachment attachment) {
            var document = MongoDbAttachmentSourceSerializer.AttachmentToBsonForFind(attachment);
            SetupConnection();

            _collection.Save(document);
        }

        /// <summary>
        ///     (псевдо)Удаление аттача из базы
        /// </summary>
        /// <param name="attachment"></param>
        public void Delete(Attachment attachment) {
            SetupConnection();
        }

        /// <summary>
        ///     Открытие потока на чтение или запись аттача
        /// </summary>
        /// <param name="attachment">Информация об аттаче</param>
        /// <param name="mode">Режим: чтение или запись</param>
        /// <returns>Дескриптов потока</returns>
        public Stream Open(Attachment attachment, FileAccess mode) {
            SetupConnection();

            return new MongoGridFS(_db, _gridFsSettings).Open(
                attachment.Uid,
                FileMode.OpenOrCreate,
                mode
            );
        }

        private void SetupConnection() {
            if(!_connected) {
                // Определимся с конфигурацией
                _dbSettings = new MongoDatabaseSettings();
                _gridFsSettings = new MongoGridFSSettings();
                _cliSettings = new MongoClientSettings {
                    Server = new MongoServerAddress(ConnectionString)
                };

                var server = new MongoClient(_cliSettings).GetServer();

                _db = server.GetDatabase(Database);
                _collection = _db.GetCollection<BsonDocument>(Collection);
                _gridFs = new MongoGridFS(_db, _gridFsSettings);

                _connected = true;
            }
        }
    }
}
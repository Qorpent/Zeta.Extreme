using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using Zeta.Extreme.BizProcess.Forms;


namespace Zeta.Extreme.MongoDB.Integration {
    /// <summary>
    ///     The representation of the Attachemnt format
    /// </summary>
    public static class MongoDbAttachmentSourceSerializer {


        /// <summary>
        /// Переформировывает attachment в BsonDocument
        /// </summary>
        /// <param name="attachment">Описание аттача типа Attachment</param>
        public static BsonDocument AttachmentToBson(Attachment attachment) {
            return new BsonDocument {
                {"_id", new ObjectId(attachment.Uid)},
                {"Filename", attachment.Name},
                {"Comment", attachment.Comment},
                {"Owner", attachment.User},
                {"Version", attachment.Version},
                {"MimeType", attachment.MimeType},
                {"Revision", attachment.Revision},
                {"Extension", attachment.Extension},
                {"MetaData", new BsonDocument(attachment.Metadata)},
                {"Deleted", false}
            };
        }

        /// <summary>
        /// Переформирование BsonDocument в Attachment
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static Attachment BsonToAttachment(BsonDocument document) {
            return new Attachment {
                Uid = document["_id"].ToString(),
                Name = document["Filename"].ToString(),
                Comment = document["Comment"].ToString(),
                User = document["Owner"].ToString(),
                Version = document["Version"].ToLocalTime(),
                MimeType = document["MimeType"].ToString(),
                Revision = document["Revision"].ToInt32(),
                Extension = document["Extension"].ToString()

            };
        }
    }

    /// <summary>
    ///     альтернативный класс MongoDbAttachmentSource с перереработанной структурой
    /// </summary>
    public class MongoDbAttachmentSourceAlternate : IAttachmentStorage {
        /// <summary>
        ///     The defenition of the default database name
        /// </summary>
        private const string DEFAULT_DB = "MongoDbAttachments";

        /// <summary>
        ///     The defenition of the default collection name
        /// </summary>
        private const string DEFAULT_COLLECTION = "AttachmentsDescription";

        private const string DEFAULT_CONNECTION_STRING = "mongodb://localhost";
        private MongoClientSettings _cliSettings;
        private MongoCollection _collection;

        private string _collectionName;
        private string _connectionString;
        private string _databaseName;

        private MongoDatabase _db;
        private MongoDatabaseSettings _dbSettings;

        private bool _connected;

        /// <summary>
        ///     The database name you want to use to store the attachements
        /// </summary>
        public string Database {
            get { return _databaseName ?? (_databaseName = DEFAULT_DB); }
            set { _databaseName = value; }
        }

        /// <summary>
        ///     The name of collection which you wanna using
        /// </summary>
        public string Collection {
            get { return _collectionName ?? (_collectionName = DEFAULT_COLLECTION); }
            set { _collectionName = value; }
        }

        /// <summary>
        ///     connection string
        /// </summary>
        public string ConnectionString {
            get { return _connectionString ?? (_connectionString = DEFAULT_CONNECTION_STRING); }
            set { _connectionString = value; }
        }

        /// <summary>
        ///     Поиск
        /// </summary>
        /// <param name="query">Запрос в виде частично или полностью заполенных полей класса Attachment</param>
        /// <returns>Перечисление полученных документов</returns>
        public IEnumerable<Attachment> Find(Attachment query) {
            SetupConnection();

            return null;
        }

        /// <summary>
        ///     Сохранение информации об аттаче в БД
        /// </summary>
        /// <param name="attachment">Описание аттача</param>
        public void Save(Attachment attachment) {
            SetupConnection();
        }

        /// <summary>
        ///     (псевдо)Удаление аттача из базы
        /// </summary>
        /// <param name="attachment"></param>
        public void Delete(Attachment attachment) {
            SetupConnection();

            var document = Find(attachment).First();
        }

        /// <summary>
        ///     Открытие потока на чтение или запись аттача
        /// </summary>
        /// <param name="attachment">Информация об аттаче</param>
        /// <param name="mode">Режим: чтение или запись</param>
        /// <returns>Дескриптов потока</returns>
        public Stream Open(Attachment attachment, FileAccess mode) {
            SetupConnection();

            return null;
        }

        private void SetupConnection() {
            if(!_connected) {
                // Определимся с конфигурацией
                _dbSettings = new MongoDatabaseSettings();
                _cliSettings = new MongoClientSettings {
                    Server = new MongoServerAddress(ConnectionString)
                };

                var server = new MongoClient(_cliSettings).GetServer();

                _db = server.GetDatabase(_databaseName);
                _collection = _db.GetCollection<BsonDocument>(_collectionName);

                _connected = true;
            }
        }
    }
}
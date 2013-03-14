using System.Collections.Generic;
using System.IO;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.MongoDB.Integration {
    /// <summary>
    ///     Реализация хранилища аттачей на MongoDB
    /// </summary>
    public class MongoDbAttachmentsSource : IAttachmentStorage {
      

        // Connection information

        
        private MongoDatabase _db;
        private MongoGridFS _gridFs;
        private string _databaseName;
        private MongoDatabaseSettings _dbSettings;
        private MongoClientSettings _cliSettings;
        private string _connectionString;
        /// <summary>
        /// Сервер по умолчанию
        /// </summary>
        public const string DEFAULT_CONNECTION_STRING = "localhost";

        /// <summary>
        /// Имя БД по умолчанию
        /// </summary>
        public const string DEFAULT_DB_NAME  = "zetaAttachments";

        /// <summary>
        /// </summary>
        public MongoDbAttachmentsSource() {
            MongoDBConnect();
            
        }

        /// <summary>
        /// Осуществляет поиск аттачментов с указанной маской поиска
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IEnumerable<Attachment> Find(Attachment query) {
            InitializeConnection();
            var endList = new List<Attachment>();

            var result = _db.GetCollection("AttachmentView").FindAs<BsonDocument>(
                Query.And(
                    new QueryDocument(
                        new InternalDocument(query)
                        )
                    )
                );

            if (result != null) {
                foreach (var el in result) {
                    endList.Add(
                        new Attachment {
                            Uid = el["_id"].ToString(),
                            Name = el["Filename"].ToString(),
                            Comment = el["Comment"].ToString(),
                            User = el["Owner"].ToString(),
                            Version = el["Version"].ToLocalTime(),
                            MimeType = el["MimeType"].ToString(),
                            Revision = el["Revision"].ToInt32(),
                            Extension = el["Extension"].ToString()
                        }
                        );
                }

                return endList;
            }
            else {
                return null;
            }
        }

        /// <summary>
        ///     Saving attachment information and preparing for writing to a stream
        /// </summary>
        /// <param name="attachment"></param>
        public void Save(Attachment attachment) {
            InitializeConnection();
            InternalDocument document = new InternalDocument(attachment);

            AttachmentViewSave(document);
         
        }

        /// <summary>
        ///     Delete an attachment
        /// </summary>
        /// <param name="attachment">attachment description</param>
        public void Delete(Attachment attachment) {
            InitializeConnection();
            _db.GetCollection("AttachmentView").Update(
                Query.And(
                    new QueryDocument(
                        new InternalDocument(attachment)
                        )
                    ),
                Update.Set(
                    "Deleted",
                    true
                    )
                );
        }

        /// <summary>
        ///     Open a stream to the current attachment
        /// </summary>
        /// <param name="attachment"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public Stream Open(Attachment attachment, FileAccess mode) {
            InitializeConnection();
            return new MongoGridFSStream(
                new MongoGridFSFileInfo(_gridFs, attachment.Uid), 
                FileMode.Append,mode
                );
        }

        private bool _initialized = false;
        private void InitializeConnection() {
            if (!_initialized) {
                _cliSettings = new MongoClientSettings();
                _cliSettings.Server = new MongoServerAddress(ConnectionString);
                _dbSettings = new MongoDatabaseSettings();
                var server = new MongoClient(_cliSettings).GetServer();
                _db = server.GetDatabase(DatabaseName, _dbSettings);
                _gridFs = new MongoGridFS(_db);
                _initialized = true;
            }
        }

        /// <summary>
        /// Строка подключения
        /// </summary>
        public string ConnectionString {
            get { return _connectionString ??(_connectionString = DEFAULT_CONNECTION_STRING); }
            set { _connectionString = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string DatabaseName {
            get { return _databaseName ?? (_databaseName  = DEFAULT_DB_NAME); }
            set { _databaseName = value; }
        }

        /// <summary>
        ///     Save the attachment view information to database
        /// </summary>
        private void AttachmentViewSave(InternalDocument document) {
            document["Deleted"] = false;
            _db.GetCollection("AttachmentView").Save(document);
        }

        /// <summary>
        ///     Class represents the attachment data in internal order
        /// </summary>
        private class InternalDocument : BsonDocument {
            public InternalDocument(Attachment attachment) {
                if (attachment.Metadata != null) {
                    AddRange(attachment.Metadata);
                }

                if (attachment.Uid != null) {
                    this["_id"] = attachment.Uid;
                }

                if (attachment.Extension != null) {
                    this["Extension"] = attachment.Extension;
                }

                if (attachment.MimeType != null) {
                    this["MimeType"] = attachment.MimeType;
                }

                if (attachment.Name != null) {
                    this["Filename"] = attachment.Name;
                }

                if (attachment.User != null) {
                    this["Owner"] = attachment.User;
                }


                this["Revision"] = attachment.Revision;

                if (attachment.Comment != null) {
                    this["Comment"] = attachment.Comment;
                }

                if (attachment.Version != null) {
                    this["Version"] = attachment.Version;
                }
            }
        }
    }
}
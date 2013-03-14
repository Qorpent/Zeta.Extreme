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
                            Uid = el.SafeGet("_id").AsString,
                            Name = el.SafeGet("Filename").AsString,
                            User = el.SafeGet("Owner").AsString,
                            Comment = el.SafeGet("Comment").AsString,
                            Version = el.SafeGet("Version").ToLocalTime(),
                            MimeType = el.SafeGet("MimeType").AsString,
                            Revision = el.SafeGet("Revision").AsInt32,
                            Extension = el.SafeGet("Extension").AsString
                        }
                    );
                }

                return endList;
            }
            
            return null;
        }

        

        /// <summary>
        ///     Saving attachment information and preparing for writing to a stream
        /// </summary>
        /// <param name="attachment"></param>
        public void Save(Attachment attachment) {
            InitializeConnection();
            AttachmentViewSave(new InternalDocument(attachment));
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
            var fs = new MongoGridFS(_db);
            return fs.Open(attachment.Uid, FileMode.OpenOrCreate, mode);
        }

        private bool _initialized;
        private void InitializeConnection() {
            if (!_initialized) {
                _cliSettings = new MongoClientSettings {
                        Server = new MongoServerAddress(ConnectionString)
                };

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
        private sealed class InternalDocument : BsonDocument {
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

                this["Version"] = attachment.Version;
            }
        }
    }
}
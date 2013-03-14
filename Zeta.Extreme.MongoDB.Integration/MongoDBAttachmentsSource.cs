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
    ///     Реализация хранилища аттачей на MongoDB
    /// </summary>
    public class MongoDbAttachmentsSource : IAttachmentStorage {
        // Connection information

        /// <summary>
        ///     Сервер по умолчанию
        /// </summary>
        public const string DEFAULT_CONNECTION_STRING = "localhost";

        /// <summary>
        ///     Имя коллекции по умолчанию
        /// </summary>
        public const string DEFAULT_COLLECTION = "AttachmentView";

        /// <summary>
        ///     Имя БД по умолчанию
        /// </summary>
        public const string DEFAULT_DB_NAME = "zetaAttachments";

        // database connection settings
        private MongoDatabase _db;
        private MongoDatabaseSettings _dbSettings;
        private MongoClientSettings _cliSettings;

        // database connection information
        private string _connectionString;
        private string _databaseName;
        
        private bool _connected;
	    private string _collectionName;

	    /// <summary>
        ///     Строка подключения
        /// </summary>
        public string ConnectionString {
            get { return _connectionString ?? (_connectionString = DEFAULT_CONNECTION_STRING); }
            set { _connectionString = value; }
        }

        /// <summary>
        ///     Имя базы данных
        /// </summary>
        public string DatabaseName {
            get { return _databaseName ?? (_databaseName = DEFAULT_DB_NAME); }
            set { _databaseName = value; }
        }

	    /// <summary>
	    ///     Имя базы данных
	    /// </summary>
	    public string CollectionName {
		    get { return _collectionName ??(_collectionName = DEFAULT_COLLECTION); }
		    set { _collectionName = value; }
	    }

	    /// <summary>
        ///     Осуществляет поиск аттачментов с указанной маской поиска
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IEnumerable<Attachment> Find(Attachment query) {
            SetupConnection();
            var endList = new List<Attachment>();

            var result = _db.GetCollection(DEFAULT_COLLECTION).FindAs<BsonDocument>(
               // Query.And(
                    new QueryDocument(new InternalDocument(query))
                 //   )
                );


            if (result != null) {
                endList.AddRange(
                    result.Select(
                        el => new Attachment {
                            Uid = el.SafeGet("_id").AsString,
                            Name = el.SafeGet("Filename").AsString,
                            User = el.SafeGet("Owner").AsString,
                            Comment = el.SafeGet("Comment").AsString,
                            Version = el.SafeGet("Version").ToLocalTime(),
                            MimeType = el.SafeGet("MimeType").AsString,
                            Revision = el.SafeGet("Revision").AsInt32,
                            Extension = el.SafeGet("Extension").AsString
                        }
                    )
                );

                return endList;
            }

            return null;
        }


        /// <summary>
        ///     Saving attachment information and preparing for writing to a stream
        /// </summary>
        /// <param name="attachment"></param>
        public void Save(Attachment attachment) {
            SetupConnection();
            AttachmentViewSave(new InternalDocument(attachment));
        }

        /// <summary>
        ///     Delete an attachment
        /// </summary>
        /// <param name="attachment">attachment description</param>
        public void Delete(Attachment attachment) {
            SetupConnection();

            _db.GetCollection(DEFAULT_COLLECTION).Update(
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
            SetupConnection();

            return new MongoGridFS(_db).Open(
                attachment.Uid,
                FileMode.OpenOrCreate,
                mode
            );
        }

        private void SetupConnection() {
            if (!_connected) {
                _cliSettings = new MongoClientSettings {
                    Server = new MongoServerAddress(ConnectionString)
                };

                _dbSettings = new MongoDatabaseSettings();
                
                // connect to the database
                _db = new MongoClient(_cliSettings).GetServer().GetDatabase(
                    DatabaseName,
                    _dbSettings
                );

                // and set the connected mark
                _connected = true;
            }
        }

        /// <summary>
        ///     Save the attachment view information to database
        /// </summary>
        private void AttachmentViewSave(InternalDocument document) {
            _db.GetCollection(DEFAULT_COLLECTION).Save(document);
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

                if (attachment.Comment != null) {
                    this["Comment"] = attachment.Comment;
                }

                this["Revision"] = attachment.Revision;
                this["Version"] = attachment.Version;

                this["Deleted"] = false;
            }
        }
    }
}
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;

using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.MongoDB.Integration {
    class MongoDBAttachmentsSource : IAttachmentStorage {
        // connection constants
        const string DB_NAME = "db";
        const string COLLECTION_NAME = "AttachmentView";

        // Connection information
        private MongoClient _client;
        private MongoServer _server;
        private MongoDatabase _database;
        private MongoGridFS _gridFS;
        private MongoCollection _mongoDBCurrentCollection;

        private BsonDocument _currentDocument;

        public MongoDBAttachmentsSource() {
            MongoDBConnect();
        }

        public IEnumerable<Attachment> Find(Attachment query) {
            return null;
        }

        public void Save(Attachment attachment) {
            _currentDocument = new BsonDocument();

            HandleVariables(attachment);
            AttachmentViewSave();
        }

        public void Delete(Attachment attachment) {

        }

        /// <summary>
        /// Open a stream to the current attachment
        /// </summary>
        /// <param name="attachment"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public Stream Open(Attachment attachment, FileAccess mode) {
            return CreateStreamToCurrentFile(mode);
        }

        /// <summary>
        /// Creates a stream to the file identified by _id in _currentDocument["_id"].ToString()
        /// </summary>
        /// <param name="mode">Acces mode to the stream</param>
        /// <returns></returns>
        private Stream CreateStreamToCurrentFile(FileAccess mode) {
            return new MongoGridFSStream(
                new MongoGridFSFileInfo(
                    _gridFS,
                    _currentDocument["_id"].ToString()
                ),
                FileMode.Append,
                mode
            );
        }

        /// <summary>
        /// Create a connection to database and select the collection in mongoDBCurrentCollection
        /// </summary>
        private void MongoDBConnect() {
            _client = new MongoClient();
            _server = _client.GetServer();
            _database = _server.GetDatabase(DB_NAME);
            _gridFS = new MongoGridFS(_database);

            _mongoDBCurrentCollection = _database.GetCollection(COLLECTION_NAME);
        }

        /// <summary>
        /// Save the attachment view information to database
        /// </summary>
        private void AttachmentViewSave() {
            _mongoDBCurrentCollection.Save(_currentDocument);
        }

        private void HandleVariables(Attachment attachment) {
            if (attachment.Uid != null) {
                _currentDocument["_id"] = attachment.Uid;
            }

            _currentDocument["File"] = new BsonDocument();
            _currentDocument["Attachment"] = new BsonDocument();

            _currentDocument["File"]["Extension"] = attachment.Extension;
            _currentDocument["File"]["MimeType"] = attachment.MimeType;
            _currentDocument["File"]["Filename"] = attachment.Name;

            _currentDocument["Attachment"]["Owner"] = attachment.User;
            _currentDocument["Attachment"]["Comment"] = attachment.Comment;
            _currentDocument["Attachment"]["Revision"] = attachment.Revision;

            _currentDocument["Metadata"] = attachment.Metadata.ToBson();

            // The "tag" field is temporarily removed
        }
    }
}

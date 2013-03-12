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

        private IDictionary<string, BsonDocument> _attachments = new Dictionary<string, BsonDocument>();

        public MongoDBAttachmentsSource() {
            MongoDBConnect();
        }

        public IEnumerable<Attachment> Find(Attachment query) {
            return null;
        }

        public void Save(Attachment attachment) {
            // Add the KeyValuePair <string, BsonDocument> to the local storage _attachments
            _attachments.Add(
                AttachmentViewSave(
                    HandleVariables(attachment)
                )
            );
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
            return CreateStreamToFile(mode, attachment.Uid);
        }

        /// <summary>
        /// Creates a stream to the file identified by _id in _currentDocument["_id"].ToString()
        /// </summary>
        /// <param name="mode">Acces mode to the stream</param>
        /// <returns></returns>
        private Stream CreateStreamToFile(FileAccess mode, string id) {
            return new MongoGridFSStream(
                new MongoGridFSFileInfo(
                    _gridFS,
                    id
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
        private KeyValuePair<string, BsonDocument> AttachmentViewSave(BsonDocument document) {
            // save the document
            _mongoDBCurrentCollection.Save(document);

            // and return back the pair as _id:Document
            return new KeyValuePair<string, BsonDocument>(
                document["_id"].ToString(),
                document
            );
        }

        private BsonDocument HandleVariables(Attachment attachment) {
            BsonDocument document = new BsonDocument();

            if (attachment.Uid != null) {
                document["_id"] = attachment.Uid;
            }

            document["File"] = new BsonDocument();
            document["Attachment"] = new BsonDocument();

            document["File"]["Extension"] = attachment.Extension;
            document["File"]["MimeType"] = attachment.MimeType;
            document["File"]["Filename"] = attachment.Name;

            document["Attachment"]["Deleted"] = false;
            document["Attachment"]["Owner"] = attachment.User;
            document["Attachment"]["Comment"] = attachment.Comment;
            document["Attachment"]["Revision"] = attachment.Revision;

            document["Metadata"] = attachment.Metadata.ToBson();

            // The "tag" field is temporarily removed

            return document;
        }
    }
}

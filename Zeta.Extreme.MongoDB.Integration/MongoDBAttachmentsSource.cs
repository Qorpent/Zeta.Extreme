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
        const string DEFAULT_DB_NAME = "db";
        const string DB_COLLECTION_NAME = "AttachmentView";

        // Connection information
        private MongoClient _client;
        private MongoServer _server;
        private MongoDatabase _database;
        private MongoGridFS _gridFS;
        private MongoCollection _mongoDBCurrentCollection;

        private BsonDocument _currentDocument;
        private string _currentDocumentID;

        public MongoDBAttachmentsSource() {
            MongoDBConnect();
            _currentDocument = new BsonDocument();
        }

        public IEnumerable<Attachment> Find(Attachment query) {
            return null;
        }

        public void Save(Attachment attachment) {
            HandleVariables(attachment);
            AttachmentViewSave();

            _currentDocumentID = _currentDocument["_id"].ToString();
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
        /// Creates a stream to the file identified by _id in _currentDocumentID
        /// </summary>
        /// <param name="mode">Acces mode to the stream</param>
        /// <returns></returns>
        private Stream CreateStreamToCurrentFile(FileAccess mode) {
            return new MongoGridFSStream(
                new MongoGridFSFileInfo(
                    _gridFS,
                    _currentDocumentID
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
            _database = _server.GetDatabase(DEFAULT_DB_NAME);
            _gridFS = new MongoGridFS(_database);

            _mongoDBCurrentCollection = _database.GetCollection(DB_COLLECTION_NAME);
        }

        /// <summary>
        /// Save the attachment view information to database
        /// </summary>
        private void AttachmentViewSave() {
            _mongoDBCurrentCollection.Save(_currentDocument);
        }

        private void HandleVariables(Attachment attachment) {
            _currentDocument["extension"] = attachment.Extension ?? "";
            _currentDocument["Code"] = attachment.Uid ?? "";
            _currentDocument["type"] = attachment.Type ?? "";
            _currentDocument["Comment"] = attachment.Comment ?? "";
        //    _currentDocument["User"] = attachment.User ?? Application.Current.Principal.CurrentUser.Identity.Name;
            _currentDocument["MimeType"] = attachment.MimeType ?? "unknown/bin";

          /*  _currentDocument["tag"] = TagHelper.ToString(
                attachment.Metadata.ToDictionary(
                    _ => _.Key == "template" ? "form" : _.Key,
                    _ => _.Value.ToString()
                )
            );

            _currentDocument["tag"] = TagHelper.Merge(attachment.Type, "/doctype:" + attachment.Type + "/");
           */
        }
    }
}

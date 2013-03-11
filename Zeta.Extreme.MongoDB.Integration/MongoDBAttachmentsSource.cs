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
            this.MongoDBConnect();
            this._currentDocument = new BsonDocument();
        }

        public IEnumerable<Attachment> Find(Attachment query) {
            return null;
        }

        public void Save(Attachment attachment) {
            this.HandleVariables(attachment);
            this.AttachmentViewSave();

            this._currentDocumentID = this._currentDocument["_id"].ToString();
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
            return this.CreateStreamToCurrentFile(mode);
        }

        /// <summary>
        /// Creates a stream to the file identified by _id in this._currentDocumentID
        /// </summary>
        /// <param name="mode">Acces mode to the stream</param>
        /// <returns></returns>
        private Stream CreateStreamToCurrentFile(FileAccess mode) {
            return new MongoGridFSStream(
                new MongoGridFSFileInfo(
                    this._gridFS,
                    this._currentDocumentID
                ),
                FileMode.Append,
                mode
            );
        }

        /// <summary>
        /// Create a connection to database and select the collection in this.mongoDBCurrentCollection
        /// </summary>
        private void MongoDBConnect() {
            this._client = new MongoClient();
            this._server = _client.GetServer();
            this._database = _server.GetDatabase(DEFAULT_DB_NAME);
            this._gridFS = new MongoGridFS(this._database);

            this._mongoDBCurrentCollection = this._database.GetCollection(DB_COLLECTION_NAME);
        }

        /// <summary>
        /// Save the attachment view information to database
        /// </summary>
        private void AttachmentViewSave() {
            this._mongoDBCurrentCollection.Insert(this._currentDocument);
        }

        private void HandleVariables(Attachment attachment) {
            this._currentDocument["extension"] = attachment.Extension ?? "";
            this._currentDocument["Code"] = attachment.Uid ?? "";
            this._currentDocument["type"] = attachment.Type ?? "";
            this._currentDocument["Comment"] = attachment.Comment ?? "";
        //    this._currentDocument["User"] = attachment.User ?? Application.Current.Principal.CurrentUser.Identity.Name;
            this._currentDocument["MimeType"] = attachment.MimeType ?? "unknown/bin";

          /*  this._currentDocument["tag"] = TagHelper.ToString(
                attachment.Metadata.ToDictionary(
                    _ => _.Key == "template" ? "form" : _.Key,
                    _ => _.Value.ToString()
                )
            );

            this._currentDocument["tag"] = TagHelper.Merge(attachment.Type, "/doctype:" + attachment.Type + "/");
           */
        }
    }
}

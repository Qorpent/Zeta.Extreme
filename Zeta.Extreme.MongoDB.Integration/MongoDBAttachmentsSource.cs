using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Builders;

using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.MongoDB.Integration {
    class MongoDBAttachmentsSource : IAttachmentStorage {
        // connection constants
        const string DB_NAME = "db";
        const string COLLECTION_NAME = "AttachmentView";

        // Connection information
        private MongoGridFS _gridFS;
        private MongoCollection _collection;

        /// <summary>
        /// Class represents the attachment data in internal order
        /// </summary>
        private class InternalDocument : BsonDocument {
            public InternalDocument(Attachment attachment) {
                // first all, we have to add the metadata 
                this.AddRange(attachment.Metadata);

                this["_id"] = attachment.Uid;

                this["Extension"] = attachment.Extension ?? "";
                this["MimeType"] = attachment.MimeType ?? "";
                this["Filename"] = attachment.Name ?? "";

                this["Deleted"] = false;
                this["Owner"] = attachment.User ?? "";
                this["Comment"] = attachment.Comment ?? "";
                this["Revision"] = attachment.Revision;
            }
        }

        private IDictionary<string, BsonDocument> _attachments = new Dictionary<string, BsonDocument>();

        public MongoDBAttachmentsSource() {
            MongoDBConnect();
        }

        public IEnumerable<Attachment> Find(Attachment query) {
            BsonDocument document = HandleVariables(query);
            IMongoQuery clause = new QueryDocument(document);

            return _collection.FindAs<IEnumerable<Attachment>>(
                Query.And(
                    clause 
                )
            ) as IEnumerable<Attachment>;
        }

        public void Save(Attachment attachment) {
            // Add the KeyValuePair <string, BsonDocument> to the local storage _attachments
            _attachments.Add(
                AttachmentViewSave(
                    HandleVariables(attachment)
                )
            );
        }

        /// <summary>
        /// Delete an attachment
        /// </summary>
        /// <param name="attachment">attachment description</param>
        public void Delete(Attachment attachment) {
            BsonDocument document = HandleVariables(attachment);
            IMongoQuery clause = new QueryDocument(document);

            _collection.Update(
                    Query.And(
                        clause
                    ),
                    Update.Set(
                        "Deleted",
                        true
                    )
                );
        }

        /// <summary>
        /// Real deleting an attachment from the database
        /// </summary>
        /// <param name="attachment">attachment description</param>
        private void DeleteReal(Attachment attachment) {
            DeleteAttachmentBinReal(attachment);    // deleting from the gridfs
            DeleteAttachmentViewReal(attachment);   // and delete the view description
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
        /// Delete the attachment represents by Attachment class
        /// </summary>
        /// <param name="attachment"></param>
        private void DeleteAttachmentBinReal(Attachment attachment) {
            _gridFS.Delete(attachment.Uid);
        }

        private void DeleteAttachmentViewReal(Attachment attachment) {
            InternalDocument document = HandleVariables(attachment);

            _collection.Remove(
                Query.EQ(
                    "_id",
                    document["_id"]
                )
            );
        }

        /// <summary>
        /// Creates a stream to the file identified by _id in _currentDocument["_id"].ToString()
        /// </summary>
        /// <param name="mode">Acces mode to the stream</param>
        /// <param name="id">mongoDB internal document/file identifier</param>
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
            MongoServer     server  = new MongoClient().GetServer();
            MongoDatabase   db      = server.GetDatabase(DB_NAME);

            _gridFS = new MongoGridFS(db);
            _collection = db.GetCollection(COLLECTION_NAME);
        }

        /// <summary>
        /// Save the attachment view information to database
        /// </summary>
        private KeyValuePair<string, BsonDocument> AttachmentViewSave(BsonDocument document) {
            _collection.Save(document);

            return new KeyValuePair<string, BsonDocument>(
                document["_id"].ToString(),
                document
            );
        }

        private InternalDocument HandleVariables(Attachment attachment) {
            return new InternalDocument(attachment);
        }
    }
}

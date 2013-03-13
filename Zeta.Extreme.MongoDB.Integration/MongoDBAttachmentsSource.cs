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

        // local storage to attachments information
        private IDictionary<string, InternalDocument> _attachments;

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

        public MongoDBAttachmentsSource() {
            MongoDBConnect();
            _attachments = new Dictionary<string, InternalDocument>();
        }

        public IEnumerable<Attachment> Find(Attachment query) {
            BsonDocument document = new InternalDocument(query);
            IMongoQuery clause = new QueryDocument(document);

            return _collection.FindAs<IEnumerable<Attachment>>(
                Query.And(
                    clause
                )
            ) as IEnumerable<Attachment>;
        }

        /// <summary>
        /// Saving attachment information and preparing for writing to a stream
        /// </summary>
        /// <param name="attachment"></param>
        public void Save(Attachment attachment) {
            InternalDocument document = new InternalDocument(attachment);

            AttachmentViewSave(document);
            _attachments.Add(document["_id"].ToString(), document);
        }

        /// <summary>
        /// Delete an attachment
        /// </summary>
        /// <param name="attachment">attachment description</param>
        public void Delete(Attachment attachment) {
            BsonDocument document = new InternalDocument(attachment);
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
        /// Creates a stream to the file identified by _id in _currentDocument["_id"].ToString()
        /// </summary>
        /// <param name="mode">Acces mode to the stream</param>
        /// <param name="id">mongoDB internal document/file identifier</param>
        /// <returns></returns>
        protected Stream CreateStreamToFile(FileAccess mode, string id) {
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
        protected void MongoDBConnect() {
            MongoServer server = new MongoClient().GetServer();
            MongoDatabase db = server.GetDatabase(DB_NAME);

            _gridFS = new MongoGridFS(db);
            _collection = db.GetCollection(COLLECTION_NAME);
        }

        /// <summary>
        /// Delete the attachment binary data
        /// </summary>
        /// <param name="attachment"></param>
        private void DeleteAttachmentBinReal(Attachment attachment) {
            _gridFS.Delete(attachment.Uid);
        }

        /// <summary>
        /// Real deletion an attachment information from the database
        /// </summary>
        /// <param name="attachment"></param>
        private void DeleteAttachmentViewReal(Attachment attachment) {
            InternalDocument document = new InternalDocument(attachment);

            _collection.Remove(
                Query.EQ(
                    "_id",
                    document["_id"]
                )
            );
        }

        /// <summary>
        /// Save the attachment view information to database
        /// </summary>
        private void AttachmentViewSave(InternalDocument document) {
            _collection.Save(document);
        }
    }
}
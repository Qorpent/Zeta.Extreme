﻿using System;
using System.IO;
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

        private IDictionary<string, BsonDocument> _attachments = new Dictionary<string, BsonDocument>();

        public MongoDBAttachmentsSource() {
            MongoDBConnect();
        }

        public IEnumerable<Attachment> Find(Attachment query) {
            BsonDocument document = HandleVariables(query);


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

        /// <summary>
        /// Delete an attachment
        /// </summary>
        /// <param name="attachment">attachment description</param>
        public void Delete(Attachment attachment) {
            BsonDocument document = HandleVariables(attachment);

            _collection.Update(
                    Query.EQ(
                        "_id",
                        document["_id"]
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
            DeleteAttachmentBin(attachment);    // deleting from the gridfs
            DeleteAttachmentView(attachment);   // and delete the view description
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

        private void DeleteAttachmentBin(Attachment attachment) {
            _gridFS.Delete(attachment.Uid);
        }

        private void DeleteAttachmentView(Attachment attachment) {
            BsonDocument document = HandleVariables(attachment);

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
            // save the document
            _collection.Save(document);

            // and return back the pair as _id:Document
            return new KeyValuePair<string, BsonDocument>(
                document["_id"].ToString(),
                document
            );
        }

        private BsonDocument HandleVariables(Attachment attachment) {
            BsonDocument document = new BsonDocument();

            document["_id"] = attachment.Uid;

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

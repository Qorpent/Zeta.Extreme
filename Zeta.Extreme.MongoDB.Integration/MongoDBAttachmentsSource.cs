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

using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver.Wrappers;

using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.MongoDB.Integration {
    class MongoDBAttachmentsSource : IAttachmentStorage {
        // connection constants
        const string DB_NAME = "db";

        // Connection information
        private MongoGridFS _gridFS;
        private MongoDatabase _db;
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

            }
        }

        public MongoDBAttachmentsSource() {
            MongoDBConnect();
            _attachments = new Dictionary<string, InternalDocument>();
        }

        public IEnumerable<Attachment> Find(Attachment query) {
            BsonDocument document = new InternalDocument(query);
            MongoCollection AttachmentView = _db.GetCollection("AttachmentView");

            var result = AttachmentView.FindOneAs<IEnumerable<Attachment>>(
                Query.And(
                    new QueryDocument(document)
                )
            );

            return null;
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
            _collection.Update(
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
            _db = server.GetDatabase(DB_NAME);

            _gridFS = new MongoGridFS(_db);
            _collection = _db.GetCollection("AttachmentView");
          
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
            document["Deleted"] = false;
            _collection.Save(document);
        }
    }
}
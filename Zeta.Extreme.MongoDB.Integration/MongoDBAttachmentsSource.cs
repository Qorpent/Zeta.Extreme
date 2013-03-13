using System;
using System.IO;
using System.Collections.Generic;

using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Builders;


using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.MongoDB.Integration {
    class MongoDBAttachmentsSource : IAttachmentStorage {
        // connection constants
        const string DB_NAME = "db";

        // Connection information
        private MongoGridFS _gridFS;
        private MongoDatabase _db;

        // local storage to attachments information
        private IDictionary<string, InternalDocument> _attachments;

        /// <summary>
        /// Class represents the attachment data in internal order
        /// </summary>
        private class InternalDocument : BsonDocument {
            public InternalDocument(Attachment attachment) {

                if (attachment.Metadata != null) {
                    this.AddRange(attachment.Metadata);
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

                if (attachment.Version != null) {
                    this["Version"] = attachment.Version;
                }

            }

        }

        public MongoDBAttachmentsSource() {
            MongoDBConnect();
            _attachments = new Dictionary<string, InternalDocument>();
        }

        public IEnumerable<Attachment> Find(Attachment query) {
            List<Attachment> endList = new List<Attachment>();

            MongoCursor<BsonDocument> result = _db.GetCollection("AttachmentView").FindAs<BsonDocument>(
                Query.And(
                    new QueryDocument(
                         new InternalDocument(query)
                     )
                )
            );

            if (result != null) {
                foreach (BsonDocument el in result) {
                    endList.Add(
                        new Attachment() {
                            Uid = el["_id"].ToString(),
                            Name = el["Filename"].ToString(),
                            Comment = el["Comment"].ToString(),
                            User = el["Owner"].ToString(),
                            Version = el["Version"].ToLocalTime(),
                            MimeType = el["MimeType"].ToString(),
                            Revision = el["Revision"].ToInt32(),
                            Extension = el["Extension"].ToString()
                        }
                    );
                }

                return endList;
            } else {
                return null;
            }
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
        }

        /// <summary>
        /// Save the attachment view information to database
        /// </summary>
        private void AttachmentViewSave(InternalDocument document) {
            document["Deleted"] = false;
            _db.GetCollection("AttachmentView").Save(document);
        }
    }
}
using System;
using MongoDB.Bson;
using MongoDB.Driver;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.MongoDB.Integration {
    /// <summary>
    ///     The representation of the Attachemnt format
    /// </summary>
    public static class MongoDbAttachmentSourceSerializer {
        /// <summary>
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        public static BsonDocument AttachmentToBson(Attachment attachment) {
            var document = new BsonDocument();

            AttachmentToBsonWithoutMetadata(attachment, document);
            if (attachment.Metadata.Count != 0) {
                document.Set("metadata", new BsonDocument(attachment.Metadata));
            }

            if (DateTime.Compare(attachment.Version, new DateTime()) != 0) {
                document.Set("uploadDate", attachment.Version);
            }
            else {
                document.Set("uploadDate", DateTime.Now);
            }

            return document;
        }

        /// <summary>
        ///     Переформировывает attachment в BsonDocument
        /// </summary>
        /// <param name="attachment"></param>
        /// <param name="document"></param>
        public static void AttachmentToBson(Attachment attachment, BsonDocument document) {
            AttachmentToBsonWithoutMetadata(attachment, document);

            if (attachment.Metadata.Count != 0) {
                document.Set("metadata", new BsonDocument(attachment.Metadata));
            }

            if (DateTime.Compare(attachment.Version, new DateTime()) != 0) {
                document.Set("uploadDate", attachment.Version);
            }
            else {
                document.Set("uploadDate", DateTime.Now);
            }
        }

        /// <summary>
        ///     Prepare document for flush binary data
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static BsonDocument FlushBinary(BsonDocument document) {
            document["length"] = 0;
            document["md5"] = 0;

            return document;
        }

        /// <summary>
        ///     Preparing query to remove chunks by Uid
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        public static QueryDocument FlushChunksByUidQuery(Attachment attachment) {
            return new QueryDocument(
                new BsonDocument {
                    {"files_id", attachment.Uid}
                }
                );
        }

        /// <summary>
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        public static BsonDocument AttachmentToBsonForFindById(Attachment attachment) {
            return new BsonDocument {
                {"_id", attachment.Uid}
            };
        }

        /// <summary>
        ///     Переформирование BsonDocument в Attachment
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static Attachment BsonToAttachment(BsonDocument document) {
            var attachment = new Attachment();

            BsonToAttachment(document, attachment);

            return attachment;
        }

        /// <summary>
        /// </summary>
        /// <param name="document"></param>
        /// <param name="attachment"></param>
        public static void BsonToAttachment(BsonDocument document, Attachment attachment) {
            if (document.Contains("_id")) attachment.Uid = document["_id"].ToString();
            if (document.Contains("filename")) attachment.Name = document["filename"].ToString();
            if (document.Contains("comment")) attachment.Comment = document["comment"].ToString();
            if (document.Contains("owner")) attachment.User = document["owner"].ToString();
            if (document.Contains("uploadDate")) attachment.Version = document["uploadDate"].ToLocalTime();
            if (document.Contains("contentType")) attachment.MimeType = document["contentType"].ToString();
            if (document.Contains("length")) attachment.Size = document["length"].ToInt64();
            if (document.Contains("revision")) attachment.Revision = document["revision"].ToInt32();
            if (document.Contains("extension")) attachment.Extension = document["extension"].ToString();
        }

        /// <summary>
        ///     Преобразует Attachment в BsonDocument, исключая непроинициализированные поля
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        public static BsonDocument AttachmentToBsonForFind(Attachment attachment) {
            var document = new BsonDocument();

            AttachmentToBsonWithoutMetadata(attachment, document);

            if (attachment.Metadata.Count != 0) {
                foreach (var el in attachment.Metadata) {
                    document["metadata" + "." + el.Key] = BsonValue.Create(el.Value);
                }
            }

            return document;
        }

        /// <summary>
        /// </summary>
        /// <param name="document"></param>
        public static void AttachmentSetDeleted(BsonDocument document) {
            document["deleted"] = true;
        }


        private static void AttachmentToBsonWithoutMetadata(Attachment attachment, BsonDocument document) {
            if (attachment.Uid != null) {
                document.Set("_id", attachment.Uid);
            }

            if (attachment.Name != null) {
                document.Set("filename", attachment.Name);
            }

            if (attachment.Comment != null) {
                document.Set("comment", attachment.Comment);
            }

            if (attachment.User != null) {
                document.Set("owner", attachment.User);
            }

            if (attachment.MimeType != null) {
                document.Set("contentType", attachment.MimeType);
            }

            if (attachment.Extension != null) {
                document.Set("extension", attachment.Extension);
            }

            if (attachment.Revision > 0) {
                document.Set("revision", attachment.Revision);
            }

            if (DateTime.Compare(attachment.Version, new DateTime()) != 0) {
                document.Set("uploadDate", attachment.Version);
            }

            document.Set("deleted", false);
        }
    }
}
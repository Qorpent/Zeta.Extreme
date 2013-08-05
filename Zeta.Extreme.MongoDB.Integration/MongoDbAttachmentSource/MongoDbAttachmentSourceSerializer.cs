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
        ///     Переформировывает attachment в BsonDocument
        /// </summary>
        /// <param name="attachment">Описание аттача типа Attachment</param>
        public static BsonDocument AttachmentToBson(Attachment attachment) {
            var document = new BsonDocument();

            document.Set("_id", attachment.Uid);
            if (attachment.Name != null) document.Set("filename", attachment.Name);
            if (attachment.Comment != null) document.Set("comment", attachment.Comment);
            if (attachment.User != null) document.Set("owner", attachment.User);
            if (attachment.MimeType != null) document.Set("contentType", attachment.MimeType);
            if (attachment.Hash != null) document.Set("md5", attachment.Hash);
            if (attachment.Size > 0) document.Set("length", attachment.Size);
            if (attachment.Revision > 0) document.Set("revision", attachment.Revision);
			
            if (attachment.Version.Year <= 1990) {
                attachment.Version = DateTime.Now;
                document.Set("uploadDate", attachment.Version);
            } else {
                document.Set("uploadDate", attachment.Version);
            }
		
	        document.Set("type", attachment.Type??"default");
            document.Set("metadata", new BsonDocument(attachment.Metadata));
            if (attachment.Extension != null) document.Set("extension", attachment.Extension);

            document.Set("deleted", false);

            return document;
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
            if (document.Contains("md5")) attachment.Hash = document["md5"].ToString();
            if (document.Contains("length")) attachment.Size = document["length"].ToInt64();
            if (document.Contains("revision")) attachment.Revision = document["revision"].ToInt32();
	        if (document.Contains("type")) attachment.Type = document["type"].ToString();
            foreach (var el in (BsonDocument)document["metadata"]) {
                attachment.Metadata[el.Name] = el.Value.ToString();
            }

            if (document.Contains("extension")) attachment.Extension = document["extension"].ToString();
        }

        /// <summary>
        ///     Преобразует Attachment в BsonDocument, исключая непроинициализированные поля
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        public static BsonDocument AttachmentToBsonForFind(Attachment attachment) {
            var document = new BsonDocument();

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

            if (DateTime.Compare(attachment.Version, new DateTime()) != 0) {
                document.Set("uploadDate", attachment.Version);
            }

            if (attachment.MimeType != null) {
                document.Set("contentType", attachment.MimeType);
            }

            if (attachment.Hash != null) {
                document.Set("md5", attachment.Hash);
            }

            if (attachment.Size > 0) {
                document.Set("length", attachment.Size);
            }

            if (attachment.Revision > 0) {
                document.Set("revision", attachment.Revision);
            }

            if (attachment.Metadata.Count != 0) {
                foreach (var el in attachment.Metadata) {
                    document.Set("metadata" + "." + el.Key, BsonValue.Create(el.Value));
                }
            }

            if (attachment.Extension != null) {
                document.Set("extension", attachment.Extension);
            }
	        if (!string.IsNullOrWhiteSpace(attachment.Type)) {
		        document.Set("type", attachment.Type);
	        }
	        document.Set("deleted", false);

            return document;
        }

        /// <summary>
        ///     Makes a QueryDocument for update a document by Uid
        /// </summary>
        /// <param name="attachment">an Attachment instance</param>
        /// <returns>QueryDocument clause</returns>
        public static QueryDocument UpdateByUidClause(Attachment attachment) {
            return new QueryDocument(
                new BsonElement(
                    "_id",
                    attachment.Uid
                )
            );
        }
    }
}
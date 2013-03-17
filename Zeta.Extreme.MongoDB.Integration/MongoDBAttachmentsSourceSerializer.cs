using System;
using System.Collections.Generic;
using MongoDB.Bson;
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
            return new BsonDocument {
                {"_id", attachment.Uid},
                {"Filename", attachment.Name},
                {"Comment", attachment.Comment},
                {"Owner", attachment.User},
                {"Version", attachment.Version},
                {"MimeType", attachment.MimeType},
                {"Revision", attachment.Revision},
                {"Extension", attachment.Extension},
                {"Metadata", new BsonDocument(attachment.Metadata)},
                {"Deleted", false}
            };
        }

        /// <summary>
        ///     Переформирование BsonDocument в Attachment
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static Attachment BsonToAttachment(BsonDocument document) {
            return new Attachment {
                Uid = document["_id"].ToString(),
                Name = document["Filename"].ToString(),
                Comment = document["Comment"].ToString(),
                User = document["Owner"].ToString(),
                Version = document["Version"].ToLocalTime(),
                MimeType = document["MimeType"].ToString(),
                Revision = document["Revision"].ToInt32(),
                Extension = document["Extension"].ToString()
            };
        }

        /// <summary>
        ///     Преобразует Attachment в BsonDocument, исключая непроинициализированные поля
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        public static BsonDocument AttachmentToBsonForFind(Attachment attachment) {
            var document = new BsonDocument();

            if (attachment.Uid != null) {
                document["_id"] = attachment.Uid;
            }

            if (attachment.Name != null) {
                document["Filename"] = attachment.Name;
            }

            if (attachment.Comment != null) {
                document["Comment"] = attachment.Comment;
            }

            if (attachment.User != null) {
                document["Owner"] = attachment.User;
            }

            if (attachment.MimeType != null) {
                document["MimeType"] = attachment.MimeType;
            }

            if (attachment.Extension != null) {
                document["Extension"] = attachment.Extension;
            }

            if (attachment.Revision > 0) {
                document["Revision"] = attachment.Revision;
            }

            if (DateTime.Compare(attachment.Version, new DateTime()) != 0) {
                document["Version"] = attachment.Version;
            }

            if (attachment.Metadata.Count != 0) {

                foreach (var el in attachment.Metadata) {
                //    string t = string.Format("Metadata.{0}", el.Key);
                  //  KeyValuePair<>
                   // document.AddRange(el);
                }
               // document["Metadata"] = new BsonDocument(attachment.Metadata);
                
            }

            // do not find deleted attachments
            document["Deleted"] = false;

            return document;
        }
    }
}
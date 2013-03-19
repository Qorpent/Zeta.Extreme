using System;
using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Bson;
using Qorpent.Utils.Extensions;
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
                {"filename", attachment.Name},
                {"comment", attachment.Comment},
                {"owner", attachment.User},
                {"uploadDate", attachment.Version},
                {"contentType", attachment.MimeType},
                {"revision", attachment.Revision},
                {"extension", attachment.Extension},
                {"metadata", attachment.Metadata.ToBsonDocument()},
                {"deleted", false}
            };
        }
		/// <summary>
		/// Null-safe хелпер для работы с Bson-доками
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="doc"></param>
		/// <param name="name"></param>
		/// <param name="def"></param>
		/// <returns></returns>
		public static T GetSafeValue<T>(this BsonDocument doc, string name, T def = default(T)) {
			if (null == doc) return def;
			if (string.IsNullOrWhiteSpace(name)) return def;
			if (!doc.Contains(name)) return def;
			var val = doc[name];
			if (typeof (T) == typeof (DateTime)) {
				if (typeof (BsonDateTime) == val.GetType()) {
					return (T)(object)(val.ToLocalTime());
				}
			}
			return val.To<T>();
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        public static BsonDocument AttachmentToBsonForSave(Attachment attachment) {
            return new BsonDocument
                {
                    {
                        "$set",
                        new BsonDocument {
                            {"comment", attachment.Comment},
                            {"owner", attachment.User},
                            {"contentType", attachment.MimeType},
                            {"revision", attachment.Revision},
                            {"extension", attachment.Extension},
                            {"metadata", new BsonDocument(attachment.Metadata)},
                            {"deleted", false}
                        }
                    }
                };
        }

        /// <summary>
        /// 
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
            return new Attachment {
                Uid = document.GetValue("_id").ToString(),
                Name = document.GetSafeValue<string>("filename"),
                Comment = document.GetSafeValue<string>("comment"),
                User = document.GetSafeValue<string>("owner"),
                Version = document.GetSafeValue<DateTime>("uploadDate").ToLocalTime(),
                MimeType = document.GetSafeValue<string>("contentType"),
                Revision = document.GetSafeValue<int>("revision"),
                Extension = document.GetSafeValue<string>("extension")
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        /// <param name="attachment"></param>
        public static void BsonToAttachment(BsonDocument document, Attachment attachment) {
            if(document.Contains("_id")) attachment.Uid = document["_id"].ToString();
            if(document.Contains("filename")) attachment.Name = document["filename"].ToString();
            if(document.Contains("comment")) attachment.Comment = document["comment"].ToString();
            if(document.Contains("owner")) attachment.User = document["owner"].ToString();
            if(document.Contains("uploadDate")) attachment.Version = document["uploadDate"].ToLocalTime();
            if(document.Contains("contentType")) attachment.MimeType = document["contentType"].ToString();
            if(document.Contains("revision")) attachment.Revision = document["revision"].ToInt32();
            if(document.Contains("extension")) attachment.Extension = document["extension"].ToString();
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
                document["filename"] = attachment.Name;
            }
            
            if (attachment.Comment != null) {
                document["comment"] = attachment.Comment;
            }

            if (attachment.User != null) {
                document["owner"] = attachment.User;
            }

            if (attachment.MimeType != null) {
                document["contentType"] = attachment.MimeType;
            }
            
            if (attachment.Extension != null) {
                document["extension"] = attachment.Extension;
            }

            if (attachment.Revision > 0) {
                document["revision"] = attachment.Revision;
            }
            
            if (DateTime.Compare(attachment.Version, new DateTime()) != 0) {
                document["uploadDate"] = attachment.Version;
            }
            
            if (attachment.Metadata.Count != 0) {
				//несколько раз же повторил, что метаданные не так ищутся на втором уровне....
				//еще показывал в консоли
                ////document["metadata"] = new BsonDocument(attachment.Metadata);
	            foreach (var pair in attachment.Metadata) {
		            if (null != pair.Value) {
			            document["metadata." + pair.Key] = BsonValue.Create(pair.Value);
		            }
	            }
            }
            
            // do not find deleted attachments
            document["deleted"] = false;

            return document;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        public static void AttachmentSetDeleted(BsonDocument document) {
            document["deleted"] = true;
        }
    }
}
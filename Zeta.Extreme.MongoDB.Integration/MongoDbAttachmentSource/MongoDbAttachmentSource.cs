using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using Qorpent.Integration.MongoDB;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.MongoDB.Integration {
    /// <summary>
    ///     Альтернативный класс MongoDbAttachmentSource
    /// </summary>
    public class MongoDbAttachmentSource : MongoDbConnector, IAttachmentStorage {
        /// <summary>
        /// 
        /// </summary>
        public override MongoGridFSSettings GridFsSettings {
            get {
                var ret = base.GridFsSettings;
                ret.Root = CollectionName;
                return ret;
            }
        }

        /// <summary>
        ///     Search mechanism to find an attachment(s)
        /// </summary>
        /// <param name="query">Запрос в виде частично или полностью заполенных полей класса Attachment</param>
        /// <returns>Перечисление полученных документов</returns>
        public IEnumerable<Attachment> Find(Attachment query) {
            return GridFs.Files.FindAs<BsonDocument>(
                new QueryDocument(
                    MongoDbAttachmentSourceSerializer.AttachmentToBsonForFind(query)
                )
            ).Select(
                MongoDbAttachmentSourceSerializer.BsonToAttachment
            ).ToList();
        }

        /// <summary>
        ///     Сохранение информации об аттаче в БД
        /// </summary>
        /// <param name="attachment">Описание аттача</param>
        public void Save(Attachment attachment) {
            SetIdToAttachmentIfNull(attachment);
            CreateFileIfNotExists(attachment);

            var doc = GridFs.Files.FindOneById(attachment.Uid);
            doc.Merge(MongoDbAttachmentSourceSerializer.AttachmentToBson(attachment), true);
            GridFs.Files.Save(doc);
        }

        /// <summary>
        ///     (псевдо)Удаление аттача из базы
        /// </summary>
        /// <param name="attachment"></param>
        public void Delete(Attachment attachment) {
            GridFs.Files.Update(
                MongoDbAttachmentSourceSerializer.UpdateByUidClause(attachment), 
                Update.Set("deleted", true),
                UpdateFlags.None
            );
        }

        /// <summary>
        ///     Открытие потока на чтение или запись аттача
        /// </summary>
        /// <param name="attachment">Информация об аттаче</param>
        /// <param name="mode">Режим: чтение или запись</param>
        /// <returns>Дескриптов потока</returns>
        public Stream Open(Attachment attachment, FileAccess mode) {
            return GridFs.FindOneById(
                attachment.Uid
            ).Open(
                (mode == FileAccess.Write) ? (FileMode.Create) : (FileMode.OpenOrCreate),
                mode
            );
        }

        /// <summary>
        ///     Sets the ObjectId to the Uid field if null
        /// </summary>
        /// <param name="attachment">an Attachment instance</param>
        private void SetIdToAttachmentIfNull(Attachment attachment) {
            if (attachment.Uid == null) {
                attachment.Uid = ObjectId.GenerateNewId().ToString();
            }
        }

        /// <summary>
        ///     Creates a GridFS file if not exists
        /// </summary>
        /// <param name="attachment">an Attachment instance</param>
        private void CreateFileIfNotExists(Attachment attachment) {
            if (GridFs.ExistsById(attachment.Uid) == false) {
                GridFs.Create(
                    attachment.Name ?? Guid.NewGuid().ToString(),
                    new MongoGridFSCreateOptions {
                        Id = attachment.Uid
                    }
                );
            }
        }
    }
}
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using Qorpent.Applications;
using Zeta.Extreme.BizProcess.Forms;
using Comdiv.Extensions;

namespace Zeta.Extreme.Form.MongoDBAttachmentSource {
    class MongoDBAttachmentStorage : IAttachmentStorage {
        const string DEFAULT_DB_NAME = "db";                        // default db to store attachments
        const string DEFAULT_AV_COLLECTION_NAME = "AttachmentView"; // default collection to store attachments

        // Connection information
        private MongoClient client;
        private MongoServer server;
        private MongoDatabase database;
        private MongoGridFS gridFS;

        // Streams
        private Stream uploadStream;
        private Stream downloadStream;

        // Current file information for insert to the database
        private BsonDocument currentFile;

        public MongoDBAttachmentStorage() {
            this.MongoDBConnect();
        }

        public IEnumerable<Attachment> Find(Attachment query) {
            //this.database.GetCollection(DEFAULT_AV_COLLECTION_NAME);
            return null;
        }

        /// <summary>
        /// Delete an attachment
        /// </summary>
        /// <param name="attachment">Attachment description</param>
        public void Delete(Attachment attachment) {
            var query = MongoDB.Driver.Builders.Query.EQ("Code", attachment.Uid);
            
            this.gridFS.Delete(attachment.Uid);
            this.database.GetCollection(DEFAULT_AV_COLLECTION_NAME).Remove(query);
        }

        public void Save(Attachment attachment) {
            // handle input data and prepare the data stream for reading
            this.SavingPrepare(attachment);

            // ... and save description and binary data
            this.AttachmentViewSave();
            this.gridFS.Upload(
                this.uploadStream,
                this.currentFile["_id"].ToString()
            );
        }

        public Stream Open(Attachment attachment, FileAccess mode) {
            Stream ReturnStream = null;

            switch(mode) {
                case FileAccess.Read:
                    this.downloadStream = new MemoryStream();
                    ReturnStream = this.downloadStream;
                    break;

                case FileAccess.Write:
                    this.uploadStream = new MemoryStream();
                    ReturnStream = this.uploadStream;
                    break;

                default:
                    throw new Exception("Not supported accesss mode " + mode);
            }

            return ReturnStream;
        }

        /// <summary>
        /// Preparing procedure to saving attachment
        /// </summary>
        /// <param name="attachment">Attachment description</param>
        private void SavingPrepare(Attachment attachment) {
            this.HandleVariables(attachment);
            this.uploadStream.Seek(0, SeekOrigin.Begin);
        }

        private void AttachmentViewSave() {
            this.database.GetCollection(DEFAULT_AV_COLLECTION_NAME).Insert(this.currentFile);
        }

        /// <summary>
        /// Init the connection to the database
        /// </summary>
        private void MongoDBConnect() {
            this.client = new MongoClient();
            this.server = client.GetServer();
            this.database = server.GetDatabase(DEFAULT_DB_NAME);
            this.gridFS = new MongoGridFS(this.database);
        }

        /// <summary>
        /// Функция обрабатывает входные параметры
        /// </summary>
        /// <param name="attachment">Входные параметры описания файла</param>
        /// <returns>Возвращает переформированные данные описания файла</returns>
        private void HandleVariables(Attachment attachment) {
            this.currentFile = new BsonDocument();

            this.currentFile["extension"] = attachment.Extension ?? "";
            this.currentFile["Code"] = attachment.Uid ?? "";
            this.currentFile["type"] = attachment.Type ?? "";
            this.currentFile["Comment"] = attachment.Comment ?? "";
            this.currentFile["User"] = attachment.User ?? Application.Current.Principal.CurrentUser.Identity.Name;
            this.currentFile["MimeType"] = attachment.MimeType ?? "unknown/bin";

            this.currentFile["tag"] = TagHelper.ToString(
                attachment.Metadata.ToDictionary(
                    _ => _.Key == "template" ? "form" : _.Key,
                    _ => _.Value.ToString()
                )
            );

            this.currentFile["tag"] = TagHelper.Merge(attachment.Type, "/doctype:" + attachment.Type + "/");
        }
    }
}

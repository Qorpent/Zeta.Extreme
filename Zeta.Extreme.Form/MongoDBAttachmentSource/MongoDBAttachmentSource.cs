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
        private MongoGridFS GridFS;

        // some system data
        private FileAccess AccessMode;

        // Streams
        private Stream UploadStream;
        private Stream DownloadStream;
        private Stream DataStream;

        // Current file information for insert to the database
        private BsonDocument CurrentFile;

        public MongoDBAttachmentStorage() {
            this.MongoConnect();
            this.DataStream = new MemoryStream();
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
            
            this.GridFS.Delete(attachment.Uid);
            this.database.GetCollection(DEFAULT_AV_COLLECTION_NAME).Remove(query);
        }

        public void Save(Attachment attachment) {
            this.HandleVariables(attachment);   // prepare variables to insert into the database
            this.SaveAttachmentView();          // first all, insert the view description
        }

        public Stream Open(Attachment attachment, FileAccess mode) {
            this.AccessMode = mode;

            switch(mode) {
                case FileAccess.Read:
                    this.DownloadStream = new MemoryStream();
                    return this.DownloadStream;
                    break;

                case FileAccess.Write:
                    this.UploadStream = new MemoryStream();
                    return this.UploadStream;
                    break;

                default:
                    return this.DataStream;
            }
        }

        /// <summary>
        /// After all binary file was uploaded onto the stream, commit it.
        /// </summary>
        /// <param name="attachment"></param>
        public void BinaryCommit(Attachment attachment) {
            this.GridFS.Upload(this.UploadStream, attachment.Uid);
        }

        /// <summary>
        /// Init the connection to the database
        /// </summary>
        private void MongoConnect() {
            this.client = new MongoClient();                        // connectint to the local server
            this.server = client.GetServer();
            this.database = server.GetDatabase(DEFAULT_DB_NAME);    // get database that specified in DEFAULT_DB_NAME
            this.GridFS = new MongoGridFS(this.database);           // and initialize the GridFS engine
        }

        /// <summary>
        /// Saving the attachement description to the database
        /// </summary>
        private void SaveAttachmentView() {
            this.database.GetCollection(DEFAULT_AV_COLLECTION_NAME).Insert(this.CurrentFile);
        }

        /// <summary>
        /// Функция обрабатывает входные параметры
        /// </summary>
        /// <param name="attachment">Входные параметры описания файла</param>
        /// <returns>Возвращает переформированные данные описания файла</returns>
        private void HandleVariables(Attachment attachment) {
            this.CurrentFile = new BsonDocument();

            this.CurrentFile["extension"] = attachment.Extension ?? "";
            this.CurrentFile["Code"] = attachment.Uid ?? "";
            this.CurrentFile["type"] = attachment.Type ?? "";
            this.CurrentFile["Comment"] = attachment.Comment ?? "";
            this.CurrentFile["User"] = attachment.User ?? Application.Current.Principal.CurrentUser.Identity.Name;
            this.CurrentFile["MimeType"] = attachment.MimeType ?? "unknown/bin";

            this.CurrentFile["tag"] = TagHelper.ToString(
                    attachment.Metadata.ToDictionary(
                        _ => _.Key == "template" ? "form" : _.Key,
                        _ => _.Value.ToString()
                    )
            );

            this.CurrentFile["tag"] = TagHelper.Merge(attachment.Type, "/doctype:" + attachment.Type + "/");
        }
    }
}

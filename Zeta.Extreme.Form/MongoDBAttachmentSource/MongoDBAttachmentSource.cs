using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Qorpent.Applications;
using Zeta.Extreme.BizProcess.Forms;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using Comdiv.Extensions;


namespace Zeta.Extreme.Form.MongoDBAttachmentSource {

    class MongoDBAttachmentStorage : IAttachmentStorage {
        const string DEFAULT_DB_NAME = "db";
        const string DEFAULT_AV_COLLECTION_NAME = "AttachmentView";

        // Connection information
        private MongoClient client;
        private MongoServer server;
        private MongoDatabase database;

        // Current file information for insert to the database
        private BsonDocument CurrentFile;

        public IEnumerable<Attachment> Find(Attachment query) {
            //this.database.GetCollection(DEFAULT_AV_COLLECTION_NAME);
            return null;
        }

        /// <summary>
        /// Delete an attachment
        /// </summary>
        /// <param name="attachment">Attachment description</param>
        public void Delete(Attachment attachment) {
            this.AttachmentRealDelete(attachment);
        }

        public void Save(Attachment attachment) {
            // nothing
        }

        public Stream Open(Attachment attachment, FileAccess mode) {
            // nothing
            return null;
        }

        /// <summary>
        /// The real function for saving an attachment in the MongoDB database
        /// </summary>
        /// <param name="attachment">Attachment description</param>
        /// <param name="FileRealTempPath">The real path where temptorary stored this file</param>
        public void AttachmentSave(Attachment attachment, string FileRealTempPath) {
            this.HandleVariables(attachment);   // prepare variables to insert into the database
            this.MongoConnect();                // establish connection to the database
            this.SaveAttachmentView();          // first all, insert the view description
            this.SaveAttachmentBinary(          // and then store the binary data
                attachment,
                FileRealTempPath
            );
        }

        private void AttachmentRealDelete(Attachment attachment) {
            MongoGridFS GridFS = new MongoGridFS(this.database);
            GridFS.Delete(attachment.Uid);
        }

        /// <summary>
        /// Init the connection to the database
        /// </summary>
        private void MongoConnect() {
            this.client = new MongoClient();
            this.server = client.GetServer();
            this.database = server.GetDatabase(DEFAULT_DB_NAME);
        }

        /// <summary>
        /// Saving the attachement description to the database
        /// </summary>
        private void SaveAttachmentView() {
            this.database.GetCollection(DEFAULT_AV_COLLECTION_NAME).Insert(this.CurrentFile);
        }

        /// <summary>
        /// Saving the birany file to the database
        /// </summary>
        /// <param name="attachment">Attachment data type, describes the uploaded file</param>
        /// <param name="FileRealTempPath">The real path where temptorary stored this file</param>
        private void SaveAttachmentBinary(Attachment attachment, string FileRealTempPath) {
            MongoGridFS GridFS = new MongoGridFS(this.database);
            GridFS.Upload(FileRealTempPath, attachment.Uid);
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

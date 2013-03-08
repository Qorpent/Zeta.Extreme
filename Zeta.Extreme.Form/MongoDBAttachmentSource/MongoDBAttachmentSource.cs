using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Qorpent.Applications;
using Zeta.Extreme.BizProcess.Forms;
using MongoDB.Driver;
using MongoDB.Bson;
using Comdiv.Extensions;


namespace Zeta.Extreme.Form.MongoDBAttachmentSource {

    class AttachmentDescription : Attachment {
        public string tag {
            get;
            set;
        }

        public string extension {
            get;
            set;
        }
    }

    class MongoDBAttachment : IAttachmentStorage {
        private const string _DEFAULT_DB_NAME = "attachments";
        private const string _DEFAULT_FILE_NAME = "noname";
        private const string _SAVE_QUERY_SKELETON = "";

        MongoClient client;
        MongoServer server;
        MongoDatabase database;
        private BsonDocument CurrentFile;

        public IEnumerable<Attachment> Find(Attachment query) {
            return null;
        }

        public void Delete(Attachment attachment) {

        }

        public void Save(Attachment attachment) {
            this.HandleVariables(attachment);
            this.MongoConnect();
            this.SaveAttachmentView();
        }

        public Stream Open(Attachment attachment, FileAccess mode) {
            return null;
        }
        /// <summary>
        /// Init the connection to the database
        /// </summary>
        private void MongoConnect() {
            this.client = new MongoClient();
            this.server = client.GetServer();
            this.database = server.GetDatabase("db");
        }
        /// <summary>
        /// Saving the attachement description to the database
        /// </summary>
        private void SaveAttachmentView() {
            this.database.GetCollection("AttachmentView").Insert(this.CurrentFile);
        }

        private void HandleEmptyVariables() {

        }

        /// <summary>
        /// Функция обрабатывает входные параметры
        /// </summary>
        /// <param name="attachment">Входные параметры описания файла</param>
        /// <returns>Возвращает переформированные данные описания файла</returns>
        private void HandleVariables(Attachment attachment) {
            this.CurrentFile = new BsonDocument();

            this.HandleEmptyVariables();

            this.CurrentFile["type"] = attachment.Type;
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
            this.CurrentFile["extension"] = attachment.Extension;
        }
    }
}

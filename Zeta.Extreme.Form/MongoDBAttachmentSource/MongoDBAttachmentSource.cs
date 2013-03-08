using System;
using System.Collections.Generic;
using System.IO;
using Qorpent.Applications;
using Zeta.Extreme.BizProcess.Forms;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Zeta.Extreme.Form.MongoDBAttachmentSource {
    class MongoDBAttachment : IAttachmentStorage {

        public IEnumerable<Attachment> Find(Attachment query) {
            return null;
        }

        public void Delete(Attachment attachment) {

        }
        public void Save(Attachment attachment) {

        }
        /// <summary>
        /// Открытие подключения к MongoDB
        /// </summary>
        /// <param name="attachment"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public Stream Open(Attachment attachment, FileAccess mode) {
            //MongoClient client = new MongoClient();
            Console.Write("{0}, {1}", attachment.Uid, attachment.Comment);
            return null;
        }

    }
}

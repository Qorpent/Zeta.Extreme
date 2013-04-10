using System;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.MongoDB.Integration.Tests {
    class MongoDbAttachmentSourceSerializerTestsBase {
        /// <summary>
        ///     Return new Attachment instance
        /// </summary>
        /// <returns></returns>
        protected Attachment GetNewAttachmentInstance() {
            var attachment = new Attachment();
            
            attachment.Uid = Guid.NewGuid().ToString();
            attachment.Name = Guid.NewGuid().ToString();
            attachment.Comment = Guid.NewGuid().ToString();
            attachment.User = Guid.NewGuid().ToString();
            attachment.Version = DateTime.Now;
            attachment.MimeType = Guid.NewGuid().ToString();
            attachment.Hash = Guid.NewGuid().ToString();
            var rand = new Random();
            attachment.Size = Convert.ToInt64(rand.Next().ToString());
            attachment.Revision = Convert.ToInt32(rand.Next().ToString());
            
            attachment.Metadata[Guid.NewGuid().ToString()] = Guid.NewGuid().ToString();
            attachment.Metadata[Guid.NewGuid().ToString()] = Guid.NewGuid().ToString();
            attachment.Metadata[Guid.NewGuid().ToString()] = Guid.NewGuid().ToString();
            attachment.Extension = Guid.NewGuid().ToString();

            return attachment;
        }
    }
}

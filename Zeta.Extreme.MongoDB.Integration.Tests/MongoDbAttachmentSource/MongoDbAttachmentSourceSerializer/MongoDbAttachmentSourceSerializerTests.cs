using System;
using MongoDB.Bson;
using NUnit.Framework;
using Zeta.Extreme.BizProcess.Forms;
using Zeta.Extreme.MongoDB.Integration;

namespace Zeta.Extreme.MongoDB.Integration.Tests {
    class MongoDbAttachmentSourceSerializerTests : MongoDbAttachmentSourceSerializerTestsBase {

        [Test]
        public void CanSerializeAttachmentToBsonAndBack() {
            var attachment = GetNewAttachmentInstance();

            var inBson = MongoDbAttachmentSourceSerializer.AttachmentToBson(attachment);
            var inAttachment = MongoDbAttachmentSourceSerializer.BsonToAttachment(inBson);

            Assert.AreEqual(attachment.Uid, inAttachment.Uid);
            Assert.AreEqual(attachment.Name, inAttachment.Name);
            Assert.AreEqual(attachment.Comment, inAttachment.Comment);
            Assert.AreEqual(attachment.User, inAttachment.User);
            Assert.AreEqual(attachment.Version.ToString(), inAttachment.Version.ToString());
            Assert.AreEqual(attachment.MimeType, inAttachment.MimeType);
            Assert.AreEqual(attachment.Hash, inAttachment.Hash);
            Assert.AreEqual(attachment.Size, inAttachment.Size);
            Assert.AreEqual(attachment.Revision, inAttachment.Revision);

            foreach (var el in attachment.Metadata) {
                Assert.AreEqual(el.Value.ToString(), inAttachment.Metadata[el.Key].ToString());
            }

            Assert.AreEqual(attachment.Extension, inAttachment.Extension);
        }

        [Test]
        public void CanParseNullReturn() {
            var attachment = MongoDbAttachmentSourceSerializer.BsonToAttachment(new BsonDocument());
            
        }
    }
}

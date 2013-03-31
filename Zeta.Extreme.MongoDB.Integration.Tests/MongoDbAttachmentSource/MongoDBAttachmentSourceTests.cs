using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Zeta.Extreme.BizProcess.Forms;

using MongoDB.Driver;
using MongoDB.Bson;

namespace Zeta.Extreme.MongoDB.Integration.Tests
{

    [TestFixture]
    public class MongoDbAttachmentSourceTests : MongoDbAttachmentSourceTestsBase {
        [Test]
        public void CanAttachmentToBsonAndBack()
        {
            var attachment = GetNewAttach();
            var reformed = MongoDbAttachmentSourceSerializer.AttachmentToBson(attachment);
            var attachmentReformed = MongoDbAttachmentSourceSerializer.BsonToAttachment(reformed);


            Assert.AreSame(attachment.Uid, attachmentReformed.Uid);
            Assert.AreSame(attachment.Name, attachmentReformed.Name); // Name
            Assert.AreSame(attachment.Comment, attachmentReformed.Comment); // Comment
            Assert.AreSame(attachment.User, attachmentReformed.User); // User
            Assert.AreNotEqual(attachment.Version, attachmentReformed.Version); // Version
            Assert.AreSame(attachment.MimeType, attachmentReformed.MimeType); // MimeType
            Assert.AreEqual(attachment.Revision, attachmentReformed.Revision); // Revision

            // CollectionAssert.AreEquivalent(attachmentReformed.Metadata, attachment.Metadata);
        }

        [Test]
        public void CanAttachmentToBsonForFind()
        {
            var attachment = GetNewAttach();

            var inBson = MongoDbAttachmentSourceSerializer.AttachmentToBsonForFind(attachment);

            Assert.AreSame(attachment.Uid, inBson["_id"].ToString());
            Assert.AreSame(attachment.Name, inBson["filename"].ToString()); // Name
            Assert.AreSame(attachment.Comment, inBson["comment"].ToString()); // Comment
            Assert.AreSame(attachment.User, inBson["owner"].ToString()); // User
            Assert.AreNotEqual(attachment.Version, inBson["uploadDate"].ToLocalTime()); // Version
            Assert.AreSame(attachment.MimeType, inBson["contentType"].ToString()); // MimeType
            Assert.AreEqual(attachment.Revision, inBson["revision"].ToInt32()); // Revision
        }


        [Test]
        public void CanFind()
        {
            var attachment = GetNewAttach();

            _mdb.Save(attachment);
            using (var stream = _mdb.Open(attachment, FileAccess.Write))
            {
                stream.Flush();
            }

            var found = _mdb.Find(attachment).FirstOrDefault();

            Assert.NotNull(found);
            Assert.AreEqual(attachment.Uid, found.Uid);
        }

        [Test]
        public void CanSaveAndThenDelete()
        {
            var attachment = GetNewAttach();

            _mdb.Save(attachment);
            using (var stream = _mdb.Open(attachment, FileAccess.Write))
            {
                stream.Flush();
            }


            var found = _mdb.Find(attachment).FirstOrDefault();

            Assert.NotNull(found);
            Assert.AreEqual(attachment.Uid, found.Uid);

            _mdb.Delete(attachment);

            var not_found = _mdb.Find(attachment).FirstOrDefault();
            Assert.IsNull(not_found);

        }

        [Test]
        public void CanDownload()
        {
            var attachment = GetNewAttach();
            byte[] someData = { 1, 2, 3, 4, 5 };
            byte[] someBuffer = { 0, 0, 0, 0, 0 };

            _mdb.Save(attachment);
            using (var stream = _mdb.Open(attachment, FileAccess.Write))
            {
                stream.Write(someData, 0, someData.Length);
                stream.Flush();
            }


            var found = _mdb.Find(attachment).FirstOrDefault();

            Assert.NotNull(found);
            Assert.AreEqual(attachment.Uid, found.Uid);
            Assert.AreNotEqual(someData, someBuffer);

            using (var stream = _mdb.Open(attachment, FileAccess.Read))
            {

                stream.Read(someBuffer, 0, someBuffer.Length);
                stream.Flush();
            }

            Assert.AreEqual(someBuffer, someData);

        }

        [Test]
        public void CanCreateBinFileAndGetUid()
        {
            var attachment = new Attachment();

            _mdb.Save(attachment);

            Assert.IsNotNull(attachment.Uid);
        }

        [Test]
        public void CanSaveAnAttachment()
        {
            var attachment = GetNewAttach();

            _mdb.Save(attachment);
            using (var stream = _mdb.Open(attachment, FileAccess.Write))
            {
                stream.Flush();
            }
        }
    }
}
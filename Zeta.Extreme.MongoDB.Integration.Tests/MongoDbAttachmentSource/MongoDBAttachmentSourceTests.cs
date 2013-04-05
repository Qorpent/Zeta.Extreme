#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : P1358Tests.cs
// Project: Zeta.Extreme.MongoDB.Integration.Tests
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.IO;
using System.Linq;
using MongoDB.Bson;
using NUnit.Framework;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.MongoDB.Integration.Tests {
    [TestFixture]
    public class MongoDbAttachmentSourceTests : MongoDbAttachmentSourceTestsBase {
        [Test]
        public void CanAttachmentToBsonAndBack() {
            Attachment attachment = GetNewAttach();
            BsonDocument reformed = MongoDbAttachmentSourceSerializer.AttachmentToBson(attachment);
            Attachment attachmentReformed = MongoDbAttachmentSourceSerializer.BsonToAttachment(reformed);

            Assert.AreSame(attachment.Uid, attachmentReformed.Uid);
            Assert.AreSame(attachment.Name, attachmentReformed.Name); // Name
            Assert.AreSame(attachment.Comment, attachmentReformed.Comment); // Comment
            Assert.AreSame(attachment.User, attachmentReformed.User); // User
            Assert.AreNotEqual(attachment.Version, attachmentReformed.Version); // Version
            Assert.AreSame(attachment.MimeType, attachmentReformed.MimeType); // MimeType
            Assert.AreEqual(attachment.Revision, attachmentReformed.Revision); // Revision
        }

        [Test]
        public void CanAttachmentToBsonForFind() {
            Attachment attachment = GetNewAttach();

            BsonDocument inBson = MongoDbAttachmentSourceSerializer.AttachmentToBsonForFind(attachment);

            Assert.AreSame(attachment.Uid, inBson["_id"].ToString());
            Assert.AreSame(attachment.Name, inBson["filename"].ToString()); // Name
            Assert.AreSame(attachment.Comment, inBson["comment"].ToString()); // Comment
            Assert.AreSame(attachment.User, inBson["owner"].ToString()); // User
            Assert.AreNotEqual(attachment.Version, inBson["uploadDate"].ToLocalTime()); // Version
            Assert.AreSame(attachment.MimeType, inBson["contentType"].ToString()); // MimeType
            Assert.AreEqual(attachment.Revision, inBson["revision"].ToInt32()); // Revision
        }

        [Test]
        public void CanCreateBinFileAndGetUid() {
            var attachment = new Attachment();

            _mdb.Save(attachment);

            Assert.IsNotNull(attachment.Uid);
        }

        [Test]
        public void CanDownload() {
            Attachment attachment = GetNewAttach();
            byte[] someData = {1, 2, 3, 4, 5};
            byte[] someBuffer = {0, 0, 0, 0, 0};

            _mdb.Save(attachment);
            using (Stream stream = _mdb.Open(attachment, FileAccess.Write)) {
                stream.Write(someData, 0, someData.Length);
                stream.Flush();
            }


            Attachment found = _mdb.Find(attachment).FirstOrDefault();

            Assert.NotNull(found);
            Assert.AreEqual(attachment.Uid, found.Uid);
            Assert.AreNotEqual(someData, someBuffer);

            using (Stream stream = _mdb.Open(attachment, FileAccess.Read)) {
                stream.Read(someBuffer, 0, someBuffer.Length);
                stream.Flush();
            }

            Assert.AreEqual(someBuffer, someData);
        }

        [Test]
        public void CanFind() {
            Attachment attachment = GetNewAttach();

            _mdb.Save(attachment);
            using (Stream stream = _mdb.Open(attachment, FileAccess.Write)) {
                stream.Flush();
            }

            Attachment found = _mdb.Find(attachment).FirstOrDefault();

            Assert.NotNull(found);
            Assert.AreEqual(attachment.Uid, found.Uid);
        }

        [Test]
        public void CanSaveAnAttachment() {
            Attachment attachment = GetNewAttach();

            _mdb.Save(attachment);
            using (Stream stream = _mdb.Open(attachment, FileAccess.Write)) {
                stream.Flush();
            }
        }

        [Test]
        public void CanSaveAndThenDelete() {
            Attachment attachment = GetNewAttach();

            _mdb.Save(attachment);
            using (Stream stream = _mdb.Open(attachment, FileAccess.Write)) {
                stream.Flush();
            }


            Attachment found = _mdb.Find(attachment).FirstOrDefault();

            Assert.NotNull(found);
            Assert.AreEqual(attachment.Uid, found.Uid);

            _mdb.Delete(attachment);

            Attachment not_found = _mdb.Find(attachment).FirstOrDefault();
            Assert.IsNull(not_found);
        }


        [Test]
        public void CanUploadAttachmentAndUpdate() {
            Attachment attachment = GetNewAttach();
            byte[] someData = { 1, 2, 3, 4, 5 };
            byte[] someData2 = { 1, 2};
            byte[] someBuffer = { 0, 0, 0, 0, 0, 0, 0, 0 };

            _mdb.Save(attachment);
            using (Stream stream = _mdb.Open(attachment, FileAccess.Write)) {
                stream.Write(someData, 0, someData.Length);
                stream.Flush();
            }

            Attachment found = _mdb.Find(attachment).FirstOrDefault();

            Assert.NotNull(found);
            Assert.AreEqual(attachment.Uid, found.Uid);
            Assert.AreNotEqual(someData, someBuffer);

            _mdb.Save(attachment);
            using (Stream stream = _mdb.Open(attachment, FileAccess.Write)) {
                stream.Write(someData2, 0, someData2.Length);
                stream.Flush();
            }
        }
    }
}
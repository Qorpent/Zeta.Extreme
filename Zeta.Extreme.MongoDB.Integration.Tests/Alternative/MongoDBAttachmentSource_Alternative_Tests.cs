using System;
using System.IO;
using NUnit.Framework;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.MongoDB.Integration.Tests.Alternative {
    [TestFixture]
    internal class MongoDBAttachmentSource_Alternative_Tests {
        private int _uid;
        private readonly MongoDbAttachmentSourceAlternate _mdb = new MongoDbAttachmentSourceAlternate();


        public Attachment GetNewAttach(string uid = null) {
            return new Attachment {
                Uid = uid ?? string.Format("{0}{1}", "Attachment", ++_uid),
                Name = string.Format("{0}{1}", "Name", _uid),
                Comment = string.Format("{0}{1}", "Comment", _uid),
                Revision = _uid,
                Version = new DateTime(1, 1, 1, 1, 1, 1, 1),
                User = string.Format("{0}{1}", "User", _uid),
                MimeType = string.Format("{0}{1}", "MimeType", _uid),
                Extension = string.Format("{0}{1}", "Extension", _uid),
                Metadata = {
                    {
                        string.Format("{0}{1}", "m1", _uid), string.Format("{0}{1}", "v1", _uid)
                    }, {
                        string.Format("{0}{1}", "m2", _uid), string.Format("{0}{1}", "v2", _uid)
                    }, {
                        string.Format("{0}{1}", "m3", _uid), string.Format("{0}{1}", "v3", _uid)
                    }
                }
            };
        }


        [Test]
        public void CanAttachmentToBsonAndBack() {
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
        public void CanAttachmentToBsonForFind() {
            var attachment = GetNewAttach();

            var inBson = MongoDbAttachmentSourceSerializer.AttachmentToBsonForFind(attachment);

            Assert.AreSame(attachment.Uid, inBson["_id"].ToString());
            Assert.AreSame(attachment.Name, inBson["Filename"].ToString()); // Name
            Assert.AreSame(attachment.Comment, inBson["Comment"].ToString()); // Comment
            Assert.AreSame(attachment.User, inBson["Owner"].ToString()); // User
            Assert.AreNotEqual(attachment.Version, inBson["Version"].ToLocalTime()); // Version
            Assert.AreSame(attachment.MimeType, inBson["MimeType"].ToString()); // MimeType
            Assert.AreEqual(attachment.Revision, inBson["Revision"].ToInt32()); // Revision
        }

        [Test]
        public void CanFind() {}

        [Test]
        public void CanOpenStream() {
            Attachment attachment = GetNewAttach();

            using (var stream = _mdb.Open(attachment, FileAccess.Write)) {
                Assert.IsNotNull(stream);
            }
        }

        [Test]
        public void CanSave() {
            var attachment = GetNewAttach();

            _mdb.Save(attachment);
        }

        [Test]
        public void CanWriteBinary() {
            var source = new byte[] {0, 1, 2, 3, 4, 5};
            var buffer = new byte[source.Length];
            Attachment attachment = GetNewAttach();

            using (var stream = _mdb.Open(attachment, FileAccess.Write)) {
                Assert.IsNotNull(stream);
                stream.Write(source, 0, source.Length);
                stream.Flush();
            }

            Assert.AreNotEqual(source, buffer);

            using (var stream = _mdb.Open(attachment, FileAccess.Read))
            {
                Assert.IsNotNull(stream);
                stream.Read(buffer, 0, buffer.Length);
                stream.Flush();
            }


            Assert.AreEqual(source, buffer);
        }
    }
}
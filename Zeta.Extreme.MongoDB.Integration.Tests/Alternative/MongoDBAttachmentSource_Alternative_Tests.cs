using NUnit.Framework;
using Zeta.Extreme.BizProcess.Forms;
using System;
using MongoDB.Bson;
using System.Collections;
using System.Collections.Generic;
using Zeta.Extreme.MongoDB.Integration;

namespace Zeta.Extreme.MongoDB.Integration.Tests.Alternative {
    [TestFixture]
    internal class MongoDBAttachmentSource_Alternative_Tests {
        private int _uid = 0;
        private MongoDbAttachmentSourceAlternate _mdb;

        public Attachment GetNewAttach(string uid = null) {
            return new Attachment {
                Uid = uid ?? string.Format("{0}{1}", "Attachment", ++_uid),
                Name = string.Format("{0}{1}", "Name", _uid),
                Comment = string.Format("{0}{1}", "Comment", _uid),
                Revision = _uid,
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
        public void CanFind() {}

        public static bool PublicInstancePropertiesEqual<T>(T self, T to, params string[] ignore) where T : class
        {
            if (self != null && to != null)
            {
                Type type = typeof(T);
                List<string> ignoreList = new List<string>(ignore);
                foreach (System.Reflection.PropertyInfo pi in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
                {
                    if (!ignoreList.Contains(pi.Name))
                    {
                        object selfValue = type.GetProperty(pi.Name).GetValue(self, null);
                        object toValue = type.GetProperty(pi.Name).GetValue(to, null);

                        if (selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue)))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            return self == to;
        }

        [Test]
        public void CanAttachmentToBsonAndBack() {
            var attachment = GetNewAttach();
            var reformed = MongoDbAttachmentSourceSerializer.AttachmentToBson(attachment);
            var attachmentReformed = MongoDbAttachmentSourceSerializer.BsonToAttachment(reformed);


            Assert.AreSame(attachment.Uid, attachmentReformed.Uid);
            Assert.AreSame(attachment.Name, attachmentReformed.Name);           // Name
            Assert.AreSame(attachment.Comment, attachmentReformed.Comment);     // Comment
            Assert.AreSame(attachment.User, attachmentReformed.User);           // User
            Assert.AreEqual(attachment.Version, attachmentReformed.Version);    // Version
            Assert.AreSame(attachment.MimeType, attachmentReformed.MimeType);   // MimeType
            Assert.AreEqual(attachment.Revision, attachmentReformed.Revision);  // Revision

           // CollectionAssert.AreEquivalent(attachmentReformed.Metadata, attachment.Metadata);
        }

        [Test]
        public void CanSave() {
            


        }
    }
}
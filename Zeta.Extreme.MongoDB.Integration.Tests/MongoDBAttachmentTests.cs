using System.Collections.Generic;
using System.IO;
using System.Linq;
using MongoDB.Driver;
using NUnit.Framework;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.MongoDB.Integration.Tests {
    [TestFixture]
    public class MongoDbAttachmentTests {
        [SetUp]
        public void Setup() {
            var db = new MongoClient().GetServer().GetDatabase("MongoDbAttachmentTests");
            db.Drop();
        }

        private byte[] _source = new byte[] { 84, 101, 115, 116, 32, 79, 75, 33 };
        private readonly MongoDbAttachmentsSource _mdb;
        private int _uid = 0;

        public MongoDbAttachmentTests() {
            _mdb = new MongoDbAttachmentsSource {
                DatabaseName = "MongoDbAttachmentTests"
            };
        }

        private IEnumerable<Attachment> Find(Attachment attachment) {
            return _mdb.Find(attachment).ToArray();
        }

        private void Save(Attachment attachment, byte[] source = null) {
            _mdb.Save(attachment);
            if (null != source) {
                using (var stream = _mdb.Open(attachment, FileAccess.Write)) {
                    stream.Write(source, 0, source.Length);
                    stream.Flush();
                }
            }
        }

        private void Delete(Attachment attachment) {
            _mdb.Delete(attachment);
        }

        private Stream Open(Attachment attachment, FileAccess mode) {
            return _mdb.Open(attachment, mode);
        }

        public void CanSaveIntoSave() {
            var source = new byte[] {84, 101, 115, 116, 32, 79, 75, 33};
            var attachment = GetNewAttach();
            var attachment2 = GetNewAttach();

            _mdb.Save(attachment);
            using (var stream = Open(attachment, FileAccess.Write)) {
                _mdb.Save(attachment2);
                using (var stream2 = Open(attachment2, FileAccess.Write)) {
                    stream2.Write(source, 0, source.Length);
                    stream2.Flush();
                }


                stream.Write(source, 0, source.Length);
                stream.Flush();
            }
        }

        private Attachment GetNewAttach() {
            return new Attachment {
                Uid = string.Format("{0}{1}", "Attachment", ++_uid),
                Name = string.Format("{0}{1}", "Name", _uid),
                Comment = string.Format("{0}{1}", "Comment", _uid),
                Revision = _uid,
                User = string.Format("{0}{1}", "User", _uid),
                MimeType = string.Format("{0}{1}", "MimeType", _uid),
                Extension = string.Format("{0}{1}", "Extension", _uid),
                Metadata = {
                    {
                        string.Format("{0}{1}", "m1", _uid), string.Format("{0}{1}", "v1", _uid)
                    }, 
                    {
                        string.Format("{0}{1}", "m2", _uid), string.Format("{0}{1}", "v2", _uid)
                    },
                    {
                        string.Format("{0}{1}", "m3", _uid), string.Format("{0}{1}", "v3", _uid)
                    }
                }
            };
        }

        private void TestFileValidlySaved(Attachment attachment, string name) {
            attachment.Name = name;
            Save(attachment);

            var attachtoFind = attachment;
            var testAttach = Find(attachtoFind).FirstOrDefault();


            Assert.NotNull(testAttach);             // Проверим, не NULL ли результат поиска
            Assert.AreEqual(name, testAttach.Name); // Проверим, соответствует ли имя найденного указанному
        }

        [Test]
        public void CanDelete() {
            var t = GetNewAttach();
            Delete(t);
        }

        [Test]
        public void CheckWasDeleted() {
            Attachment attachment = GetNewAttach();

            Save(attachment);                                   // save an attachment 
            Delete(attachment);

            var found = Find(attachment).FirstOrDefault();      // try to find it
            Assert.Null(found);                                 // compare with null
        }

        [Test]
        public void CanCreateAndSaveAnEmptyAttachment() {
            var attachment = GetNewAttach();

            Save(attachment);
            var found = Find(attachment);

            Assert.NotNull(found);
        }

        [Test]
        public void CanDoubleSave() {
            Save(GetNewAttach(), _source);
            Save(GetNewAttach(), _source);
        }

        [Test]
        public void CanFind() {
            var attachment = GetNewAttach();
            Find(attachment);
        }

        [Test]
        public void CanConfigure() {
            
        }

        [Test]
        public void CanFindByUid() {
            var attachment1 = new Attachment { Uid = "CanFindByUid1", Name = "tuponame1" };
            var attachment2 = new Attachment { Uid = "CanFindByUid2", Name = "tuponame2" };
            var query = new Attachment { Uid = "CanFindByUid1" };

            Save(attachment1);
            Save(attachment2);

            var result = Find(query);
            Assert.NotNull(result);

            var found = result.FirstOrDefault();


            Assert.AreEqual(1, result.Count());
            Assert.NotNull(found);
            Assert.AreEqual("CanFindByUid1", found.Uid);
            Assert.AreEqual("tuponame1", found.Name);
        }

        [Test]
        public void CanFindByOtherItem() {
            var attachment1 = new Attachment { Uid = "CanFindByOtherItem1", Name = "tuponame1" };
            var attachment2 = new Attachment { Uid = "CanFindByOtherItem2", Name = "tuponame2" };
            var attachment3 = new Attachment { Uid = "CanFindByOtherItem3", Name = "tuponame3" };
            var attachment4 = new Attachment { Uid = "CanFindByOtherItem4", Name = "tuponame3" };

            var query = new Attachment { Name = "tuponame3" };

            Save(attachment1);
            Save(attachment2);
            Save(attachment3);
            Save(attachment4);

            var result = Find(query);
            Assert.NotNull(result);

            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void CanRewriteProperties() {
            Attachment attachment = GetNewAttach();

            TestFileValidlySaved(attachment, "First Name");
            TestFileValidlySaved(attachment, "Second Name");
            TestFileValidlySaved(attachment, "Third Name");
        }

        [Test]
        public void CanSave() {
            var attachment = GetNewAttach();

            Save(attachment, _source);
        }

        [Test]
        public void CanSaveEmptyFile() {
            var attachment = GetNewAttach();
            Save(attachment);
        }
    }
}
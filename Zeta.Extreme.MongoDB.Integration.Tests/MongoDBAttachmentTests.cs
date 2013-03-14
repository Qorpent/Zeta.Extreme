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

        private void Delete(FormAttachment attachment) {
            _mdb.Delete(attachment);
        }

        private Stream Open(Attachment attachment, FileAccess mode) {
            return _mdb.Open(attachment, mode);
        }

        public void CanSaveIntoSave() {
            var source = new byte[] {84, 101, 115, 116, 32, 79, 75, 33};
            var attachment = new FormAttachment {
                Uid = "Test_OK2",
                Extension = "sda",
                MimeType = "dada",
                User = "remalloc",
                Comment = "test",
                Revision = 123456789,
                Name = "Test OK File",
                Type = "mdb-test",
                Metadata = {
                    {
                        "s", "sdffsdf"
                    }, {
                        "test", "ok"
                    }
                }
            };

            var attachment2 = new FormAttachment {
                Uid = "Test_OK3",
                Extension = "sda",
                MimeType = "dada",
                User = "remalloc",
                Comment = "test",
                Revision = 123456789,
                Name = "Test OK File",
                Type = "mdb-test",
                Metadata = {
                    {
                        "Owner", "FUCKKKKKKK!"
                    }, {
                        "test", "ok"
                    }, {
                        "InternalDocument", "InternalDocument"
                    }
                }
            };

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
            return new Attachment() {
                Uid = string.Format("{0}{1}", "Attachment", ++_uid),
                Name = string.Format("{0}{1}", "Name", _uid),
                Comment = string.Format("{0}{1}", "Comment", _uid),
                Revision = _uid,
                User = string.Format("{0}{1}", "User", _uid),
                MimeType = string.Format("{0}{1}", "MimeType", _uid),
                Extension = string.Format("{0}{1}", "Extension", _uid)
            };
        }

        private void TestFileValidlySaved(Attachment attachment, string name) {
            attachment.Name = name;
            Save(attachment);

            var attachtoFind = new Attachment {
                Uid = attachment.Uid,
                Revision = 123456789
            };

            var testAttach = Find(attachtoFind).FirstOrDefault();


            Assert.NotNull(testAttach);             // Проверим, не NULL ли результат поиска
            Assert.AreEqual(name, testAttach.Name); // Проверим, соответствует ли имя найденного указанному
        }

        [Test]
        public void CanDelete() {
            var attachment = new FormAttachment {
                Uid = "Test_OK2",
                User = "remalloc",
                Comment = "test",
                Revision = 123456789
            };

            Delete(attachment);
        }

        [Test]
        public void CheckWasDeleted() {
            var attachment = new FormAttachment
            {
                Uid = "CheckWasDeleted",
                User = "remalloc",
                Comment = "test",
                Revision = 123456789
            };

            Save(attachment);                                   // save an attachment 
            Delete(attachment);

            var found = Find(attachment).FirstOrDefault(); ;    // try to find it
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
            var source = new byte[] {84, 101, 115, 116, 32, 79, 75, 33};
            var attachment = new FormAttachment {
                Uid = "Test_OK2",
                Name = "Test OK File",
                Type = "mdb-test"
            };

            var attachment2 = new FormAttachment {
                Uid = "Test_OK_Double",
                Name = "Test OK File Double",
                Type = "mdb-test"
            };

            Save(attachment, source);
            Save(attachment2, source);
        }

        [Test]
        public void CanFind() {
            var attachment = new FormAttachment {
                User = "remalloc",
                Revision = 123456789
            };

            Find(attachment);
        }

        [Test]
        public void CanConfigure() {
            
        }

        [Test]
        public void CanFindByUid() {
            var attachment1 = new Attachment {Uid = "simpleuid", Name = "tuponame"};
            var attachment2 = new Attachment {Uid = "simpleuid2", Name = "tuponame"};
            var query = new Attachment {Uid = "simpleuid"};

            Save(attachment1);
            Save(attachment2);

            var result = Find(query);
            Assert.NotNull(result);

            var found = result.First();


            Assert.AreEqual(1, result.Count());
            Assert.NotNull(found);
            Assert.AreEqual("simpleuid", found.Uid);
            Assert.AreEqual("tuponame", found.Name);
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
            var attachment = new Attachment {
                Uid = "Test_OK2",
                Extension = "sda",
                MimeType = "dada",
                User = "remalloc",
                Comment = "test",
                Revision = 123456789,
                Type = "mdb-test",
                Metadata = {
                    {
                        "s", "sdffsdf"
                    }, {
                        "test", "ok"
                    }
                }
            };


            TestFileValidlySaved(attachment, "First Name");
            TestFileValidlySaved(attachment, "Second Name");
            TestFileValidlySaved(attachment, "Third Name");
        }

        [Test]
        public void CanSave() {
            var source = new byte[] {84, 101, 115, 116, 32, 79, 75, 33};
            var attachment = new FormAttachment {
                Uid = "Test_OK2",
                Extension = "sda",
                MimeType = "dada",
                User = "remalloc",
                Comment = "test",
                Revision = 123456789,
                Name = "Test OK FileHH",
                Type = "mdb-test",
                Metadata = {
                    {
                        "s", "sdffsdf"
                    }, {
                        "test", "ok"
                    }
                }
            };

            Save(attachment, source);
        }

        [Test]
        public void CanSaveEmptyFile() {
            var attachment = new FormAttachment {
                Uid = "Test_OK2",
                Extension = "sda",
                MimeType = "dada",
                User = "remalloc",
                Comment = "test",
                Revision = 123456789,
                Name = "Test OK File",
                Type = "mdb-test",
                Metadata = {
                    {
                        "s", "sdffsdf"
                    }, {
                        "test", "ok"
                    }
                }
            };

            Save(attachment);
        }
    }
}
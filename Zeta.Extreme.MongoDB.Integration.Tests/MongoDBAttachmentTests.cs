using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MongoDB.Driver;
using NUnit.Framework;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.MongoDB.Integration.Tests {
    [TestFixture]
    public class MongoDbAttachmentTests {
        private readonly MongoDbAttachmentsSource _mdb;

        /// <summary>
        ///     Constructor
        /// </summary>
        public MongoDbAttachmentTests() {
           
            _mdb = new MongoDbAttachmentsSource {DatabaseName = "MongoDbAttachmentTests"};
        }
        [SetUp]
        public void Setup() {
            var db = new MongoClient().GetServer().GetDatabase("MongoDbAttachmentTests");
            db.Drop();
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

        private IEnumerable<Attachment> Find(Attachment attachment) {
            var t = _mdb.Find(attachment);


            foreach (var item in t) {
                Console.WriteLine("Uid : {0}", item.Uid);
            }

            Console.WriteLine("---");

            var result = t.ToArray();
            return result;
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

            Save(attachment, source);
        }

        [Test]
        public void CanRewriteProperties()
        {
            var attachment = new Attachment
            {
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
        public void CanFindByUid() {
            var attachment1 = new Attachment { Uid = "simpleuid", Name = "tuponame" };
            var attachment2 = new Attachment {Uid = "simpleuid2", Name = "tuponame"};
            Save(attachment1);
            Save(attachment2);
            var query = new Attachment {Uid = "simpleuid"};
            var result = Find(query);
            Assert.AreEqual(1,result.Count());
            var found = result.First();
            Assert.NotNull(found);
            Assert.AreEqual("simpleuid",found.Uid);
            Assert.AreEqual("tuponame",found.Name);
        }

        private void TestFileValidlySaved(Attachment attachment, string name) {
            attachment.Name = name;
            Save(attachment);
            var attachtoFind = new Attachment {Uid = attachment.Uid};
            var testAttach = Find(attachtoFind).FirstOrDefault();
            Assert.NotNull(testAttach);
            Assert.AreEqual(name, testAttach.Name);
        }

        [Test]
        public void CanSaveEmptyFile()
        {
            var attachment = new FormAttachment
            {
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
            using (Stream stream = _mdb.Open(attachment, FileAccess.Write)) {
                _mdb.Save(attachment2);
                using (Stream stream2 = _mdb.Open(attachment2, FileAccess.Write)) {
                    stream2.Write(source, 0, source.Length);
                    stream2.Flush();
                }


                stream.Write(source, 0, source.Length);
                stream.Flush();
            }
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
    }
}
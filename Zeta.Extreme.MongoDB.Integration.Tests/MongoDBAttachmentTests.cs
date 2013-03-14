using System;
using System.IO;
using NUnit.Framework;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.MongoDB.Integration.Tests {
    [TestFixture]
    public class MongoDbAttachmentTests {
        private readonly MongoDBAttachmentsSource _mdb;

        /// <summary>
        ///     Constructor
        /// </summary>
        public MongoDbAttachmentTests() {
            _mdb = new MongoDBAttachmentsSource();
        }

        private void Save(byte[] source, FormAttachment attachment) {
            _mdb.Save(attachment);
            using (var stream = _mdb.Open(attachment, FileAccess.Write)) {
                stream.Write(source, 0, source.Length);
                stream.Flush();
            }
        }

        private void Delete(FormAttachment attachment) {
            _mdb.Delete(attachment);
        }

        private void Find(Attachment attachment) {
            var t = _mdb.Find(attachment);


            foreach (var item in t) {
                Console.WriteLine("Uid : {0}", item.Uid);
            }

            Console.WriteLine("---");
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

            Save(source, attachment);
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

            Save(source, attachment);
            Save(source, attachment2);
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
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.MongoDB.Integration.Tests {
    public class MongoDBAttachmentTests {
        private MongoDBAttachmentsSource mdb;

        /// <summary>
        /// Constructor
        /// </summary>
        public MongoDBAttachmentTests() {
            this.mdb = new MongoDBAttachmentsSource();
        }

        private void Save(byte[] source, FormAttachment attachment) {
            this.mdb.Save(attachment);
            using (var stream = this.mdb.Open(attachment, FileAccess.Write)) {
                stream.Write(source, 0, source.Length);
                stream.Flush();
            }
        }

        private void Delete(FormAttachment attachment) {
            this.mdb.Delete(attachment);
        }

       

        public void CanSave() {
            var source = new byte[] { 84, 101, 115, 116, 32, 79, 75, 33 };
            var attachment = new FormAttachment {
                Uid = "Test_OK2",
                Extension = "sda",
                MimeType = "dada",
                User = "remalloc",
                Comment = "test",
                Revision = 0123456789,
                Name = "Test OK File",
                Type = "mdb-test",
                Metadata = {
                    {
                        "s", "sdffsdf"
                    },
                    {
                        "test", "ok"
                    }
                }
            };

            this.Save(source, attachment);
        }

        public void CanDoubleSave() {
            var source = new byte[] { 84, 101, 115, 116, 32, 79, 75, 33 };
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

            this.Save(source, attachment);
            this.Save(source, attachment2);
        }

        public void CanSaveIntoSave() {
            var source = new byte[] { 84, 101, 115, 116, 32, 79, 75, 33 };
            var attachment = new FormAttachment {
                Uid = "Test_OK2",
                Extension = "sda",
                MimeType = "dada",
                User = "remalloc",
                Comment = "test",
                Revision = 0123456789,
                Name = "Test OK File",
                Type = "mdb-test",
                Metadata = {
                    {
                        "s", "sdffsdf"
                    },
                    {
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
                Revision = 0123456789,
                Name = "Test OK File",
                Type = "mdb-test",
                Metadata = {
                    {
                        "s", "sdffsdf"
                    },
                    {
                        "test", "ok"
                    }
                }
            };

            this.mdb.Save(attachment);
            using (var stream = this.mdb.Open(attachment, FileAccess.Write)) {

                this.mdb.Save(attachment2);
                using (var stream2 = this.mdb.Open(attachment2, FileAccess.Write)) {
                    stream2.Write(source, 0, source.Length);
                    stream2.Flush();
                }


                stream.Write(source, 0, source.Length);
                stream.Flush();
            }
        }

        public void CanDelete() {
            var attachment = new FormAttachment {
                Uid = "Test_OK2",
                Name = "Test OK File",
                Type = "mdb-test"
            };

            this.Delete(attachment);
        }


        public void CanFind() {
            var attachment = new FormAttachment {                           // Test attachment description
                Uid = "Test_OK",
                Name = "Test OK File",
                Type = "mdb-test"
            };

            this.mdb.Find(attachment);
        }
    }
}

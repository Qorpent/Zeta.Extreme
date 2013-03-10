using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using System.Threading.Tasks;
using Zeta.Extreme.BizProcess.Forms;
using Zeta.Extreme.Form.MongoDBAttachmentSource;

namespace Zeta.Extreme.Form.Tests {
    public class MongoDBStorageTest {
        private MongoDBAttachmentStorage mdb = new MongoDBAttachmentStorage();


        private void SaveView(Attachment attachment) {
            this.mdb.Save(attachment);
        }

        private void SaveBinary(Attachment attachment) {
            var source = new byte[] { 84, 101, 115, 116, 32, 79, 75, 33 };  // «Test OK!» phrase in ASCII

                                                                            // open a stream to write data
            using (var s = this.mdb.Open(attachment, FileAccess.Write)) {
                s.Write(source, 0, source.Length);                          // writing...
                s.Seek(0, SeekOrigin.Begin);                                // and move to the begin of data stream
                this.mdb.BinaryCommit(attachment);                          // and commit new data
            }
        }

        public void CanSave() {
            // Test attachment description
            var attachment = new FormAttachment {
                Uid = "Test_OK",
                Name = "Test OK File",
                Type = "mdb-test"
            };

            this.SaveView(attachment);
            this.SaveBinary(attachment);

        }

        public void CanFindAttachment() {
            var attachment = new FormAttachment {                           // Test attachment description
                Uid = "Test_OK",
                Name = "Test OK File",
                Type = "mdb-test"
            };

            this.mdb.Find(attachment);
        }

        public void CanDelete() {
            var attachment = new FormAttachment {                           // Test attachment description
                Uid = "Test_OK",
                Name = "Test OK File",
                Type = "mdb-test"
            };

            this.mdb.Delete(attachment);
        }

    }
}

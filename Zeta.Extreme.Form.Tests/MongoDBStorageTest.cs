﻿using System;
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

        public void CanSave() {
            var source = new byte[] { 84, 101, 115, 116, 32, 79, 75, 33 };
            var attachment = new FormAttachment {
                Uid = "Test_OK2",
                Name = "Test OK File",
                Type = "mdb-test"
            };

            using (var stream = this.mdb.Open(attachment, FileAccess.Write)) {
                stream.Write(source, 0, source.Length);
                this.mdb.Save(attachment);
            }
        }

        public void CanDelete() {
            var attachment = new FormAttachment {
                Uid = "Test_OK2",
                Name = "Test OK File",
                Type = "mdb-test"
            };

            this.mdb.Delete(attachment);
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

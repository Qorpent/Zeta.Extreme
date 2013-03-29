using System;
using System.IO;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using NUnit.Framework;
using Qorpent.Applications;
using Qorpent.Data;
using Qorpent.Data.Connections;
using Qorpent.IoC;
using Zeta.Extreme.BizProcess.Forms;
using Zeta.Extreme.Form.DbfsAttachmentSource;
using Zeta.Extreme.MongoDB.Integration;

namespace DbfsToMongo.Connector.Tests
{
    class DbfsToMongoConnectorTests
    {
        private readonly DbfsToMongoConnector _connector;

        protected MongoDatabase _db;
        /// <summary>
        /// После P13-58 - коллекция для дескрипторов
        /// </summary>
        protected MongoCollection<BsonDocument> _filecollection;
        /// <summary>
        /// После P13-58 - коллекция для чанков
        /// </summary>
        protected MongoCollection<BsonDocument> _blobcollection;
        /// <summary>
        /// После P13-58 - коллекция для индекса
        /// </summary>

        protected MongoCollection<BsonDocument> _indexcollection;

        public DbfsToMongoConnectorTests()
        {
            _connector = new DbfsToMongoConnector();
        }

        [SetUp]
        public void setup()
        {
            _db = new MongoClient().GetServer().GetDatabase(_connector.MongoDb.Database);
            // По идее MondoDbAS должна использовать имя коллекции как базис для GridFS
            _filecollection = _db.GetCollection<BsonDocument>(_connector.MongoDb.Collection + ".files");
            _blobcollection = _db.GetCollection<BsonDocument>(_connector.MongoDb.Collection + ".chunks");
            _indexcollection = _db.GetCollection<BsonDocument>("system.indexes");
            _filecollection.Drop();
            _blobcollection.Drop();
            //NOTICE: we haven't drop SYSTEM.INDEXES - MongoDB prevent it!!!!

            // Это выражает простую мысль - при работе компоненты должны существовать только эти коллекции
            _filecollection = _db.GetCollection<BsonDocument>(_connector.MongoDb.Collection + ".files");
            _blobcollection = _db.GetCollection<BsonDocument>(_connector.MongoDb.Collection + ".chunks");
            _indexcollection = _db.GetCollection<BsonDocument>("system.indexes");
        }

        /*
* M O N G O D B N A T I V E T E S T S
* */

        [Test]
        public void MongoCanSaveAttachmentToTarget()
        {
            Attachment attachment = DbfsToMongoConnectorTestsBase.GetNewAttach();

            _connector.MongoDb.Save(attachment);

            var found = _connector.MongoDb.Find(attachment);
            Assert.NotNull(found);
        }

        [Test]
        public void MongoCanFind()
        {
            var attachment = DbfsToMongoConnectorTestsBase.GetNewAttach();

            _connector.MongoDb.Save(attachment);
            using (var stream = _connector.MongoDb.Open(attachment, FileAccess.Write))
            {
                stream.Flush();
            }

            var found = _connector.MongoDb.Find(attachment).FirstOrDefault();

            Assert.NotNull(found);
            Assert.AreEqual(attachment.Uid, found.Uid);
        }

        [Test]
        public void MongoCanSaveAndThenDelete()
        {
            var attachment = DbfsToMongoConnectorTestsBase.GetNewAttach();

            _connector.MongoDb.Save(attachment);
            using (var stream = _connector.MongoDb.Open(attachment, FileAccess.Write))
            {
                stream.Flush();
            }


            var found = _connector.MongoDb.Find(attachment).FirstOrDefault();

            Assert.NotNull(found);
            Assert.AreEqual(attachment.Uid, found.Uid);

            _connector.MongoDb.Delete(attachment);

            var not_found = _connector.MongoDb.Find(attachment).FirstOrDefault();
            Assert.IsNull(not_found);

        }

        [Test]
        public void MongoCanDownload()
        {
            var attachment = DbfsToMongoConnectorTestsBase.GetNewAttach();
            byte[] someData = { 1, 2, 3, 4, 5 };
            byte[] someBuffer = { 0, 0, 0, 0, 0 };

            _connector.MongoDb.Save(attachment);
            using (var stream = _connector.MongoDb.Open(attachment, FileAccess.Write))
            {
                stream.Write(someData, 0, someData.Length);
                stream.Flush();
            }


            var found = _connector.MongoDb.Find(attachment).FirstOrDefault();

            Assert.NotNull(found);
            Assert.AreEqual(attachment.Uid, found.Uid);
            Assert.AreNotEqual(someData, someBuffer);

            using (var stream = _connector.MongoDb.Open(attachment, FileAccess.Read))
            {

                stream.Read(someBuffer, 0, someBuffer.Length);
                stream.Flush();
            }

            Assert.AreEqual(someBuffer, someData);

        }

        [Test]
        public void MongoCanCreateBinFileAndGetUid()
        {
            var attachment = new Attachment();

            _connector.MongoDb.Save(attachment);

            Assert.IsNotNull(attachment.Uid);
        }

        [Test]
        public void MongoCanSaveAnAttachment()
        {
            var attachment = DbfsToMongoConnectorTestsBase.GetNewAttach();

            _connector.MongoDb.Save(attachment);
            using (var stream = _connector.MongoDb.Open(attachment, FileAccess.Write))
            {
                stream.Flush();
            }
        }



        /* D B F S N A T I V E T E S T S */
        [Test]
        public void DbfsCanReadAttachments()
        {
            var attachments = _connector.Dbfs.Find(
                new FormAttachment
                {
                    TemplateCode = "balans2011A.in",
                    Year = 2012,
                    Period = 13,
                    ObjId = 352
                }
            ).ToArray();

            Assert.AreEqual(2, attachments.Length);
        }

        [Test]
        public void DbfsCanReadBinary()
        {
            var attachment = _connector.Dbfs.Find(
                new FormAttachment
                {
                    TemplateCode = "balansA.in",
                    Year = 2010,
                    Period = 16,
                    ObjId = 538
                }
            ).First();

            var ms = new MemoryStream();
            using (var s = _connector.Dbfs.Open(attachment, FileAccess.Read))
            {
                s.CopyTo(ms);
            }

            Assert.AreEqual(ms.Length, attachment.Size);
        }

        [Test]
        public void DbfsCanWriteAttachments()
        {
            var attachment = new FormAttachment
            {
                Uid = Guid.NewGuid().ToString(),
                Type = "dbfs-test"
            };

            _connector.Dbfs.Save(attachment);

            var testattachment = _connector.Dbfs.Find(
                new Attachment
                {
                    Uid = attachment.Uid
                }
            ).First();

            Assert.AreEqual(testattachment.Type, attachment.Type);

        }

        [Test]
        public void DbfsCanWriteBinary()
        {
            var attachment = new FormAttachment{
                Uid = Guid.NewGuid().ToString(),
                Type = "dbfs-test"
            };
            _connector.Dbfs.Save(attachment);
            var data = new byte[] { 1, 2, 3, 4, 5 };
            
            using (
                var s = _connector.Dbfs.Open(attachment, FileAccess.Write)
            ) {
                s.Write(data, 0, 5);
                s.Flush();
            }

            var testattachment = _connector.Dbfs.Find(
                new Attachment {
                    Uid = attachment.Uid
                }
            ).First();

            Assert.AreEqual(5, testattachment.Size);

            var ms = new MemoryStream();
            using (var s = _connector.Dbfs.Open(attachment, FileAccess.Read)) {
                s.CopyTo(ms);
            }

            Assert.AreEqual(ms.Length, testattachment.Size);
            Assert.AreEqual(1, ms.GetBuffer()[0]);
            Assert.AreEqual(5, ms.GetBuffer()[4]);

        }
    }
}
﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Zeta.Extreme.BizProcess.Forms;

using MongoDB.Driver;
using MongoDB.Bson;

namespace Zeta.Extreme.MongoDB.Integration.Tests {

    [TestFixture]
    public class MongoDBAttachmentSourceTests {
        private readonly MongoDbAttachmentSource _mdb = new MongoDbAttachmentSource();

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
        
        [SetUp]
        public virtual void Setup()
        {
            _db = new MongoClient().GetServer().GetDatabase(_mdb.Database);
            // По идее MondoDbAS должна использовать имя коллекции как базис для GridFS
            _filecollection = _db.GetCollection<BsonDocument>(_mdb.Collection + ".files");
            _blobcollection = _db.GetCollection<BsonDocument>(_mdb.Collection + ".chunks");
            _indexcollection = _db.GetCollection<BsonDocument>("system.indexes");
            _filecollection.Drop();
            _blobcollection.Drop();
            //NOTICE: we haven't drop SYSTEM.INDEXES - MongoDB prevent it!!!!

            // Это выражает простую мысль - при работе компоненты должны существовать только эти коллекции
            _filecollection = _db.GetCollection<BsonDocument>(_mdb.Collection + ".files");
            _blobcollection = _db.GetCollection<BsonDocument>(_mdb.Collection + ".chunks");
            _indexcollection = _db.GetCollection<BsonDocument>("system.indexes");

        }

        [Test]
        public void CanAttachmentToBsonAndBack()
        {
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
        public void CanAttachmentToBsonForFind()
        {
            var attachment = GetNewAttach();

            var inBson = MongoDbAttachmentSourceSerializer.AttachmentToBsonForFind(attachment);

            Assert.AreSame(attachment.Uid, inBson["_id"].ToString());
            Assert.AreSame(attachment.Name, inBson["filename"].ToString()); // Name
            Assert.AreSame(attachment.Comment, inBson["comment"].ToString()); // Comment
            Assert.AreSame(attachment.User, inBson["owner"].ToString()); // User
            Assert.AreNotEqual(attachment.Version, inBson["uploadDate"].ToLocalTime()); // Version
            Assert.AreSame(attachment.MimeType, inBson["contentType"].ToString()); // MimeType
            Assert.AreEqual(attachment.Revision, inBson["revision"].ToInt32()); // Revision
        }


        [Test]
        public void CanFind() {
            var attachment = GetNewAttach();

            _mdb.Save(attachment);
            using (var stream = _mdb.Open(attachment, FileAccess.Write)) {
                stream.Flush();
            }
            
            var found = _mdb.Find(attachment).FirstOrDefault();
            
            Assert.NotNull(found);
            Assert.AreEqual(attachment.Uid, found.Uid);
        }

        [Test]
        public void CanSaveAndThenDelete() {
            var attachment = GetNewAttach();

            _mdb.Save(attachment);
            using(var stream = _mdb.Open(attachment, FileAccess.Write))
            {
                stream.Flush();
            }
            

            var found = _mdb.Find(attachment).FirstOrDefault();

            Assert.NotNull(found);
            Assert.AreEqual(attachment.Uid, found.Uid);

            _mdb.Delete(attachment);

            var not_found = _mdb.Find(attachment).FirstOrDefault();
            Assert.IsNull(not_found);

        }

        [Test]
        public void CanDownload() {
            var attachment = GetNewAttach();
            byte[] someData = {1, 2, 3, 4, 5};
            byte[] someBuffer = {0, 0, 0, 0, 0};

            _mdb.Save(attachment);
            using(var stream = _mdb.Open(attachment, FileAccess.Write))
            {
                stream.Write(someData, 0, someData.Length);
                stream.Flush();
            }
           

            var found = _mdb.Find(attachment).FirstOrDefault();

            Assert.NotNull(found);
            Assert.AreEqual(attachment.Uid, found.Uid);
            Assert.AreNotEqual(someData, someBuffer);

            using (var stream = _mdb.Open(attachment, FileAccess.Read))  {

                stream.Read(someBuffer, 0, someBuffer.Length);
                stream.Flush();
            }

            Assert.AreEqual(someBuffer, someData);

        }

        [Test]
        public void CanUpdateBinaryAndDescription() {
            var attachment = GetNewAttach();

            byte[] someData2 = { 1, 2, 3, 4, 5 };
            byte[] someData = { 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, };
            
            _mdb.Save(attachment);
            using (var stream = _mdb.Open(attachment, FileAccess.Write))
            {
                stream.Write(someData, 0, someData.Length);
                stream.Flush();
            }


            var found = _mdb.Find(attachment).FirstOrDefault();

            Assert.NotNull(found);
            Assert.AreEqual(attachment.Uid, found.Uid);


            using (var stream = _mdb.Open(attachment, FileAccess.Write))
            {
                stream.Write(someData2, 0, someData2.Length);
                stream.Flush();
            }

            var found2 = _mdb.Find(attachment).FirstOrDefault();
            Assert.NotNull(found2);
            Assert.AreEqual(attachment.Uid, found2.Uid);

            Assert.AreEqual(someData2.Length, found2.Size);
        }




        public Attachment GetNewAttach(string uid = null) {
            return new Attachment {
                Uid = ObjectId.GenerateNewId().ToString(),
                Name =  "Name",
                Comment = "Comment",
                Revision = 115,
                Version = new DateTime(1, 1, 1, 1, 1, 1, 1),
                User = "User",
                MimeType = "MimeType",
                Extension = "Extension",
                Metadata = {
                    {
                        "m1", "v1"
                    }, {
                        "m2", "v2"
                    }
                }
            };
        }

        [Test]
        public void CanCreateBinFileAndGetUid() {
            var attachment = new Attachment();

            _mdb.Save(attachment);

            Assert.IsNotNull(attachment.Uid);
        }

        [Test]
        public void CanSaveAnAttachment() {
            var attachment = GetNewAttach();

            _mdb.Save(attachment);
            using (var stream = _mdb.Open(attachment, FileAccess.Write)) {
                stream.Flush();
            }
        }
    }
}
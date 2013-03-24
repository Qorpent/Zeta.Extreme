using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using System.Collections;
using MongoDB.Bson;
using MongoDB.Driver;
using NUnit.Framework;
using DbfsToMongo.Wrapper;
using DbfsToMongo.Connector;
using Zeta.Extreme.BizProcess.Forms;

namespace DbfsToMongoWrapperTests {
    public class DbfsToMongoWrapperTests {
        private DbfsToMongoWrapper _wrapper;
        private DbfsToMongoConnector _connector;

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
        public void setup() {
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


        public DbfsToMongoWrapperTests() {
            _wrapper = new DbfsToMongoWrapper();
            _connector = new DbfsToMongoConnector();
        }



        [Test]
        public void CanCorrectUpdateAttachments() {
            var attachment = _wrapper.MigrateNextAttachmentToMongo();

            // Проверим, что мы можем найти то, что вставили.
            var foundFromMongoFirst = _wrapper.connector.MongoDb.Find(new Attachment {
                Uid = attachment.Uid
            }).FirstOrDefault();
            Assert.IsNotNull(foundFromMongoFirst);
            // ОкейЮ, проверили

            using (var stream = _wrapper.connector.MongoDb.Open(new Attachment {
                Uid = attachment.Uid
            }, FileAccess.Write)) {
                byte[] testBytes = { 9, 8, 7, 6, 5, 4, 3, 2, 1 };
                stream.Write(testBytes, 0, testBytes.Length);
                stream.Flush();
            }

            // Проверим, что мы можем найти то, что изменили
            var foundFromMongoSecond = _wrapper.connector.MongoDb.Find(new Attachment {
                Uid = attachment.Uid
            }).FirstOrDefault();
            Assert.IsNotNull(foundFromMongoSecond);
            // Окей, проверили


            // проверим, что мы имеем разлчие в длине бинариников, которые загрузили
            Assert.AreNotEqual(foundFromMongoFirst.Size, foundFromMongoSecond.Size);



        }
    }
}

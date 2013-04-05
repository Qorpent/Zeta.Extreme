using System.IO;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using NUnit.Framework;
using Zeta.Extreme.BizProcess.Forms;
using Zeta.Extreme.MongoDB.Integration;

namespace Zeta.Extreme.DbfsToMongo.Wrapper.Tests
{
    public class DbfsToMongoWrapperTests {
        private DbfsToMongoWrapper _wrapper;

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

        protected MongoDbAttachmentSource _mdb;
        protected MongoCollection<BsonDocument> _indexcollection;

        [SetUp]
        public void setup() {
            _mdb = new MongoDbAttachmentSource {
                Database = "Att"
            };
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
        public void CanCorrectUpdateAttachments() {
            _wrapper = new DbfsToMongoWrapper();

            var attachment = _wrapper.MigrateNextAttachmentToMongo();


            // Проверим, что мы можем найти то, что вставили.
            var foundFromMongoFirst = _wrapper.Find(new Attachment
            {
                Uid = attachment.Uid
            }).FirstOrDefault();
            Assert.IsNotNull(foundFromMongoFirst);
            // ОкейЮ, проверили

            using (
                var stream = _wrapper.Open(
                    new Attachment {
                        Uid = attachment.Uid
                    },
                    FileAccess.Write
                )
            ) {
                byte[] testBytes = { 9, 8, 7, 6, 5, 4, 3, 2, 1 };
                stream.Write(testBytes, 0, testBytes.Length);
                stream.Flush();
            }

            // Проверим, что мы можем найти то, что изменили
            var foundFromMongoSecond = _wrapper.Find(
                new Attachment {
                    Uid = attachment.Uid
                }
            ).FirstOrDefault();
            Assert.IsNotNull(foundFromMongoSecond);
            // Окей, проверили


            // проверим, что мы имеем разлчие в длине бинариников, которые загрузили
            Assert.AreNotEqual(foundFromMongoFirst.Size, foundFromMongoSecond.Size);
            


        }
    }
}
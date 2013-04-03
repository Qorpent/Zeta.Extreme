using MongoDB.Driver;
using NUnit.Framework;
using Qorpent.Log;

namespace Zeta.Extreme.MongoDB.Integration.Tests {
    class MongoDbLogsTests : MongoDbLogsTestsBase {
        
        [Test]
        public void IsValidSerializing() {
            var logMessageOrig = GetNewLogInstance();
            var document = MongoDbLogsSerializer.LogMessageToBsonDocument(logMessageOrig);

            _mongoDbLogs.Write(logMessageOrig);

            Assert.AreEqual(logMessageOrig.Name, document["name"].ToString());
            Assert.AreEqual(logMessageOrig.Level, (LogLevel)document["level"].ToInt32());
            Assert.AreEqual(logMessageOrig.Code, document["code"].ToString());
            Assert.AreEqual(logMessageOrig.Message, document["message"].ToString());

            Assert.AreEqual(logMessageOrig.Server, document["server"].ToString());
            Assert.AreEqual(logMessageOrig.ApplicationName, document["applicationName"].ToString());
        }

        [Test]
        public void CanWriteLogs() {
            var logMessageOrig = GetNewLogInstance();

            for (int i = 0; i < WRITE_COMMITS_COUNT; i++) {
                _mongoDbLogs.Write(logMessageOrig);
            }

            var mongoClient = new MongoClient(ConnectionString);
            var mongoServer = mongoClient.GetServer();
            var mongoDatabase = mongoServer.GetDatabase(DatabaseName, new MongoDatabaseSettings());
            var collections = mongoDatabase.GetCollectionNames();


            // Проверим, что лог писался каждый раз и писался корректно
            Assert.AreEqual(mongoDatabase.GetCollection(LogsCollectionName).Count(), WRITE_COMMITS_COUNT);

            // проверим, что наши коллекции существуют
            Assert.IsTrue(mongoDatabase.CollectionExists(LogsCollectionName));
            Assert.IsTrue(mongoDatabase.CollectionExists(LogsCollectionName + ".users"));
            Assert.IsTrue(mongoDatabase.CollectionExists(LogsCollectionName + ".forms"));
            Assert.IsTrue(mongoDatabase.CollectionExists(LogsCollectionName + ".servers"));
            Assert.IsTrue(mongoDatabase.CollectionExists(LogsCollectionName + ".companies"));
            Assert.IsTrue(mongoDatabase.CollectionExists(LogsCollectionName + ".periods"));

            foreach (var collection in collections) {
                if (collection.StartsWith(LogsCollectionName + ".")) {
                    // проверим, что всё писалось корректно
                    Assert.AreEqual(mongoDatabase.GetCollection(collection).Count(), 1);
                }
            }

            // ПРОХОД ЗАПИСИ №2

            var logMessageOrigNext = GetNewLogInstance();
            for (int i = 0; i < WRITE_COMMITS_COUNT; i++) {
                _mongoDbLogs.Write(logMessageOrigNext);
            }

            // Проверим, что лог писался каждый раз и писался корректно
            Assert.AreEqual(mongoDatabase.GetCollection(LogsCollectionName).Count(), (WRITE_COMMITS_COUNT * 2));
        }
    }
}
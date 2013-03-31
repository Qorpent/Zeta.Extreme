using MongoDB.Driver;
using NUnit.Framework;

namespace Zeta.Extreme.MongoDB.Integration.Tests {
    class MongoDbLogsTests : MongoDbLogsTestsBase {
        
        [Test]
        public void IsValidSerializing() {
            var logMessageOrig = GetNewLogInstance();
            var document = MongoDbLogsSerializer.LogMessageToBson(logMessageOrig);
            var logMessageSerialized = MongoDbLogsSerializer.BsonToLogMessage(document);

            Assert.AreEqual(logMessageOrig.Name, logMessageSerialized.Name);
            Assert.AreEqual(logMessageOrig.Level, logMessageSerialized.Level);
            Assert.AreEqual(logMessageOrig.Code, logMessageSerialized.Code);
            Assert.AreEqual(logMessageOrig.Message, logMessageSerialized.Message);
        }

        [Test]
        public void CanWriteLogsWithPartitioning() {
            var logMessageOrig = GetNewLogInstance();

            for (int i = 0; i < 27; i++) {
                _mongoDbLogs.Write(logMessageOrig);
            }

            var mongoClient = new MongoClient(ConnectionString);
            var mongoServer = mongoClient.GetServer();
            var mongoDatabase = mongoServer.GetDatabase(DatabaseName, new MongoDatabaseSettings());
            var collections = mongoDatabase.GetCollectionNames();

            foreach (var collection in collections) {
                if (collection.StartsWith(LogsCollectionName + ".")) {
                    Assert.AreEqual(
                        mongoDatabase.GetCollection(collection).Count(),
                        (
                            (27 / MongoDbLayoutSpecification.MONGODBLOGS_STAT_PARTITION_COUNT)
                                +
                                (((27 % MongoDbLayoutSpecification.MONGODBLOGS_STAT_PARTITION_COUNT) != 0) ? (1) : (0))
                        )
                    );
                }
            }
        }
    }
}
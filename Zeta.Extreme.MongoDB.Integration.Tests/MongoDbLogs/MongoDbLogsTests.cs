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
        public void CanWriteLog() {
            var logMessageOrig = GetNewLogInstance();

            for (int i = 0; i < 27; i++ ) _mongoDbLogs.Write(logMessageOrig);
        }
    }
}
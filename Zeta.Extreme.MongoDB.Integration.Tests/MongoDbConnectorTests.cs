using MongoDB.Bson;
using NUnit.Framework;
using MongoDB.Driver;

namespace Zeta.Extreme.MongoDB.Integration.Tests {
    class MongoDbConnectorTests : MongoDbConnectorTestsBase {
        [Test]
        public void CanConnectToMongoDb() {
            // ensure that classes was created correct, but server still disconnected
            Assert.AreEqual(MongoServerState.Disconnected, Connector.Server.State);

            // do something (RW operation) to (real) connect to the server
            Connector.Collection.Save(
                new BsonDocument {
                    {"testOk", true}
                }
            );

            Assert.AreEqual(MongoServerState.Connected, Connector.Server.State);
        }
    }
}

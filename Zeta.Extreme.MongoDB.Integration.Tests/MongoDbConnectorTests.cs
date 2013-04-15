using NUnit.Framework;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Zeta.Extreme.MongoDB.Integration.Tests {
    class MongoDbConnectorTests : MongoDbConnectorTestsBase {
        [Test]
        public void CanConnectToMongoDb() {
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

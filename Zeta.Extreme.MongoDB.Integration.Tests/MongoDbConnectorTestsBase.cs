using NUnit.Framework;

namespace Zeta.Extreme.MongoDB.Integration.Tests {
    class MongoDbConnectorTestsBase {
        protected MongoDbConnector Connector;

        [TestFixtureSetUp]
        public void FixtureSetUp() {
            Connector = new MongoDbConnector();
            Connector.Collection.RemoveAll();
        }
    }
}
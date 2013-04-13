using NUnit.Framework;

namespace Zeta.Extreme.MongoDB.Integration.Tests {
    class MongoDbConnectorTestsBase {
        protected const string DEFAULT_DB_NAME = "MongoDbConnectorTest";

        protected MongoDbConnector Connector;

        [TestFixtureSetUp]
        public void FixtureSetUp() {
            Connector = new MongoDbConnector();
        }
    }
}
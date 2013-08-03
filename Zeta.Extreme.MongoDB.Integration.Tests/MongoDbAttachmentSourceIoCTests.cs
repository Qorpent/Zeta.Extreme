using NUnit.Framework;
using Qorpent.IoC;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.MongoDB.Integration.Tests {
    [TestFixture]
    public class MongoDbAttachmentSourceIoCTests {
        private IContainer container;

        [Test]
        public void CanSetupAttachmentSourceFromContainer() {
        
            RegisterMongoAttachmentSource();

            var result = container.Get<IAttachmentStorage>("attachment.source");
            Assert.NotNull(result);
            Assert.IsInstanceOf<MongoDbAttachmentSource>(result);
            var myresult = (MongoDbAttachmentSource) result;
            Assert.AreEqual("mongodb://localhost", myresult.ConnectionString);
            Assert.AreEqual("testdatabase", myresult.Database.Name);
            Assert.AreEqual("testcollection", myresult.Collection.Name);
        }
        [SetUp]
        public void Setup() {
            container = ContainerFactory.CreateDefault();
        }

        private void RegisterMongoAttachmentSource() {
            var component = container.EmptyComponent();
            component.Name = "attachment.source";
            component.ServiceType = typeof (IAttachmentStorage);
            component.ImplementationType = typeof (MongoDbAttachmentSource);
            component.Lifestyle = Lifestyle.Transient;
            component.Parameters["ConnectionString"] = "mongodb://localhost";
            component.Parameters["DatabaseName"] = "testdatabase";
            component.Parameters["CollectionName"] = "testcollection";
            container.Register(component);
        }

        private void RegisterFormAttachmentSource()
        {
            var component = container.EmptyComponent();
            component.ServiceType = typeof(IFormAttachmentStorage);
            component.ImplementationType = typeof(FormAttachmentSource);
            component.Lifestyle = Lifestyle.Transient;
            container.Register(component);
        }

        [Test]
        public void FormSourceWillGetMongoSource() {
            RegisterFormAttachmentSource();
            RegisterMongoAttachmentSource();
            var formsource = container.Get<IFormAttachmentStorage>();
            var myformsource = (FormAttachmentSource) formsource;
            Assert.IsInstanceOf<MongoDbAttachmentSource>(myformsource.InternalStorage);
        }
    }
}

using NUnit.Framework;
using Qorpent;
using Qorpent.Applications;
using Qorpent.IoC;

namespace Zeta.Extreme.FrontEnd.Tests {
    class FormServerTests : ServiceBase {
        [Test]
        public void CanCreateFormServerFormContainer() {
            var application = new Application {
                Container = new Container()
            };

            var component = application.Container.EmptyComponent();
            component.ServiceType = typeof(FormServer);
            application.Container.Register(component);

            var formServer = application.Container.Get(typeof(FormServer));
            Assert.IsNotNull(formServer);
        }
    }
}

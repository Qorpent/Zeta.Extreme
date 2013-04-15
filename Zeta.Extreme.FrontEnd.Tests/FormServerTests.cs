using System;
using System.Security.Principal;
using NUnit.Framework;
using Qorpent.Applications;
using Qorpent.IoC;
using Qorpent.Security;
using Zeta.Extreme.BizProcess.Forms;
using Zeta.Extreme.Form.InputTemplates;
using Zeta.Extreme.Model;
using Qorpent;

namespace Zeta.Extreme.FrontEnd.Tests {
    class FormServerTests : ServiceBase {
        private FormServer _formServer;

        [SetUp]
        public void SetUp() {
            _formServer = new FormServer();
        }

        [Test]
        public void CanStartSession() {
            var user = new GenericPrincipal(
                new GenericIdentity(
                    Environment.UserName
                ),
                
                new string[] {}
            );

            Application = new Application {
                Container = new SimpleContainer()
            };


            var component = Application.Container.EmptyComponent();
            component.ServiceType = typeof(IPrincipalSource);
            component.ImplementationType = typeof (DefaultPrincipalSource);
            component.Lifestyle = Lifestyle.Transient;
            Application.Container.Register(component);


            Assert.IsNotNull(Application);
            Assert.IsNotNull(Application.Principal);
            Assert.IsNotNull(Application.Principal.CurrentUser);
            Assert.IsNotNull(Application.Principal.CurrentUser.Identity);
            Assert.IsNotNull(Application.Principal.CurrentUser.Identity.Name);
            
            _formServer.SetApplication(Application);

            var session = _formServer.Start(
                new InputTemplate(),
                new Obj(),
                2012,
                2
            );

            Assert.IsNotNull(session);
        }
    }
}

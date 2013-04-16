using System;
using System.Security.Principal;
using NUnit.Framework;
using Qorpent.Applications;
using Qorpent.IoC;
using Qorpent.Security;
using Zeta.Extreme.BizProcess.Forms;
using Zeta.Extreme.Form.InputTemplates;
using Zeta.Extreme.Form.Themas;
using Zeta.Extreme.Model;
using Qorpent;

namespace Zeta.Extreme.FrontEnd.Tests {
    class FormServerTests : ServiceBase {
        private FormServer _formServer;

        /// <summary>
        /// 
        /// </summary>
        public class FormRowProvider : IFormRowProvider {
            public FormStructureRow[] GetRows(IFormSession session) {
                throw new NotImplementedException();
            }
        }

        [SetUp]
        public void SetUp() {
            _formServer = new FormServer();
        }

        [Test]
        public void CanStartSession() {
            Application = new Application {
                Container = new SimpleContainer()
            };

            var comp1 = Application.Container.EmptyComponent();
            comp1.ServiceType = typeof(IPrincipalSource);
            comp1.ImplementationType = typeof (DefaultPrincipalSource);
            comp1.Lifestyle = Lifestyle.Transient;
            Application.Container.Register(comp1);

            var comp2 = Application.Container.EmptyComponent();
            comp2.Name = "SampleCode.row.preparator";
            comp2.ServiceType = typeof (IFormRowProvider);
            comp2.ImplementationType = typeof (FormRowProvider);
            comp2.Lifestyle = Lifestyle.Transient;
            Application.Container.Register(comp2);

            var obj = new Obj();
            var template = new InputTemplate {
                Thema = new Thema {
                    Code = "SampleCode"
                }
            };

            var sess = new FormSession(template, 2012, 2, obj);

            Assert.IsNotNull(sess);


            Assert.IsNotNull(Application);
            Assert.IsNotNull(Application.Principal);
            Assert.IsNotNull(Application.Principal.CurrentUser);
            Assert.IsNotNull(Application.Principal.CurrentUser.Identity);
            Assert.IsNotNull(Application.Principal.CurrentUser.Identity.Name);
            
            _formServer.SetApplication(Application);
        }

        [Test]
        public void CanCreateFormServerInstance() {
            var formServer = new FormServer();
            Assert.IsNotNull(formServer);
        }

        [Test]
        public void CanSetApplicationToAFormServerInstance() {
            var formServer = new FormServer();
            var application = new Application {
                Container = new SimpleContainer()
            };

            Assert.IsNotNull(formServer);
            Assert.IsNotNull(application);

            formServer.SetApplication(application);
        }

        [Test]
        public void CanWorkWithContainer() {
            var formServer = new FormServer();
            var application = new Application {
                Container = new SimpleContainer()
            };

            Assert.IsNotNull(formServer);
            Assert.IsNotNull(application);

            var comp1 = Application.Container.EmptyComponent();
            comp1.ServiceType = typeof(IPrincipalSource);
            comp1.ImplementationType = typeof(DefaultPrincipalSource);
            comp1.Lifestyle = Lifestyle.Transient;
            Application.Container.Register(comp1);

            var comp2 = Application.Container.EmptyComponent();
            comp2.Name = "SampleCode.row.preparator";
            comp2.ServiceType = typeof(IFormRowProvider);
            comp2.ImplementationType = typeof(FormRowProvider);
            comp2.Lifestyle = Lifestyle.Transient;
            Application.Container.Register(comp2);

            var obj = new Obj();
            var template = new InputTemplate {
                Thema = new Thema {
                    Code = "SampleCode"
                }
            };

            var sess = new FormSession(template, 2012, 2, obj);

            Assert.IsNotNull(sess);
            Assert.IsNotNull(Application);

            formServer.SetApplication(application);
        }
    }
}

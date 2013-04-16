using System;
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
        ///     Перектырый класс для тестовой инициализации
        /// </summary>
        public class FormRowProvider : IFormRowProvider {
            public FormStructureRow[] GetRows(IFormSession session) {
                throw new NotImplementedException();
            }
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

            var comp1 = application.Container.EmptyComponent();
            comp1.ServiceType = typeof(IPrincipalSource);
            comp1.ImplementationType = typeof(DefaultPrincipalSource);
            comp1.Lifestyle = Lifestyle.Transient;
            application.Container.Register(comp1);

            var comp2 = application.Container.EmptyComponent();
            comp2.Name = "SampleCode.row.preparator";
            comp2.ServiceType = typeof(IFormRowProvider);
            comp2.ImplementationType = typeof(FormRowProvider);
            comp2.Lifestyle = Lifestyle.Transient;
            application.Container.Register(comp2);

            var obj = new Obj();
            var template = new InputTemplate {
                Thema = new Thema {
                    Code = "SampleCode"
                }
            };

            var sess = new FormSession(template, 2012, 2, obj);

            Assert.IsNotNull(sess);
            Assert.IsNotNull(application);

            formServer.SetApplication(application);
        }
    }
}

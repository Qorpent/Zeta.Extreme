using System;
using System.Security.Principal;
using NUnit.Framework;
using Qorpent;
using Qorpent.Applications;
using Qorpent.IO;
using Qorpent.IoC;
using Qorpent.Security;
using Zeta.Extreme.BizProcess.Forms;
using Zeta.Extreme.Form.InputTemplates;
using Zeta.Extreme.Form.Themas;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.FrontEnd.Tests
{
    public class StubPrincipalSource : IPrincipalSource
    {
        public IPrincipal CurrentUser { get; set; }
        public IPrincipal BasePrincipal { get; set; }

        public StubPrincipalSource()
        {
            CurrentUser = new GenericPrincipal(
                new GenericIdentity(
                    Environment.UserName
                ),
                new string[] { }
            );
        }

        public void SetCurrentUser(IPrincipal usr)
        {
            CurrentUser = usr;
        }
    }

    public class FormRowProvider : IFormRowProvider
    {
        public FormStructureRow[] GetRows(IFormSession session, IZetaRow contextRow = null, int baseLevel = 0) {
            return new FormStructureRow[1];
        }
    }

    public class TestExtremeFactory : IExtremeFactory
    {
        public ISession CreateSession(SessionSetupInfo setupInfo = null)
        {
            return new Session(true);
        }

        public IQuery CreateQuery(QuerySetupInfo setupInfo = null)
        {
            throw new NotImplementedException();
        }

        public IRowHandler CreateRowHandler()
        {
            throw new NotImplementedException();
        }

        public IColumnHandler CreateColumnHandler()
        {
            throw new NotImplementedException();
        }

        public IObjHandler CreateObjHandler()
        {
            throw new NotImplementedException();
        }

        public ITimeHandler CreateTimeHandler()
        {
            throw new NotImplementedException();
        }

        public IFormulaStorage GetFormulaStorage()
        {
            throw new NotImplementedException();
        }

        public IReferenceHandler CreateReference()
        {
            throw new NotImplementedException();
        }
    }

    class FormServerTests : ServiceBase
    {
        public Application app;

        [SetUp]
        public void Setup()
        {
            app = new Application
            {
                Container = new Container()
            };

            app.Container.Register(new ComponentDefinition<IFormServer, FormServer>());
            app.Container.Register(new ComponentDefinition<IFormSession, FormSession>());
            app.Container.Register(new ComponentDefinition<IPrincipalSource, StubPrincipalSource>());
            app.Container.Register(new ComponentDefinition<IExtremeFactory, TestExtremeFactory>());
            app.Container.Register(new ComponentDefinition<IFileService, FileService>());
            app.Container.Register(new ComponentDefinition<IRoleResolver, DefaultRoleResolver>());
            app.Container.Register(new ComponentDefinition<IFormRowProvider, FormRowProvider>(Lifestyle.Default, "test.row.preparator"));
			
	        
        }

        [Test]
        public void CanCreateFormServerFormContainer()
        {
            var formServer = app.Container.Get<IFormServer>();
            Assert.IsNotNull(formServer);
        }

        [Test]
        public void CanCreateFormSessionFromContainer()
        {
            var formSession = app.Container.Get<IFormSession>(
                null,
                new object[] {
                    new InputTemplate(),
                    2013,
                    2,
                    new Obj(),
					null
                }
            );

            Assert.IsNotNull(formSession);
        }

        [Test]
        public void CanFormServerExecuteFromContainer()
        {
            var formServer = app.Container.Get<IFormServer>();
            Assert.IsNotNull(formServer);

            formServer.Execute(app);
        }

        [Test]
        public void FormServerCanCreateFormSessionFromContainer()
        {
            var formServer = app.Container.Get<IFormServer>();
            Assert.IsNotNull(formServer);

            formServer.Execute(app);

            var formSession = ((FormServer)formServer).CreateSession(
                new InputTemplate {
                    Thema = new Thema {
                        Code = "test"
                    }
                },
                new Obj(), 2012, 2,null
            );

            Assert.IsNotNull(formSession);
        }

		
    }
}

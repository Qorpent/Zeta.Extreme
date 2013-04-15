using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Qorpent;
using Qorpent.Applications;
using Qorpent.Events;
using Qorpent.IO;
using Qorpent.IoC;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.BizProcess.Forms;
using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.Form.InputTemplates;
using Zeta.Extreme.Form.SaveSupport;
using Zeta.Extreme.Form.Themas;
using Zeta.Extreme.FrontEnd.Actions;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.MetaCaches;
using Zeta.Extreme.Model.Querying;
using Zeta.Extreme.Model.SqlSupport;

using Zeta.Extreme.MongoDB.Integration;
using Zeta.Extreme.Form;
namespace Zeta.Extreme.FrontEnd.Tests {
    class FormServerTests : ServiceBase {
        private MongoDbAttachmentSource obj;
        private Container _container;
        private ComponentDefinition<IAttachmentStorage, MongoDbAttachmentSource> component;
        private IApplication a;

        [Test]
        public void CanCreateFormServer() {
            _container = new Container();
            var component = _container.EmptyComponent();
            component.Name = "attachment.source";
            component.ServiceType = typeof(IAttachmentStorage);
            component.ImplementationType = typeof(MongoDbAttachmentSource);
            component.Lifestyle = Lifestyle.Transient;
            component.Parameters["ConnectionString"] = "testconnection";
            component.Parameters["Database"] = "testdatabase";
            component.Parameters["Collection"] = "testcollection";
            _container.Register(component);

            a = new Application { Container = _container };
            var t = a.Files.Resolve("~/.tmp/dgfdg.dll");
            Assert.IsNotNull(t);

        }
    }
}

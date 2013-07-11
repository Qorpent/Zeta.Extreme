using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Qorpent.Applications;
using Qorpent.IoC;
using Qorpent.Security;
using Qorpent.Wiki;
using Qorpent;
using Zeta.Extreme.MongoDB.Integration.WikiStorage;

namespace Zeta.Extreme.MongoDB.Integration.Tests.Wiki {
    public class StubIdentity : IIdentity {
        public string Name { get; set; }
        public string AuthenticationType { get; set; }
        public bool IsAuthenticated { get; set; }
    }

    public class StubClaimsPrincipal : IPrincipal {
        public bool IsInRole(string role) {
            throw new NotImplementedException();
        }

        public IIdentity Identity { get; set; }
    }

    public class StubPrincipalSource : IPrincipalSource {
        public IPrincipal CurrentUser { get; private set; }
        public IPrincipal BasePrincipal { get; set; }
        public void SetCurrentUser(IPrincipal usr) {
            CurrentUser = usr;
        }
    }

    class WikiTests : ServiceBase {
        private IApplication app;

        [SetUp]
        public void SetUp() {
            var storage = new MongoWikiPersister {
               DatabaseName = "Zefs"
            };

            storage.Database.Drop();
        }

        [Test]
        public void CanSaveWikiPage() {
           var wikiPage = new WikiPage {
               Code = "test",
               Editor = "remalloc",
               Existed = true,
               LastWriteTime = DateTime.Now,
               Owner = "remalloc",
               Text = "some text",
               Title = "fgfgdfgd"
           };

           var storage = new MongoWikiPersister {
               DatabaseName = "Zefs",
               CollectionName = "main"
           };

           storage.Save(wikiPage);
           storage.Get("test");
        }

        public void GetLockTask(bool isFirst) {
            app.Principal.SetCurrentUser(new StubClaimsPrincipal { Identity = new StubIdentity { Name = Guid.NewGuid().ToString() } });
            var storage = new MongoWikiPersister {
                DatabaseName = "Zefs",
                CollectionName = "main"
            };
            storage.SetApplication(app);
            var locked = storage.GetLock("test");

            if (isFirst) {
                Assert.IsTrue(locked);
            } else {
                Assert.IsFalse(locked);
            }
        }

        [Test]
        public void CanLockWikiPage() {
            app = new Application();
            app.Container.Register(new ComponentDefinition<IPrincipalSource, StubPrincipalSource>());
            

            var wikiPage = new WikiPage {
                Code = "test",
                Editor = "remalloc",
                Existed = true,
                LastWriteTime = DateTime.Now,
                Owner = "remalloc",
                Text = "some text",
                Title = "fgfgdfgd"
            };

            var storage = new MongoWikiPersister {
                DatabaseName = "Zefs",
                CollectionName = "main"
            };
            storage.Save(wikiPage);
            var r= storage.Get("test");
            Assert.NotNull(r);

            var t01 = new Task(() => GetLockTask(true));
            var t02 = new Task(() => GetLockTask(false));
            var t03 = new Task(() => GetLockTask(false));
            var t04 = new Task(() => GetLockTask(false));
            var t05 = new Task(() => GetLockTask(false));

            t01.Start();
            t02.Start();
            t03.Start();
            t04.Start();
            t05.Start();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Qorpent.Wiki;
using Zeta.Extreme.MongoDB.Integration.WikiStorage;

namespace Zeta.Extreme.MongoDB.Integration.Tests.Wiki {
    class WikiTests {
        [Test]
        public void CanSaveAndClonePage() {
            var wp = new MongoWikiPersister {
                CollectionName = "Wiki",
                DatabaseName = "Zefs",
            };

            wp.Collection.RemoveAll();

            wp.Save(
                new WikiPage {
                    Code = "Test",
                    Editor = "remalloc",
                    Existed = true,
                    LastWriteTime = DateTime.Now,
                    Owner = "remalloc",
                    Published = DateTime.Now,
                    Text = "sdffsdff",
                    Title = "fggfgdfg",
                    Version = ""
                }
            );

            wp.CreateVersion("Test", "New version");
        }
    }
}

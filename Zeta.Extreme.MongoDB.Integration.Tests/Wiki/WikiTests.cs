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

        [Test]
        public void CanRestorePage() {
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
                    Text = "Not crap",
                    Title = "Test page"
                }
            );

            var correct = wp.GetLatestVersion("Test");
            Assert.NotNull(correct);
            correct.Text = "Not crap";

            var t = wp.CreateVersion("Test", "There will be crap");
            Assert.AreEqual(true, t.IsSuccess);

            var g = wp.GetLatestVersion("Test");
            Assert.NotNull(g);
            Assert.AreEqual(g.Version, t.Version);
            g.Text = "Crap";
            wp.Save(g);

            var z = wp.GetLatestVersion("Test");
            Assert.NotNull(z);
            Assert.AreEqual("Crap", z.Text);

            var u = wp.RestoreVersion("Test", correct.Version);
            Assert.AreEqual(true, u.IsSuccess);

            var q = wp.GetLatestVersion("Test");
            Assert.NotNull(q);
            Assert.AreEqual("Not crap", q.Text);
        }
    }
}

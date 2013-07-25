﻿using System;
using System.Linq;
using NUnit.Framework;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Form.InputTemplates;
using Zeta.Extreme.FrontEnd;
using Zeta.Extreme.Model;

namespace Zeta.Extreme.MongoDB.Integration.Tests.MongoDbFormChat
{
    [TestFixture]
    public class MongoDbFormChatProviderTest
    {
        private MongoDbFormChatProvider _provider;
        private MongoDbConnector _connector;

        [SetUp]
        public void Setup() {
            _provider = new MongoDbFormChatProvider {
                DatabaseName = "test",
                CollectionName = "test"
            };

            _connector = new MongoDbConnector {
                ConnectionString = _provider.ConnectionString,
                DatabaseName = _provider.DatabaseName,
                CollectionName = _provider.CollectionName
            };

            _connector.Collection.RemoveAll();
            _connector.Database.GetCollection(_provider.CollectionName + "_usr").RemoveAll();
        }

        [Test]
        public void CanSaveItem()
        {
            var session = new FormSession(new InputTemplate { Code = "x" }, 2012, 1, new Obj { Id = 2 }) { Usr = "y" };
            _provider.AddMessage(session, "test1","");
            _provider.AddMessage(session, "test2","");
            _provider.AddMessage(session, "test3","");
            var all = _connector.Collection.FindAll().ToArray();
            Assert.AreEqual(3, all.Length);
            Assert.True(all.All(_ => _["year"].AsInt32 == 2012));
            Assert.True(all.All(_ => _["period"].AsInt32 == 1));
            Assert.True(all.All(_ => _["obj"].AsInt32 == 2));
            Assert.True(all.All(_ => _["form"].AsString == "x"));
            Assert.True(all.All(_ => _["user"].AsString == "y"));
            Assert.True(all.Any(_ => _["text"].AsString == "test1"));
            Assert.True(all.Any(_ => _["text"].AsString == "test2"));
            Assert.True(all.Any(_ => _["text"].AsString == "test3"));
        }

        [Test]
        public void CanSaveMessagesToAdminAndSupport()
        {
            var session = new FormSession(new InputTemplate { Code = "x" }, 2012, 1, new Obj { Id = 2 }) { Usr = "y" };
            _provider.AddMessage(session, "test1", "admin");
            _provider.AddMessage(session, "test2", "support");
            _provider.AddMessage(session, "test3", "");
            var all = _connector.Collection.FindAll().ToArray();
            Assert.AreEqual(3, all.Length);
            Assert.True(all.All(_ => _["year"].AsInt32 == 2012));
            Assert.True(all.All(_ => _["period"].AsInt32 == 1));
            Assert.True(all.All(_ => _["obj"].AsInt32 == 2));
            Assert.True(all.All(_ => _["form"].AsString == "x"));
            Assert.True(all.All(_ => _["user"].AsString == "y"));
            Assert.True(all.Any(_ => _["text"].AsString == "test1"));
            Assert.True(all.Any(_ => _["text"].AsString == "test2"));
            Assert.True(all.Any(_ => _["text"].AsString == "test3"));
        }

        [Test]
        public void CanSetHaveRead() {
            var session = GetFormSession();
            
            var fisrst = _provider.AddMessage(session, "First");
            var second = _provider.AddMessage(session, "Second");
            Assert.AreEqual(2, _provider.GetUpdatesCount("SomeOtherUser"));

            _provider.SetHaveRead("SomeOtherUser");

            Assert.AreEqual(0, _provider.GetUpdatesCount("SomeOtherUser"));

            /*_provider.SetHaveRead(session.Usr);
            Assert.AreEqual(0, _provider.GetUpdatesCount(session.Usr));*/
        }

        [Test]
        public void CanCorrectGetLastRead() {
            var session = GetFormSession();
            var fisrst = _provider.AddMessage(session, "First");
            var second = _provider.AddMessage(session, "Second");
            _provider.SetHaveRead("SomeOtherUser");
            var lastRead = _provider.GetLastRead("SomeOtherUser");
            Assert.AreNotEqual(DateTime.MinValue, lastRead);
        }

        [Test]
        public void CanFindItems()
        {
            var session = new FormSession(new InputTemplate { Code = "x" }, 2012, 1, new Obj { Id = 2 }) { Usr = "y" };
            _provider.AddMessage(session, "test1");
            _provider.AddMessage(session, "test2");
            _provider.AddMessage(session, "test3");
            var session2 = new FormSession(new InputTemplate { Code = "x" }, 2013, 1, new Obj { Id = 2 }) { Usr = "y" };
            _provider.AddMessage(session2, "test3");
            _provider.AddMessage(session2, "test4");
            _provider.AddMessage(session2, "test5");
            var result1 = _provider.GetSessionItems(session).ToArray();
            var result2 = _provider.GetSessionItems(session2).ToArray();
            Assert.AreEqual(3, result1.Length);
            Assert.AreEqual(3, result2.Length);
            Assert.True(result1.Any(_ => _.Text == "test1"));
            Assert.True(result2.Any(_ => _.Text == "test4"));

        }

        [Test]
        public void CanMarkRead()
        {
            var session = new FormSession(new InputTemplate { Code = "x" }, 2012, 1, new Obj { Id = 2 }) { Usr = "y" };
            var message = _provider.AddMessage(session, "test1");
            _provider.Archive(message.Id, "x");
            var marks = _connector.Database.GetCollection(_provider.CollectionName + "_usr").FindAll().ToArray();
            Assert.AreEqual(1, marks.Length);
            var mark = marks.First();
            Assert.AreEqual(mark["message_id"].AsString, message.Id);
            Assert.AreEqual(mark["user"].AsString, "x");
            Assert.AreEqual(mark["archive"].AsBoolean, true);
        }

        [TestCase(2004, null, null, false, 2)]
        [TestCase(2006, null, null, true, 2)]
        [TestCase(1991, "2,4", null, false, 1)]
        [TestCase(1991, "2,4", null, true, 2)]
        [TestCase(1991, "4", null, true, 1)]
        [TestCase(1991, "4", null, false, 0)]
        [TestCase(1991, "2", null, false, 1)]
        [TestCase(1991, null, null, false, 2)]
        [TestCase(1991, null, null, true, 4)]
        public void CanFindAll(int year, string objids, string typenames, bool includeread, int count)
        {
            PrepareMessagesForFindAll();
            int[] objs = null;
            string[] types = null;
            if (!string.IsNullOrWhiteSpace(objids))
            {
                objs = objids.Split(',').Select(ConvertExtensions.ToInt).ToArray();
            }
            if (!string.IsNullOrWhiteSpace(typenames))
            {
                types = typenames.Split(',');
            }
            var allmessagesNoFilters = _provider.FindAll("x", new DateTime(year, 1, 1), objs, types, null, includeread).ToArray();
            Assert.AreEqual(count, allmessagesNoFilters.Length);
        }

        [Test]
        public void TestHaveUpdates()
        {
            var session = new FormSession(new InputTemplate { Code = "x" }, 2012, 1, new Obj { Id = 2 }) { Usr = "y" };
            var message = _provider.AddMessage(session, "test1");
            _provider.AddMessage(session, "test2");

            Assert.AreEqual(2, _provider.GetUpdatesCount("x"));
            _provider.Archive(message.Id, "x");
            Assert.AreEqual(1, _provider.GetUpdatesCount("x"));
            _provider.SetHaveRead("x");
            Assert.AreEqual(0, _provider.GetUpdatesCount("x"));

            _provider.AddMessage(session, "test3");
            _provider.AddMessage(session, "test4");
            _provider.AddMessage(session, "test5");

            Assert.AreEqual(3, _provider.GetUpdatesCount("x"));

        }

        private void PrepareMessagesForFindAll()
        {
            var session = new FormSession(new InputTemplate { Code = "x" }, 2012, 1, new Obj { Id = 2 }) { Usr = "y" };
            var message = _provider.AddMessage(session, "test1");
            var message1 = _provider.AddMessage(session, "test2");
            var message2 = _provider.AddMessage(session, "test3");
            var message3 = _provider.AddMessage(session, "test4");
            message1.Time = new DateTime(2001, 1, 1);
            message1.ObjId = 3;
            message2.Time = new DateTime(2005, 1, 1);
            message2.ObjId = 4;
            message3.Time = new DateTime(2007, 1, 1);
            message3.ObjId = 5;
            _connector.Collection.Save(MongoDbFormChatSerializer.ChatItemToBson(null, message1));
            _connector.Collection.Save(MongoDbFormChatSerializer.ChatItemToBson(null, message2));
            _connector.Collection.Save(MongoDbFormChatSerializer.ChatItemToBson(null, message3));
            _provider.Archive(message1.Id, "x");
            _provider.Archive(message2.Id, "x");
        }

        private FormSession GetFormSession() {
            return new FormSession(
                new InputTemplate {
                    Code = "x"
                },
                2012,
                1,
                new Obj {
                    Id = 2
                }
            ) {
                Usr = "y"
            };
        }
    }
}
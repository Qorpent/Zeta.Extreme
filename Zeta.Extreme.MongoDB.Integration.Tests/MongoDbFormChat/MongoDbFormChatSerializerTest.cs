using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using NUnit.Framework;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.BizProcess.Forms;
using Zeta.Extreme.Form.InputTemplates;
using Zeta.Extreme.FrontEnd;
using Zeta.Extreme.Model;

namespace Zeta.Extreme.MongoDB.Integration.Tests.MongoDbFormChat
{
	[TestFixture]
	public class MongoDbFormChatProviderTest {
		private MongoDbFormChatProvider _provider;

		[SetUp]
		public void Setup() {
			_provider = new MongoDbFormChatProvider();
			_provider.DatabaseName = "test";
			_provider.CollectionName = "test";
			_provider.SetupConnection();
			_provider.Database.Drop();
			_provider.SetupConnection();
		}

		[Test]
		public void CanSaveItem() {
			var session = new FormSession(new InputTemplate {Code = "x"}, 2012, 1, new Obj {Id = 2}){Usr = "y"};
			_provider.AddMessage(session, "test1");
			_provider.AddMessage(session, "test2");
			_provider.AddMessage(session, "test3");
			var all = _provider.Collection.FindAll().ToArray();
			Assert.AreEqual(3,all.Length);
			Assert.True(all.All(_=>_["year"].AsInt32==2012));
			Assert.True(all.All(_ => _["period"].AsInt32 == 1));
			Assert.True(all.All(_ => _["obj"].AsInt32 == 2));
			Assert.True(all.All(_ => _["form"].AsString == "x"));
			Assert.True(all.All(_ => _["user"].AsString == "y"));
			Assert.True(all.Any(_=>_["text"].AsString == "test1"));
			Assert.True(all.Any(_=>_["text"].AsString == "test2"));
			Assert.True(all.Any(_=>_["text"].AsString == "test3"));
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
			var result2= _provider.GetSessionItems(session2).ToArray();
			Assert.AreEqual(3,result1.Length);
			Assert.AreEqual(3, result2.Length);
			Assert.True(result1.Any(_=>_.Text=="test1"));
			Assert.True(result2.Any(_ => _.Text == "test4")); 

		}
	}


	[TestFixture]
	public class MongoDbFormChatSerializerTest
	{
		[Test]
		public void NormalizationTest() {
			var doc = new FormChatItem{Text = "xxx"};
			var session = new FormSession(new InputTemplate {Code = "test"},2012,11,new Obj{Id=352}){Usr = "y"};
			MongoDbFormChatSerializer.Normalize(session,doc);
			Assert.AreEqual("y",doc.User);
			Assert.AreEqual(2012,doc.Year);
			Assert.AreEqual(11, doc.Period);
			Assert.AreEqual("test", doc.FormCode);
			Assert.AreEqual("xxx", doc.Text);
			Assert.AreEqual((DateTime.Now-DateTime.Now).TotalSeconds.ToInt(), (doc.Time-DateTime.Now).TotalSeconds.ToInt());
			Assert.False(string.IsNullOrWhiteSpace(doc.Id));
			Assert.AreEqual(352,doc.ObjId);
		}
		[Test]
		public void NormalizationTestPreserveUser() {
			var doc = new FormChatItem {User = "x",Text = "xxx"};
			var session = new FormSession(new InputTemplate { Code = "test" }, 2012, 11, new Obj { Id = 352 });
			MongoDbFormChatSerializer.Normalize(session, doc);
			Assert.AreEqual("x",doc.User);
		}

		[Test]
		public void ToBsonTest() {
			var doc = new FormChatItem { Text = "xxx" };
			var session = new FormSession(new InputTemplate { Code = "test" }, 2012, 11, new Obj { Id = 352 }) { Usr = "y" };
			var bdoc = MongoDbFormChatSerializer.ChatItemToBson(session, doc);
			Assert.AreEqual(bdoc["_id"].AsString,doc.Id);
			Assert.AreEqual(bdoc["user"].AsString, doc.User);
			Assert.AreEqual(bdoc["text"].AsString, doc.Text);
			Assert.AreEqual(bdoc["time"].ToLocalTime().ToString(), doc.Time.ToLocalTime().ToString());
			Assert.AreEqual(bdoc["form"].AsString, doc.FormCode);
			Assert.AreEqual(bdoc["year"].AsInt32, doc.Year);
			Assert.AreEqual(bdoc["period"].AsInt32, doc.Period);
			Assert.AreEqual(bdoc["obj"].AsInt32, doc.ObjId);
		}

		[Test]
		public void ToBsonSearchTest() {
			var session = new FormSession(new InputTemplate { Code = "test" }, 2012, 11, new Obj { Id = 352 }) { Usr = "y" };
			var bdoc = MongoDbFormChatSerializer.SessionToSearchDocument(session);
			Assert.AreEqual(4,bdoc.ElementCount);
			Assert.AreEqual(bdoc["form"].AsString, session.Template.Code);
			Assert.AreEqual(bdoc["year"].AsInt32, session.Year);
			Assert.AreEqual(bdoc["period"].AsInt32, session.Period);
			Assert.AreEqual(bdoc["obj"].AsInt32, session.Object.Id);
		}

		[Test]
		public void FromBsonTest() {
			var bdoc = BsonDocument.Parse("{_id:'zzz', form:'xxx',year:2012,period:1,obj:345,time:ISODate('2013-04-05T06:47:35Z'),user:'x',text:'text' }");
			var doc = MongoDbFormChatSerializer.BsonToChatItem(bdoc);
			Assert.AreEqual(bdoc["_id"].AsString, doc.Id);
			Assert.AreEqual(bdoc["user"].AsString, doc.User);
			Assert.AreEqual(bdoc["text"].AsString, doc.Text);
			Assert.AreEqual(bdoc["time"].ToLocalTime().ToString(), doc.Time.ToString());
			Assert.AreEqual(bdoc["form"].AsString, doc.FormCode);
			Assert.AreEqual(bdoc["year"].AsInt32, doc.Year);
			Assert.AreEqual(bdoc["period"].AsInt32, doc.Period);
			Assert.AreEqual(bdoc["obj"].AsInt32, doc.ObjId);
		}
	}
}

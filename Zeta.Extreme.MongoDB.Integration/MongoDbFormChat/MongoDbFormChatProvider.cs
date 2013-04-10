#region LICENSE

// Copyright 2012-2013 Fagim Sadykov
// Project: Zeta.Extreme.MongoDB.Integration
// Original file :MongoDbFormChatProvider.cs
// Branch: ZEUS
// This code is produced especially for ZEUS PROJECT and
// can be used only with agreement from Fagim Sadykov
// and ZEUS PROJECTS'S owner

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Wrappers;
using Qorpent.IoC;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.MongoDB.Integration {
	/// <summary>
	///     Mongo - реализаци€ чата формы
	/// </summary>
	[ContainerComponent(Lifestyle.Transient, ServiceType = typeof (IFormChatProvider))]
	public class MongoDbFormChatProvider : MongoAttachedProviderBase, IFormChatProvider {
		/// <summary>
		///     ѕоиск св€занных с сессией сообщений чата
		/// </summary>
		/// <param name="session"></param>
		/// <returns>
		/// </returns>
		public IEnumerable<FormChatItem> GetSessionItems(IFormSession session) {
			SetupConnection();
			var search = MongoDbFormChatSerializer.SessionToSearchDocument(session);
			return Collection.Find(new QueryDocument(search)).Select(MongoDbFormChatSerializer.BsonToChatItem);
		}

		/// <summary>
		///     ƒобавление нового сообщени€
		/// </summary>
		/// <param name="session"></param>
		/// <param name="message"></param>
		/// <returns>
		/// </returns>
		public FormChatItem AddMessage(IFormSession session, string message) {
			SetupConnection();
			var item = new FormChatItem {Text = message};
			var bson = MongoDbFormChatSerializer.ChatItemToBson(session, item);
			Collection.Save(bson);
			return item;
		}

		/// <summary>
		///     ѕомечает сообщение с указанным идентификатором как прочтенное
		///     пользователем
		/// </summary>
		/// <param name="uid"></param>
		/// <param name="user"></param>
		public void Archive(string uid, string user) {
			SetupConnection();
			var collection = Database.GetCollection<BsonDocument>(CollectionName + "_usr");
			var data = new Dictionary<string, object>
				{
					{"message_id", uid},
					{"user", user}
				};
			var query = new BsonDocument(data);
			var item = collection.FindOne(new QueryDocument(query));
			if (null == item) {
				item = query;
			}
			item["archive"] = true;
			collection.Save(item);
		}

		/// <summary>
		///     ѕомечает сообщение с указанным идентификатором как прочтенное
		///     пользователем
		/// </summary>
		/// <param name="user"></param>
		public void SetHaveRead(string user) {
			SetupConnection();
			var collection = Database.GetCollection<BsonDocument>(CollectionName + "_usr");
			var data = new Dictionary<string, object>
				{
					{"message_id", "ALL"},
					{"user", user}
				};
			var query = new BsonDocument(data);
			var item = collection.FindOne(new QueryDocument(query));
			if (null == item) {
				item = query;
			}
			item["lastread"] = DateTime.Now;
			collection.Save(item);
		}

		/// <summary>
		///     ¬озвращает дату последней отметки о прочтении
		/// </summary>
		/// <param name="user"></param>
		/// <returns>
		/// </returns>
		public DateTime GetLastRead(string user) {
			SetupConnection();
			var collection = Database.GetCollection<BsonDocument>(CollectionName + "_usr");
			var data = new Dictionary<string, object>
				{
					{"message_id", "ALL"},
					{"user", user}
				};
			var query = new BsonDocument(data);
			var item = collection.FindOne(new QueryDocument(query));
			if (null == item) {
				return DateTime.MinValue;
			}
			if (!item.Contains("lastread")) {
				return DateTime.MinValue;
			}
			return item["lastread"].ToLocalTime();
		}

		/// <summary>
		///     ѕровер€ет наличие обноелний в базе сообщений
		/// </summary>
		/// <param name="user"></param>
		/// <param name="objids"></param>
		/// <param name="types"></param>
		/// <returns>
		/// </returns>
		public long GetUpdatesCount(string user,int[] objids = null, string[] types=null) {
			SetupConnection();
			var lastread = GetLastRead(user);
			var query = GenerateFindAllMessagesQuery(user, lastread, null, null, false);
			query["user"] = new BsonDocument("$ne",user);
			return Collection.Count(new QueryDocument(query));
		}

		/// <summary>
		/// </summary>
		/// <param name="user"></param>
		/// <param name="startdate"></param>
		/// <param name="objids"></param>
		/// <param name="types"></param>
		/// <param name="includeArchived"></param>
		/// <returns>
		/// </returns>
		public IEnumerable<FormChatItem> FindAll(string user, DateTime startdate, int[] objids, string[] types,bool includeArchived) {
			SetupConnection();
			var query = GenerateFindAllMessagesQuery(user, startdate, objids, types, includeArchived);
			var mongoquery = new QueryDocument(query);
			var result = Collection.Find(mongoquery).SetSortOrder(SortBy.Ascending("time")).Select(MongoDbFormChatSerializer.BsonToChatItem).ToArray();
			foreach (var formChatItem in result) {
				var colleciton = Database.GetCollection(CollectionName + "_usr");
				var usrdata  = colleciton.FindOne(new QueryDocument(new BsonDocument(new Dictionary<string, object>
					{
						{"message_id", formChatItem.Id},
						{"user", user}
					})));
				if (null != usrdata) {
					formChatItem.Userdata = usrdata.ToDictionary();
				}
					
			}
			return result;

		}

		private BsonDocument GenerateFindAllMessagesQuery(string user, DateTime startdate, int[] objids, string[] types,
		                                                  bool includeArchived) {
			if (startdate.Year <= 1990) {
				startdate = DateTime.Now.AddDays(-30);
			}
			var query = new BsonDocument();
			query["time"] = new BsonDocument("$gt", startdate);
			if (null != objids && 0 != objids.Length) {
				query["obj"] = new BsonDocument("$in", new BsonArray(objids));
			}
			if (null != types && 0 != types.Length) {
				query["type"] = new BsonDocument("$in", new BsonArray(types));
			}
			if (!includeArchived) {
				query["$where"] = string.Format(
					"!db.{0}_usr.findOne({{message_id:this._id, user:'{1}', archive:true}})",
					CollectionName, user.Replace("\\","\\\\"));
			}
			return query;
		}
	}
}
#region LICENSE
// Copyright 2007-2013 Qorpent Team - http://github.com/Qorpent
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// PROJECT ORIGIN: Zeta.Extreme.Form/StateRule.cs
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver; 
using MongoDB.Driver.Builders;
using Qorpent.IoC;
using Qorpent.MongoDBIntegration;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.MongoDB.Integration {
	/// <summary>
	///     Mongo - реализация чата формы
	/// </summary>
	[ContainerComponent(Lifestyle.Transient, ServiceType = typeof (IFormChatProvider))]
	public class MongoDbFormChatProvider : MongoDbBasedServiceBase, IFormChatProvider {
		/// <summary>
		/// </summary>
        public MongoDbFormChatProvider() {
			ConnectionString = "mongodb://localhost:27017";
			DatabaseName = "MongoAttachedProviderBase";
			CollectionName = GetType().Name;
		}

        /// <summary>
		///     Поиск связанных с сессией сообщений чата
		/// </summary>
		/// <param name="session"></param>
		/// <returns>
		/// </returns>
		public IEnumerable<FormChatItem> GetSessionItems(IFormSession session) {
			
            return Collection.Find(
                new QueryDocument(
                    MongoDbFormChatSerializer.SessionToSearchDocument(session)    
                )
            ).Select(
                MongoDbFormChatSerializer.BsonToChatItem
            );
		}

		/// <summary>
		///     Добавление нового сообщения
		/// </summary>
		/// <param name="session"></param>
		/// <param name="message"></param>
		/// <param name="type"></param>
		/// <returns>
		/// </returns>
		public FormChatItem AddMessage(IFormSession session, string message, string type="default") {
			type = type ?? "default";
			var item = new FormChatItem {
			    Text = message,
				Type = type
			};
            
			Collection.Save(
                MongoDbFormChatSerializer.ChatItemToBson(
                    session,
                    item
                )
            );

			return item;
		}

		/// <summary>
		///     Помечает сообщение с указанным идентификатором как прочтенное
		///     пользователем
		/// </summary>
		/// <param name="uid"></param>
		/// <param name="user"></param>
		public void Archive(string uid, string user) {

            var collection = SelectUsrCollection();
            var query = MakeSearchQueryForUsrCollection(uid, user);
			var item = collection.FindOne(
                new QueryDocument(query)
            ) ?? query;

		    MongoDbFormChatSerializer.SetArchived(item);

			collection.Save(item);
		}

        /// <summary>
        ///     Selects the COLLECTIONAME_usr collection
        /// </summary>
        /// <returns></returns>
        private MongoCollection<BsonDocument> SelectUsrCollection() {
            return Database.GetCollection<BsonDocument>(
                CollectionName + "_usr"
            );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private BsonDocument MakeSearchQueryForUsrCollection(string uid, string user) {
            return new BsonDocument(
                new Dictionary<string, object> {
					{"message_id", uid},
					{"user", user}
			    }
            );
        }

		/// <summary>
		///     Помечает сообщение с указанным идентификатором как прочтенное
		///     пользователем
		/// </summary>
		/// <param name="user"></param>
		public void SetHaveRead(string user) {
           
            var collection = SelectUsrCollection();
            var query = MakeSearchQueryForUsrCollection("ALL", user);

			var document = (
                collection.FindOne(
			        new QueryDocument(query)
		        )
            ) ?? query;

            document.Set("lastread", DateTime.Now);
            collection.Save(document);
		}

		/// <summary>
		///     Возвращает дату последней отметки о прочтении
		/// </summary>
		/// <param name="user"></param>
		/// <returns>
		/// </returns>
		public DateTime GetLastRead(string user) {
            var item = SelectUsrCollection().FindOne(
                Query.And(
                    Query.EQ("message_id", "ALL"),
                    Query.EQ("user", user)
                )  
            );

			if (null == item) {
				return DateTime.MinValue;
			}

			return !item.Contains("lastread") ? DateTime.MinValue : item["lastread"].ToLocalTime();
		}

		/// <summary>
		///     Проверяет наличие обноелний в базе сообщений
		/// </summary>
		/// <param name="user"></param>
		/// <param name="objids"></param>
		/// <param name="types"></param>
		/// <param name="forms"></param>
		/// <returns>
		/// </returns>
		public long GetUpdatesCount(string user,int[] objids = null, string[] types=null, string[] forms = null) {

			var lastread = GetLastRead(user);
			var query = GenerateFindAllMessagesQuery(
                user,
                lastread,
                objids,
                types,
                forms,
                false
            );
			query["user"] = new BsonDocument("$ne",user);
			return Collection.Count(new QueryDocument(query));
		}

		/// <summary>
		/// </summary>
		/// <param name="user"></param>
		/// <param name="startdate"></param>
		/// <param name="objids"></param>
		/// <param name="types"></param>
		/// <param name="forms"></param>
		/// <param name="includeArchived"></param>
		/// <returns>
		/// </returns>
		public IEnumerable<FormChatItem> FindAll(string user, DateTime startdate, int[] objids = null, string[] types=null, string[] forms=null,bool includeArchived=false) {

			var query = GenerateFindAllMessagesQuery(
                user,
                startdate,
                objids,
                types,
                forms,
                includeArchived
            );

			var result = Collection.Find(
                new QueryDocument(query)
            ).SetSortOrder(
                SortBy.Ascending("time")
            ).Select(
                MongoDbFormChatSerializer.BsonToChatItem
            ).ToArray();

			foreach (var formChatItem in result) {
                var usrdata = SelectUsrCollection().FindOne(
                    new QueryDocument(
                        new BsonDocument(
                            new Dictionary<string, object> {
						        {"message_id", formChatItem.Id},
						        {"user", user}
					        }
                        )
                    )
                );

				if (null != usrdata) {
					formChatItem.Userdata = usrdata.ToDictionary();
				}
			}

		    return result;
		}

		private BsonDocument GenerateFindAllMessagesQuery(
            string user,
            DateTime startdate,
            int[] objids,
            string[] types,
            string[] forms,
            bool includeArchived
        ) {
			if (startdate.Year <= 1990) {
				startdate = DateTime.Now.AddDays(-30);
			}

			var query = new BsonDocument();
			query["time"] = new BsonDocument(
                "$gt",
                startdate
            );

			if (null != objids && 0 != objids.Length) {
				query["obj"] = new BsonDocument(
                    "$in",
                    new BsonArray(objids)
                );
			}

			if (null != types && 0 != types.Length) {
				query["type"] = new BsonDocument(
                    "$in",
                    new BsonArray(types)
                );
			}

			if (null != forms && 0 != forms.Length) {
				query["form"] = new BsonDocument(
                    "$in",
                    new BsonArray(forms)
                );
			}

			if (!includeArchived) {
				query["$where"] = string.Format(
					"!db.{0}_usr.findOne({{message_id:this._id, user:'{1}', archive:true}})",
					CollectionName,
                    user.Replace(
                        "\\",
                        "\\\\"
                    )
                );
			}

			return query;
		}
	}
}
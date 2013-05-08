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
using System.Linq;
using MongoDB.Bson;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.MongoDB.Integration.MongoDbLogs
{
	/// <summary>
	/// Сервис регистрации клиентской статистики с браузеров
	/// </summary>
	public class MongoClientStatisticsRegistry : MongoBasedServiceBase, IClientStatisticsRegistry {
		/// <summary>
		/// </summary>
		public MongoClientStatisticsRegistry()
		{
			ConnectionString = "mongodb://localhost:27017";
			DatabaseName = "formdata";
			CollectionName = "clientstats";
		}

		/// <summary>
		/// Регистрирует данные пользовательской сессииds
		/// </summary>
		/// <param name="clientJson"></param>
		/// <param name="srchost"></param>
		/// <param name="usragent"></param>
		public void RegisterClientStatistics(string clientJson, string srchost, string usragent) {
			var doc = BsonDocument.Parse(clientJson);
			doc["host"] = srchost;
			doc["agent"] = usragent;
			doc["time"] = DateTime.Now;
			var head = doc.GetElement("head").Value as BsonDocument;
			var body = doc.GetElement("body");
			doc.Remove("body");
			foreach (var e in head.Elements.ToArray()) {
				doc[e.Name] = e.Value;
			}
			doc.Remove("head");
			doc["body"] = body.Value;
			try {
				SetupConnection();
				Connector.Collection.Save(doc);
			}
			catch {
				// it's not main ativity of programm, can be skipped
			}
		}

	}
}

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
using System.Reflection;
using MongoDB.Bson;
using MongoDB.Driver;
using Qorpent;
using Qorpent.Events;
using Qorpent.Utils.Extensions;

namespace Zeta.Extreme.MongoDB.Integration {
	/// <summary>
	/// Базовый класс для служб, основанных на Mongo
	/// </summary>
	[RequireReset(All=true,Options = new[]{"mongo"})]
	public class MongoBasedServiceBase : ServiceBase {
		private static bool __moduleInitialized;
		private bool __connecitonInitialized;

		/// <summary>
		///     Connector to MongoDB
		/// </summary>
		public MongoDbConnector Connector;

		/// <summary>
		/// Conneciton string for mongo
		/// </summary>
		public string ConnectionString { get; set; }

		/// <summary>
		/// Database in mongo
		/// </summary>
		public string DatabaseName { get; set; }

		/// <summary>
		/// Collection in mongo
		/// </summary>
		public string CollectionName { get; set; }

		/// <summary>
		/// Reset connection
		/// </summary>
		public void SetupConnection() {
			if (!__connecitonInitialized) {
				Connector = new MongoDbConnector
					{
						ConnectionString = ConnectionString,
						DatabaseName = DatabaseName,
						DatabaseSettings = new MongoDatabaseSettings(),
						CollectionName = CollectionName
					};
				__connecitonInitialized = true;
			}
			if (!__moduleInitialized) {
				var moduleJavaScript = Assembly.GetExecutingAssembly().ReadManifestResource("chat.js");
				var bsonJavaScript = new BsonJavaScript(moduleJavaScript);
				Connector.Database.Eval(bsonJavaScript);
				__moduleInitialized = true;
			}
		}
	}
}
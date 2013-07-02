using MongoDB.Bson;
using MongoDB.Driver;
using Qorpent.Mvc;

namespace Zeta.Extreme.MongoDB.Integration.FrontEndStat {
    /// <summary>
    ///     A class for writing user's statistics form frontend to MongoDB
    /// </summary>
    public class FrontEndStatStorage : MongoDbConnector, IClientStatStorage {
        /// <summary>
        ///     Write statistics to MongoDB as a JSON string
        /// </summary>
        /// <param name="jsonStat">a json-string</param>
        public void Write(string jsonStat) {
            Collection.Insert(new UpdateDocument(BsonDocument.Parse(jsonStat)));
        }
    }
}

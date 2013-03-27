using Qorpent.Log;
using MongoDB.Bson;

namespace Zeta.Extreme.MongoDB.Integration {
    class MongoDbLogsSerializer {
        public static BsonDocument LogMessageToBson(LogMessage message) {
            var document = new BsonDocument();

            if(!string.IsNullOrEmpty(message.Name)) document.Set("name", message.Name);
            
            document.Set("level", message.Level);
            
            if (!string.IsNullOrEmpty(message.Code)) document.Set("code", message.Code);
            if (!string.IsNullOrEmpty(message.Message)) document.Set("message", message.Message);
            if (null != message.HostObject) document.Set("hostObject", message.HostObject.ToString());
            if (!string.IsNullOrEmpty(message.User)) document.Set("user", message.User);
            if (null != message.Error) document.Set("error", message.Error.ToString());
            if (!string.IsNullOrEmpty(message.Server)) document.Set("server", message.Server);
            if (!string.IsNullOrEmpty(message.ApplicationName)) document.Set("applicationName", message.ApplicationName);
            
            document.Set("time", message.Time);
            document.Set("lexInfo", message.LexInfo.ToString());
            
            if (null != message.MvcCallInfo) document.Set("mvcCallInfo", message.MvcCallInfo.ToString());
            if (null != message.MvcContext) document.Set("mvcContext", message.MvcContext.ToString());
            
            return document;
        }
    }
}

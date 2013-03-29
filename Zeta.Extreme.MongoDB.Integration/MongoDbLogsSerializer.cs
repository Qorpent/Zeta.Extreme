using System;
using Qorpent.Dsl;
using Qorpent.Log;
using MongoDB.Bson;
using Qorpent.Mvc;

namespace Zeta.Extreme.MongoDB.Integration {
    /// <summary>
    /// 
    /// </summary>
    public static class MongoDbLogsSerializer {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static BsonDocument LogMessageToBson(LogMessage message) {
            var document = new BsonDocument();
            LogMessageToBson(message, document);
            
            return document;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="document"></param>
        public static void LogMessageToBson(LogMessage message, BsonDocument document) {
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
            
            if (null != message.MvcCallInfo) document.Set("mvcCallInfo", message.MvcCallInfo.ToBson());
            if (null != message.MvcContext) document.Set("mvcContext", message.MvcContext.ToBson());

            // set _id manually
            document.Set("_id", ObjectId.GenerateNewId());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static LogMessage BsonToLogMessage(BsonDocument document) {
            var message = new LogMessage();

            BsonToLogMessage(document, message);

            return message;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        /// <param name="message"></param>
        public static void BsonToLogMessage(BsonDocument document, LogMessage message) {
            BsonValue value;
            
            if (document.TryGetValue("name", out value)) message.Name = value.ToString();
            if (document.TryGetValue("level", out value)) message.Level = (LogLevel)value.ToInt32();
            if (document.TryGetValue("code", out value)) message.Code = value.ToString();
            if (document.TryGetValue("message", out value)) message.Message = value.ToString();
            if (document.TryGetValue("hostObject", out value)) message.HostObject = value.ToString();
            if (document.TryGetValue("user", out value)) message.User = value.ToString();
            if (document.TryGetValue("error", out value)) message.Error = new Exception(value.ToString());
            if (document.TryGetValue("server", out value)) message.Server = value.ToString();
            if (document.TryGetValue("applicationName", out value)) message.ApplicationName = value.ToString();
            if (document.TryGetValue("time", out value)) message.Time = value.ToUniversalTime();
            //if (document.TryGetValue("lexInfo", out value)) message.LexInfo = value.ToString();
            //if (document.TryGetValue("mvcCallInfo", out value)) message.MvcCallInfo = value.ToString();
            //if (document.TryGetValue("mvcContext", out value)) message.MvcContext = value.ToString();
        }
    }
}

using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;
using Qorpent.Dsl;
using Qorpent.Log;
using MongoDB.Bson;
using Qorpent.Mvc;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.MongoDB.Integration {
    /// <summary>
    /// 
    /// </summary>
    public static class MongoDbLogsSerializer {
        /// <summary>
        ///     Represents a LogMessage instance as a BsonDocument object
        /// </summary>
        /// <param name="message">LogMessage instance</param>
        /// <returns>BsonDocument instance</returns>
        public static BsonDocument LogMessageToBson(LogMessage message) {
            var document = new BsonDocument();
            LogMessageToBson(message, document);
            
            return document;
        }
        
        /// <summary>
        ///     Represents a LogMessage instance as a BsonDocument object
        /// </summary>
        /// <param name="message">LogMessage instance</param>
        /// <param name="document">BsonDocument instance</param>
        public static void LogMessageToBson(LogMessage message, BsonDocument document) {
            if(!string.IsNullOrEmpty(message.Name)) document.Set("name", message.Name);
            
            document.Set("level", message.Level);
            
            if (!string.IsNullOrEmpty(message.Code)) document.Set("code", message.Code);
            if (!string.IsNullOrEmpty(message.Message)) document.Set("message", message.Message);

            if (null != message.HostObject) {
                document.Set(
                    "HostObject",
                    FormSessionToBsonDocument((IFormSession)message.HostObject)
                );
            }

            if (!string.IsNullOrEmpty(message.User)) document.Set("user", message.User);

            if (null != message.Error) {
                document.Set(
                    "error",
                    ExceptionToBsonDocument(message.Error)
                );
            }

            if (!string.IsNullOrEmpty(message.Server)) document.Set("server", message.Server);
            if (!string.IsNullOrEmpty(message.ApplicationName)) document.Set("applicationName", message.ApplicationName);
            
            document.Set("time", message.Time);

            if (null != message.MvcCallInfo) {
                var sb = new StringBuilder();
                sb.AppendFormat("{0}", message.LexInfo);
                document.Set("lexInfo", sb.ToString());
            }

            if (null != message.MvcCallInfo) {
                var sb = new StringBuilder();
                sb.AppendFormat("{0}", message.MvcCallInfo);
                document.Set("mvcCallInfo", sb.ToString());
            }

            if (null != message.MvcContext) {
                var sb = new StringBuilder();
                sb.AppendFormat("{0}", message.MvcContext);
                document.Set("mvcContext", sb.ToString());
            }

            // set _id manually
            document.Set("_id", ObjectId.GenerateNewId());
        }

        /// <summary>
        ///     Represents a BsonDocument object as a LogMessage instance
        /// </summary>
        /// <param name="document">BsonDocument source</param>
        /// <returns>LogMessage destination object</returns>
        public static LogMessage BsonToLogMessage(BsonDocument document) {
            var message = new LogMessage();
            BsonToLogMessage(document, message);

            return message;
        }
        
        /// <summary>
        ///     Represents a BsonDocument object as a LogMessage instance
        /// </summary>
        /// <param name="document">BsonDocument source</param>
        /// <param name="message">LogMessage destination object</param>
        public static void BsonToLogMessage(BsonDocument document, LogMessage message) {
            BsonValue value;

            if (document.TryGetValue("name", out value)) message.Name = value.ToString();
            if (document.TryGetValue("level", out value)) message.Level = (LogLevel)value.ToInt32();
            if (document.TryGetValue("code", out value)) message.Code = value.ToString();
            if (document.TryGetValue("message", out value)) message.Message = value.ToString();
            if (document.TryGetValue("hostObject", out value)) message.HostObject = value.ToString();
            if (document.TryGetValue("user", out value)) message.User = value.ToString();

            var doc = document["error"];
            if (doc != BsonNull.Value) {
                message.Error = new Exception(doc["Message"].ToString());
            }

            if (document.TryGetValue("server", out value)) message.Server = value.ToString();
            if (document.TryGetValue("applicationName", out value)) message.ApplicationName = value.ToString();
            if (document.TryGetValue("time", out value)) message.Time = value.ToUniversalTime();
            if (document.TryGetValue("lexInfo", out value)) message.LexInfo = new LexInfo();
            if (document.TryGetValue("mvcCallInfo", out value)) message.MvcCallInfo = new MvcCallInfo();
            if (document.TryGetValue("mvcContext", out value)) message.MvcContext = new SimpleMvcContext();
        }

        /// <summary>
        ///     Exception to BsonDocument serializing
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        private static BsonDocument ExceptionToBsonDocument(Exception exception) {
            var document = new BsonDocument();
            ExceptionToBsonDocument(exception, document);

            return document;
        }

        /// <summary>
        /// 
        /// </summary>
        [Serializable]
        public class SerializedException : Exception, ISerializable {

        }

        private static void ExceptionToBsonDocument(Exception exception, BsonDocument document) {
            document.Set(
                "Data",
                (exception.Data.Count != 0) ? (new BsonDocument(exception.Data)) : (new BsonDocument())
            );

            document.Set(
                "HResult",
                BsonValue.Create(exception.HResult).AsBsonValue
            );

            document.Set(
                "HelpLink",
                (exception.HelpLink != null) ? (BsonValue.Create(exception.HelpLink).AsBsonValue) : (BsonNull.Value)
            );

            document.Set(
                "InnerException",
                (exception.InnerException != null) ? (ExceptionToBsonDocument(exception.InnerException)) : (new BsonDocument())
            );

            document.Set(
                "Message",
                exception.Message
            );

            document.Set(
                "Source",
                (exception.Source != null) ? (BsonValue.Create(exception.Source).AsBsonValue) : (BsonNull.Value)
            );

            document.Set(
                "StackTrace",
                (exception.StackTrace != null) ? (BsonValue.Create(exception.StackTrace).AsBsonValue) : (BsonNull.Value)
            );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formSession"></param>
        /// <returns></returns>
        public static BsonDocument FormSessionToBsonDocument(IFormSession formSession) {
            var document = new BsonDocument();
            FormSessionToBsonDocument(formSession, document);

            return document;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formSession"></param>
        /// <param name="document"></param>
        public static void FormSessionToBsonDocument(IFormSession formSession, BsonDocument document) {
            document.Set("form", formSession.Template.Code);
            document.Set("company", formSession.Object.Name);
            document.Set("period", formSession.Period);
        }
    }
}

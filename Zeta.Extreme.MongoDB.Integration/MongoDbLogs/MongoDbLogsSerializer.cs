using System;
using System.Text;
using Qorpent.Dsl;
using Qorpent.Log;
using MongoDB.Bson;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.MongoDB.Integration {
    /// <summary>
    ///     Serializing ILogMessage to BsonDocument
    /// </summary>
    public static class MongoDbLogsSerializer {
        /// <summary>
        ///     Represents a LogMessage instance as a BsonDocument object
        /// </summary>
        /// <param name="message">LogMessage instance</param>
        /// <returns>BsonDocument instance</returns>
        public static BsonDocument LogMessageToBsonDocument(LogMessage message) {
            var document = new BsonDocument();
            LogMessageToBsonDocument(message, document);
            
            return document;
        }
        
        /// <summary>
        ///     Represents a LogMessage instance as a BsonDocument object
        /// </summary>
        /// <param name="message">LogMessage instance</param>
        /// <param name="document">BsonDocument instance</param>
        public static void LogMessageToBsonDocument(LogMessage message, BsonDocument document) {
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
            document.Set(
                "LexInfo",
                LexInfoToBsonDocument(message.LexInfo)
            );

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
        ///     Exception to BsonDocument serializing
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        private static BsonDocument ExceptionToBsonDocument(Exception exception) {
            var document = new BsonDocument();
            ExceptionToBsonDocument(exception, document);

            return document;
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
        private static BsonDocument FormSessionToBsonDocument(IFormSession formSession) {
            var document = new BsonDocument();
            FormSessionToBsonDocument(formSession, document);

            return document;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formSession"></param>
        /// <param name="document"></param>
        private static void FormSessionToBsonDocument(IFormSession formSession, BsonDocument document) {
            document.Set("form", formSession.Template.Code);
            document.Set("company", formSession.Object.Name);
            document.Set("period", formSession.Period);
        }

        /// <summary>
        ///     Represents a LexInfo document as a BsonDocument
        /// </summary>
        /// <param name="lexInfo">a LexInfo document</param>
        /// <returns>LexInfo document as a BsonDocument</returns>
        private static BsonDocument LexInfoToBsonDocument(LexInfo lexInfo) {
            var document = new BsonDocument();
            LexInfoToBsonDocument(lexInfo, document);

            return document;
        }

        /// <summary>
        ///     Represents a LexInfo document as a BsonDocument
        /// </summary>
        /// <param name="lexInfo">a LexInfo document</param>
        /// <param name="document">an empty BsonDocument instance</param>
        private static void LexInfoToBsonDocument(LexInfo lexInfo, BsonDocument document) {
            document.Set("CharIndex", lexInfo.CharIndex);
            document.Set("Column", lexInfo.Column);
            document.Set("File", lexInfo.File);
            document.Set("Length", lexInfo.Length);
            document.Set("Line", lexInfo.Line);
        }
    }
}

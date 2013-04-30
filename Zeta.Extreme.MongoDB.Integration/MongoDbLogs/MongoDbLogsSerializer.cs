using System;
using System.Text;
using Qorpent.Dsl;
using Qorpent.Log;
using MongoDB.Bson;
using Qorpent.Mvc;
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
            /*if(!string.IsNullOrEmpty(message.Name)) document.Set("name", message.Name);
            
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
                document.Set(
                    "MvcCallInfo",
                    MvcCallInfoToBsonDocument(message.MvcCallInfo)
                );
            }

            if (null != message.MvcContext) {
                document.Set(
                    "MvcContext",
                    MvcContextToBsonDocument(message.MvcContext)
                );
            }
            */
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

        /// <summary>
        ///     Represents a MvcCallInfo document as a BsonDocument
        /// </summary>
        /// <param name="mvcCallInfo">a MvcCallInfo document</param>
        /// <returns>MvcCallInfo document as a BsonDocument</returns>
        private static BsonDocument MvcCallInfoToBsonDocument(MvcCallInfo mvcCallInfo) {
            var document = new BsonDocument();
            MvcCallInfoToBsonDocument(mvcCallInfo, document);

            return document;
        }

        /// <summary>
        ///     Represents a MvcCallInfo document as a BsonDocument
        /// </summary>
        /// <param name="mvcCallInfo">a MvcCallInfo document</param>
        /// <param name="document">an empty BsonDocument instance</param>
        private static void MvcCallInfoToBsonDocument(MvcCallInfo mvcCallInfo, BsonDocument document) {
            document.Set("Url", mvcCallInfo.Url);
            document.Set("ActionName", mvcCallInfo.ActionName);
            document.Set("RenderName", mvcCallInfo.RenderName);
            document.Set("Parameters", mvcCallInfo.Parameters.ToBsonDocument());
        }

        /// <summary>
        ///     Represents an IMvcContext document as a BsonDocument
        /// </summary>
        /// <param name="mvcContext">an IMvcContext document</param>
        /// <returns>IMvcContext document as a BsonDocument</returns>
        private static BsonDocument MvcContextToBsonDocument(IMvcContext mvcContext) {
            var document = new BsonDocument();
            MvcContextToBsonDocument(mvcContext, document);

            return document;
        }

        /// <summary>
        ///     Represents an IMvcContext document as a BsonDocument
        /// </summary>
        /// <param name="mvcContext">an IMvcContext document</param>
        /// <param name="document">an empty BsonDocument instance</param>
        private static void MvcContextToBsonDocument(IMvcContext mvcContext, BsonDocument document) {
            document.Set("Output", mvcContext.Output.ToBsonDocument()); // ???
            document.Set("ActionName", mvcContext.ActionName);
            document.Set("RenderName", mvcContext.RenderName);
//            document.Set("User", mvcContext.User.ToBsonDocument()); // ???
//            document.Set("ViewName", mvcContext.ViewName.ToBsonDocument()); // ???
//            document.Set("MasterViewName", mvcContext.MasterViewName.ToBsonDocument()); // ???
//            document.Set("XData", mvcContext.XData.ToBsonDocument()); // ???
            document.Set("Parameters", mvcContext.Parameters.ToBsonDocument());
//            document.Set("LogonUser", mvcContext.LogonUser.ToBsonDocument()); // ???
//            document.Set("Application", mvcContext.Application.ToBsonDocument()); // ???
//            document.Set("ActionDescriptor", mvcContext.ActionDescriptor.ToBsonDocument()); // ???
//            document.Set("RenderDescriptor", mvcContext.RenderDescriptor.ToBsonDocument()); // ???
//            document.Set("Uri", mvcContext.Uri.ToBsonDocument()); // ???
//            document.Set("AuthrizeResult", mvcContext.AuthrizeResult.ToBsonDocument()); // ???
//            document.Set("IgnoreActionResult", mvcContext.IgnoreActionResult);
//            document.Set("NotModified", mvcContext.NotModified);
//            document.Set("ActionResult", mvcContext.ActionResult.ToBsonDocument());
            document.Set("Error", (mvcContext.Error != null) ? ExceptionToBsonDocument(mvcContext.Error).AsBsonValue : BsonNull.Value);
            document.Set("StatusCode", mvcContext.StatusCode);
            document.Set("LastModified", mvcContext.LastModified);
            document.Set("Etag", (mvcContext.Etag != null) ? BsonValue.Create(mvcContext.Etag).AsBsonValue : BsonNull.Value);
            document.Set("IfModifiedSince", mvcContext.IfModifiedSince);
            document.Set("IfNoneMatch", (mvcContext.IfNoneMatch != null) ? BsonValue.Create(mvcContext.IfNoneMatch).AsBsonValue : BsonNull.Value);
            document.Set("ContentType", (mvcContext.ContentType != null) ? BsonValue.Create(mvcContext.ContentType).AsBsonValue : BsonNull.Value);
            document.Set("Language", (mvcContext.Language != null) ? BsonValue.Create(mvcContext.Language).AsBsonValue : BsonNull.Value);
        }
    }
}

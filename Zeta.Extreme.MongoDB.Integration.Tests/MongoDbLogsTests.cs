using System;
using System.Collections.Generic;
using NUnit.Framework;
using Qorpent.Log;
using Qorpent.Serialization;
using Zeta.Extreme.Form;
using Zeta.Extreme.BizProcess.Forms;
using Zeta.Extreme.BizProcess.StateManagement;
using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.Form.InputTemplates;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;


namespace Zeta.Extreme.MongoDB.Integration.Tests {
    class MongoDbLogsTests : MongoDbLogsTestsBase {


        [Test]
        public void IsValidSerializing()
        {
            var logMessageOrig = new LogMessage();

            logMessageOrig.Name = "Somename";
            logMessageOrig.Level = (LogLevel) 0;
            logMessageOrig.Code = "SomeCode";
            logMessageOrig.Message = "shitshitshit";
            logMessageOrig.Error = new Exception("someExc");
            


            var document = MongoDbLogsSerializer.LogMessageToBson(logMessageOrig);
            var logMessageSerialized = MongoDbLogsSerializer.BsonToLogMessage(document);

            Assert.AreEqual(logMessageOrig.Name, logMessageSerialized.Name);
            Assert.AreEqual(logMessageOrig.Level, logMessageSerialized.Level);
            Assert.AreEqual(logMessageOrig.Code, logMessageSerialized.Code);
            Assert.AreEqual(logMessageOrig.Message, logMessageSerialized.Message);


            // is it correct?
            //Assert.AreEqual(logMessageOrig.Error.ToString(), logMessageSerialized.Error.ToString());
        }

        [Test]
        public void CanWriteLog() {
            var logMessageOrig = GetNewLogInstance();

            _mongoDbLogs.Write(logMessageOrig);
        }
    }
}
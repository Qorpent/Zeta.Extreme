using System;
using System.Collections.Generic;
using NUnit.Framework;
using Qorpent.Log;
using Zeta.Extreme.BizProcess.Forms;
using Zeta.Extreme.MongoDB.Integration;
using MongoDB.Bson;

using System.Threading;

namespace Zeta.Extreme.MongoDB.Integration.Tests {
    class MongoDbLogsTests {
        private MongoDbLogs _mongoDbLogs;

        [TestFixtureSetUp]
        public void FixTureSetUp() {
            _mongoDbLogs = new MongoDbLogs();
        }


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
            var logMessageOrig = new LogMessage();

            logMessageOrig.Name = "Somename";
            logMessageOrig.Level = (LogLevel)0;
            logMessageOrig.Code = "SomeCode";
            logMessageOrig.Message = "shitshitshit";
            logMessageOrig.Error = new Exception("someExc");

 

            _mongoDbLogs.Write(logMessageOrig);
        }
    }
}
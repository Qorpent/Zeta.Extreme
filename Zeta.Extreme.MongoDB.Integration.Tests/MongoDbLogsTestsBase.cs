using System;
using System.Collections.Generic;
using MongoDB.Bson;
using NUnit.Framework;
using Qorpent.Dsl;
using Qorpent.Log;
using Qorpent.Mvc;
using Zeta.Extreme.BizProcess.Forms;
using Zeta.Extreme.BizProcess.StateManagement;
using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.Form.InputTemplates;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.MongoDB.Integration.Tests {
    class MongoDbLogsTestsBase {
        protected MongoDbLogs _mongoDbLogs;

        [TestFixtureSetUp]
        public void FixTureSetUp() {
            _mongoDbLogs = new MongoDbLogs();
        }

        private class TestFormSession : IFormSession {
            public string Uid { get; private set; }
            public int Year { get; private set; }
            public int Period { get; private set; }
            public IZetaMainObject Object { get; private set; }
            public IInputTemplate Template { get; private set; }
            public string Usr { get; private set; }
            public List<OutCell> Data { get; private set; }
            public IUserLog Logger { get; set; }
            public LockStateInfo GetStateInfo() {
                return new LockStateInfo();
            }

            public TestFormSession(int year, int period) {
                Year = year;
                Period = period;
                Template = new InputTemplate();
                Object = new Obj();
            }
        }

        public static LogMessage GetNewLogInstance(string unicString = "", bool noUnic = false) {
            var logInstance = new LogMessage();

            if (string.IsNullOrEmpty(unicString)) {
                if (noUnic == false) {
                    unicString = ObjectId.GenerateNewId().ToString();
                }
            }

            logInstance.Name = "SomeName" + unicString;
            logInstance.Level = (LogLevel) 0;
            logInstance.Code = "Code" + unicString;
            logInstance.Message = "SomeMessage" + unicString;
            logInstance.Error = new Exception("someExc");

            logInstance.HostObject = new TestFormSession(2013, 777) {
                Template = {
                    Code = "SomeTCode" + unicString
                },
                Object = {
                    Name = "SomeOName" + unicString
                }
            };

            logInstance.ApplicationName = "SomeApp" + unicString;
            logInstance.LexInfo = new LexInfo();
            logInstance.MvcCallInfo = new MvcCallInfo();
            logInstance.MvcContext = new SimpleMvcContext();

            return logInstance;
        }
    }
}

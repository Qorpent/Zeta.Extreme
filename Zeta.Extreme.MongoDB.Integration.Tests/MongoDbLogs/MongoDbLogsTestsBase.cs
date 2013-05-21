using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
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
        protected const int WRITE_COMMITS_COUNT = 27;

        protected MongoDbLogs.MongoDbLogs _mongoDbLogs;

        private string _connectionString;
        private string _databaseName;
        private string _logsCollectionName;

        /// <summary>
        ///     Connection string
        /// </summary>
        public string ConnectionString {
            get {
                return _connectionString ?? (_connectionString = MongoDbLayoutSpecification.DEFAULT_CONNECTION_STRING);
            }

            set {
                _connectionString = value;
            }
        }

        /// <summary>
        ///     Logs database
        /// </summary>
        public string DatabaseName {
            get {
                return _databaseName ?? (_databaseName = MongoDbLayoutSpecification.DEFAULT_LOGS_DB);
            }

            set {
                _databaseName = value;
            }
        }

        /// <summary>
        ///     Default collection prefix
        /// </summary>
        public string LogsCollectionName {
            get {
                return _logsCollectionName ?? (_logsCollectionName = MongoDbLayoutSpecification.DEFAULT_LOGS_COLLECTION);
            }

            set {
                _logsCollectionName = value;
            }
        }

        [TestFixtureSetUp]
        public void FixTureSetUp() {
            var mongoClient = new MongoClient(ConnectionString);
            var mongoServer = mongoClient.GetServer();
            var mongoDatabase = mongoServer.GetDatabase(DatabaseName, new MongoDatabaseSettings());
            
            mongoDatabase.Drop();

            _mongoDbLogs = new MongoDbLogs.MongoDbLogs {
                MongoLogsConnectionString = ConnectionString,
                MongoLogsDatabaseName = DatabaseName,
                MongoLogsCollectionName = LogsCollectionName
            };
        }

        [SetUp]
        public void TestSetup() {
            var mongoClient = new MongoClient(ConnectionString);
            var mongoServer = mongoClient.GetServer();
            var mongoDatabase = mongoServer.GetDatabase(DatabaseName, new MongoDatabaseSettings());

            var collections = mongoDatabase.GetCollectionNames();

            foreach (var collection in collections) {
                if (collection.StartsWith(LogsCollectionName)) {
                    mongoDatabase.DropCollection(collection);
                }
            }

        }

        private class TestFormSession : IFormSession {
            public string Uid { get; private set; }
            public int Year { get; private set; }
	        int IFormSession.Period {
		        get { return Period; }
		        set { Period = value; }
	        }

	        public int Period { get; private set; }
            public IZetaMainObject Object { get; private set; }
            public IInputTemplate Template { get; private set; }
            public string Usr { get; set; }
            public List<OutCell> Data { get; private set; }
            public IUserLog Logger { get; set; }
	        public IDictionary<string, object> Parameters { get; private set; }
	        public IZetaMainObject WorkingObject { get; private set; }

	        public LockStateInfo GetStateInfo() {
                return new LockStateInfo();
            }

	        public FormAttachment[] GetAttachedFiles() {
		        throw new NotImplementedException();
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
            logInstance.LexInfo = new LexInfo("testfile", 1, 2, 3, 4);
            logInstance.MvcCallInfo = new MvcCallInfo {
                Url = "https://www.example.com/test.html?q=e#c",
                ActionName = "SomeShittyAction",
                RenderName = "IKillYouIfNotCompile",
                Parameters = new Dictionary<string, string> {
                    {"ass", "hole"}
                }
            };
            logInstance.MvcContext = new SimpleMvcContext();
            logInstance.MvcContext.Error = new Exception();

            return logInstance;
        }
    }
}

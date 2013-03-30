using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using NUnit.Framework;
using Qorpent.Data;
using Qorpent.IoC;
using Qorpent.Log;
using Zeta.Extreme.Form.InputTemplates;
using Zeta.Extreme.Model;
using Zeta.Extreme.FrontEnd;
using Zeta.Extreme.Model.MetaCaches;
using Zeta.Extreme.Model.Querying;
using Qorpent.Data.Connections;



namespace Zeta.Extreme.MongoDB.Integration.Tests {
    class MongoDbLogsTestsBase {
        private static Task loadrowcahe;
        private static bool wascallnhibernate;

        protected ISerialSession _serial;
        protected Session session;

        [SetUp]
		public virtual void setup() {
			session = new Session(true);
			_serial = session.AsSerial();
		}

		[TearDown]
		public void teardown() {
			if (null != session) {
				Console.WriteLine(session.GetStatisticString());
			}
		}

		[TestFixtureSetUp]
		public virtual void FixtureSetup() {
			if (!wascallnhibernate) {
				Qorpent.Applications.Application.Current.Container.Register(new BasicComponentDefinition { Lifestyle = Lifestyle.Singleton, ImplementationType = typeof(DatabaseConnectionProvider), ServiceType = typeof(IDatabaseConnectionProvider) });
				Qorpent.Applications.Application.Current.DatabaseConnections.Register(new ConnectionDescriptor{PresereveCleanup=true, ConnectionString = "Data Source=assoibdx;Initial Catalog=eco;Persist Security Info=True;User ID=sfo_home;Password=rhfcysq$0;Application Name=zeta-test3",Name = "Default"},false);
				Periods.Get(12);
				RowCache.start();
				ColumnCache.Start();
				ObjCache.Start();
				FormulaStorage.Default.AutoBatchCompile = false;
				FormulaStorage.Default.LoadDefaultFormulas(null);
				FormulaStorage.Default.AutoBatchCompile = true;
				ColumnCache.Start();
				wascallnhibernate = true;
			}
		}


        public static LogMessage GetNewLogInstance(string unicString = "") {
            var logInstance = new LogMessage();

            if (string.IsNullOrEmpty(unicString)) {
                unicString = ObjectId.GenerateNewId().ToString();
            }

            logInstance.Name = "SomeName" + unicString;
            logInstance.Level = (LogLevel) 0;
            logInstance.Code = "Code" + unicString;
            logInstance.Message = "SomeMessage" + unicString;
            logInstance.Error = new Exception("someExc");



            logInstance.HostObject =  new FormSession(new InputTemplate {
                Code = "balans2011A.in"
            }, 2012, 13, new Obj {
                Id = 352
            });


            logInstance.ApplicationName = "SomeApp" + unicString;



            return logInstance;
        }
    }
}

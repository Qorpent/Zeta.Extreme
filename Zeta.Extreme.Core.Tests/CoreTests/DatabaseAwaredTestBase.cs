using NUnit.Framework;
using Qorpent.Data;
using Qorpent.Data.Connections;
using Qorpent.IoC;
using Zeta.Extreme.Model.MetaCaches;

namespace Zeta.Extreme.Core.Tests.CoreTests {
    public abstract class DatabaseAwaredTestBase {
        private static bool wascallnhibernate;

        [TestFixtureSetUp]
        public virtual void FixtureSetup() {
            if (!wascallnhibernate) {
                Qorpent.Applications.Application.Current.Container.Register(new BasicComponentDefinition { Lifestyle = Lifestyle.Singleton, ImplementationType = typeof(DatabaseConnectionProvider), ServiceType = typeof(IDatabaseConnectionProvider) });
                Qorpent.Applications.Application.Current.DatabaseConnections.Register(new ConnectionDescriptor{PresereveCleanup=true, ConnectionString = "Data Source=192.168.26.137;Initial Catalog=eco;Persist Security Info=True;User ID=sfo_home;Password=rhfcysq$0;Application Name=zeta-test3",Name = "Default"},false);
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
    }
}
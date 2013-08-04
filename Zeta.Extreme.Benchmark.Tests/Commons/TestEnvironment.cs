using Qorpent.Applications;
using Qorpent.Data;
using Qorpent.Data.Connections;
using Qorpent.IoC;
using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.Form.Themas;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.MetaCaches;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.Benchmark.Tests {
	public static class TestEnvironment {
		public const string CONNECTION =
			"Data Source=192.168.26.137;Initial Catalog=eco;Persist Security Info=True;User ID=sfo_home;Password=rhfcysq$0;Application Name=zeta-test4";
		private static bool initiated = false;
		private static IThemaFactory _themafactory;

		public static void Init() {
			if (initiated) return;
			Application.Current.Container.Register(new BasicComponentDefinition { Lifestyle = Lifestyle.Singleton, ImplementationType = typeof(DatabaseConnectionProvider), ServiceType = typeof(IDatabaseConnectionProvider) });
			Application.Current.DatabaseConnections.Register(new ConnectionDescriptor { PresereveCleanup = true, ConnectionString = CONNECTION, Name = "Default" }, false);
			Periods.Get(12);
			RowCache.start();
			ColumnCache.Start();
			ObjCache.Start();
			FormulaStorage.Default.AutoBatchCompile = false;
			FormulaStorage.Default.LoadDefaultFormulas(null);
			FormulaStorage.Default.AutoBatchCompile = true;
			ExtremeFactory.RealFactory = new DefaultExtremeFactory();
			ColumnCache.Start();
			_themafactory = new ExtremeFormProvider(ThemaSourceHelper.GetSource(), CONNECTION).Factory; 
			initiated = true;
		}

		public static IMetaCache DefaultMetaCache {
			get { return MetaCache.Default; }
		}

		public static IThemaFactory DefaultThemaFactory {
			get { return _themafactory; }
		}
	}
}
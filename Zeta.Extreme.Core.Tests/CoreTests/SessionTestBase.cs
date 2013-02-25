#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : SessionTestBase.cs
// Project: Zeta.Extreme.Core.Tests
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Qorpent.Data;
using Qorpent.Data.Connections;
using Qorpent.IoC;
using Zeta.Extreme.Meta;

namespace Zeta.Extreme.Core.Tests.CoreTests {
	public class SessionTestBase {
		private static Task loadrowcahe;
		private static bool wascallnhibernate;

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
				ColumnCache.start();
				ObjCache.Start();
				FormulaStorage.Default.AutoBatchCompile = false;
				var _sumh = new StrongSumProvider();
				var formulas = RowCache. Byid.Values.Where(_ => _.IsFormula && !_sumh.IsSum(_) && _.ResolveTag("extreme")=="1").ToArray();

				foreach (var f in formulas) {
					var req = new FormulaRequest {Key ="row:"+ f.Code, Formula = f.Formula, Language = f.FormulaEvaluator};
					FormulaStorage.Default.Register(req);
					
				}

				var colformulas = (
					                  from c in ColumnCache.Byid.Values//myapp.storage.AsQueryable<col>()
					                  where c.IsFormula && c.FormulaEvaluator == "boo" && !string.IsNullOrEmpty(c.Formula)
					                  select new {c = c.Code, f = c.Formula}
				                  ).ToArray();


				foreach (var c in colformulas)
				{
					var req = new FormulaRequest { Key = "col:"+c.c, Formula = c.f, Language = "boo" };
					FormulaStorage.Default.Register(req);
					
				}
				FormulaStorage.Default.CompileAll();
				FormulaStorage.Default.AutoBatchCompile = true;
				ColumnCache.start();
				wascallnhibernate = true;
			}
		}

		protected ISerialSession _serial;
		protected Session session;
	}
}
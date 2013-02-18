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
using Comdiv.Application;
using Comdiv.Persistence;
using Comdiv.Zeta.Data.Minimal;
using Comdiv.Zeta.Model;
using NUnit.Framework;

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
				myapp.ioc.Clear();
				myapp.ioc.setupHibernate(
					new NamedConnection("Default",
					                    "Data Source=assoibdx;Initial Catalog=eco;Persist Security Info=True;User ID=sfo_home;Password=rhfcysq$0;Application Name=zeta3"),
					//"Data Source=(local);Initial Catalog=eco;Integrated Security=True;Min Pool Size=5;Application Name=zeta3"),
					new ZetaMinimalMode());
				Periods.Get(12);
				RowCache.start();
				FormulaStorage.Default.AutoBatchCompile = false;
				var _sumh = new StrongSumProvider();
				var formulas = RowCache.byid.Values.Where(_ => _.IsFormula && !_sumh.IsSum(_) && _.ResolveTag("extreme")=="1").ToArray();

				foreach (var f in formulas) {
					var req = new FormulaRequest {Key ="row:"+ f.Code, Formula = f.Formula, Language = f.FormulaEvaluator};
					FormulaStorage.Default.Register(req);
					
				}

				var colformulas = (
					                  from c in myapp.storage.AsQueryable<col>()
					                  where c.IsFormula && c.FormulaEvaluator == "boo" && null!=c.Formula && ""!=c.Formula
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
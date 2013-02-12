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
using Comdiv.Model.Mapping;
using Comdiv.Persistence;
using Comdiv.Zeta.Data.Minimal;
using Comdiv.Zeta.Model;
using FluentNHibernate;
using NHibernate.Cfg;
using NUnit.Framework;

namespace Zeta.Extreme.Core.Tests.CoreTests {
	public class ZetaMinimalMode : PersistenceModel, IConfigurationBoundedModel {
		public ZetaMinimalMode() {
			Add(new rowmap());
			Add(new colmap());
			Add(new objmap("/standalone/", 1));
			Add(new PeriodMap());
		}

		public bool IsFor(Configuration cfg) {
			if (cfg.Properties.ContainsKey("__connection")) {
				if (cfg.Properties["__connection"].ToLower().Contains("postgres")) {
					return false;
				}
			}
			return true;
		}
	}

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
				RowCache.start("m111", "m112", "m260", "m250", "m218", "r590", "m220");
				FormulaStorage.Default.AutoBatchCompile = false;
				var _sumh = new StrongSumProvider();
				var formulas = RowCache.byid.Values.Where(_ => _.IsFormula && !_sumh.IsSum(_)).ToArray();

				foreach (var f in formulas) {
					var req = new FormulaRequest {Key = f.Code, Formula = f.Formula, Language = f.FormulaEvaluator};
					FormulaStorage.Default.Register(req);
					try {
						FormulaStorage.Default.CompileAll();
					}
					catch (Exception e) {
						Console.WriteLine(f.Code + ":" + e.Message);
						req.PreparedType = typeof (CompileErrorFormulaStub);
						req.ErrorInCompilation = e;
					}
				}

				FormulaStorage.Default.AutoBatchCompile = true;
				ColumnCache.start();
				wascallnhibernate = true;
			}
		}

		protected ISerialSession _serial;
		protected Session session;
	}
}
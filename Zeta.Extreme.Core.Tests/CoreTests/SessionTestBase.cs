using System;
using Comdiv.Application;
using Comdiv.Persistence;
using Comdiv.Zeta.Data.Minimal;
using Comdiv.Zeta.Model;
using NUnit.Framework;

namespace Zeta.Extreme.Core.Tests.CoreTests {
	public class SessionTestBase {
		protected ZexSession session;

		[SetUp]
		public void setup() {
			this.session = new ZexSession(true);
		}

		[TearDown]
		public void teardown()
		{
			if(null!=session) {
				Console.WriteLine(session.GetStatisticString());
			}
		}

		private static bool wascallnhibernate ;
		[TestFixtureSetUp]
		public void FixtureSetup() {
			if(!wascallnhibernate) {
				myapp.ioc.Clear();
				myapp.ioc.setupHibernate(
					new NamedConnection("Default",
					                    "Data Source=(local);Initial Catalog=eco;Integrated Security=True;Min Pool Size=5;Application Name=local-debug"),
					new ZetaClassicModel());
				Periods.Get(12);
				RowCache.start();
				ColumnCache.start();
				wascallnhibernate = true;
			}
		}
	}
}
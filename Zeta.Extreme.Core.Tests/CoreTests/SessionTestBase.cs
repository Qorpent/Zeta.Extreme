using System;
using Comdiv.Application;
using Comdiv.Model.Mapping;
using Comdiv.Persistence;
using Comdiv.Zeta.Data.Minimal;
using Comdiv.Zeta.Model;
using FluentNHibernate;
using NHibernate.Cfg;
using NUnit.Framework;

namespace Zeta.Extreme.Core.Tests.CoreTests {

	public class ZetaMinimalMode : PersistenceModel, IConfigurationBoundedModel
	{
		public ZetaMinimalMode()
		{
			Add(new rowmap());
			Add(new colmap());
			Add(new objmap("/standalone/",1));
			Add(new PeriodMap());
		}

		#region IConfigurationBoundedModel Members

		public bool IsFor(Configuration cfg)
		{
			if (cfg.Properties.ContainsKey("__connection"))
			{
				if (cfg.Properties["__connection"].ToLower().Contains("postgres"))
				{
					return false;
				}
			}
			return true;
		}

		#endregion
	}

	public class SessionTestBase {
		protected ZexSession session;

		[SetUp]
		public virtual void setup() {
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
		public virtual void FixtureSetup() {
			if(!wascallnhibernate) {
				myapp.ioc.Clear();
				myapp.ioc.setupHibernate(
					new NamedConnection("Default",
					                    "Data Source=(local);Initial Catalog=eco;Integrated Security=True;Min Pool Size=5;Application Name=local-debug"),
					new ZetaMinimalMode());
				Periods.Get(12);
				RowCache.start("m111","m112","m260","m250");
				ColumnCache.start();
				wascallnhibernate = true;
			}
		}
	}
}
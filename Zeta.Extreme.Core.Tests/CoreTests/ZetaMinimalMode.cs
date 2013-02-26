using Comdiv.Model.Mapping;
using Comdiv.Zeta.Model;
using FluentNHibernate;
using NHibernate.Cfg;

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
}
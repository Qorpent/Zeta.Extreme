using System.Linq;
using NUnit.Framework;
using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.Form.Themas;

namespace Zeta.Extreme.Form.Tests.ThemaLoader
{
	/// <summary>
	/// </summary>
	[Category("integration")]
	[Explicit]
	[TestFixture]
	public class CanLoadBizDependencyTests
	{
		private IThemaFactory factory;

		[TestFixtureSetUp]
		public void FixtureSetup() {
			var formProvider = new ExtremeFormProvider(@"c:\apps\eco\tmp\compiled_themas","Data Source=assoibdx;Initial Catalog=eco;Persist Security Info=True;User ID=sfo_home;Password=rhfcysq$0;Application Name=zeta-test3");
			factory = formProvider.Factory;
		}

		[TestCase("balans_rnma", "balans2011")]
		[TestCase("balans_ros", "balans2011")]
		[TestCase("balans_rns", "balans2011")]
		[TestCase("balans_rsim", "balans2011")]
		[TestCase("balans_rbp", "balans2011")]
		[TestCase("balans_rdbp", "balans2011")]
		[TestCase("controlpointall", "balans")]
		[TestCase("controlpointall", "balans2011")]
		[TestCase("controlpointall", "nalog")]
		[TestCase("controlpointall", "prib")]
		[TestCase("controlpointall", "prib2011")]
		[TestCase("controlpointall", "f8")]
		[TestCase("controlpointall", "f7")]
		[TestCase("controlpointall", "zatr_pr")]
		[TestCase("controlpointall", "zatr")]
		[TestCase("controlpointall", "osnpok")]
		[TestCase("controlpointall", "finres")]
		[TestCase("controlpointall", "ras_dk")]
		[TestCase("controlpointall", "balans_rns")]
		[TestCase("controlpointall", "rashsn")]
		[TestCase("controlpointall", "inv")]
		[TestCase("controlpointall", "zatr_am")]
		[TestCase("controlpointall", "trud_fzp_nespis")]
		[TestCase("controlpointall", "trud_cent_vipl")]
		[TestCase("controlpointall", "trud_obosn")]
		[TestCase("controlpointall", "trud_zatr_lehr_new")]
		[TestCase("controlpointall", "trud_soc_vipl")]
		[TestCase("controlpointall", "trud_zatr_pers")]
		[TestCase("controlpointall", "balans_rdbp")]
		[TestCase("controlpointall", "balans_rsim")]
		[TestCase("controlpointall", "balans_ro")]
		[TestCase("prib_cons", "finres_cons")]
		[TestCase("free_active", "balans2011")]
		[TestCase("osnpok", "rashsn")]
		[TestCase("osnpok", "inv")]
		[TestCase("osnpok", "trud")]
		[TestCase("osnpok", "zatr")]
		[TestCase("osnpok", "nalog")]
		[TestCase("osnpok", "prib2011")]
		[TestCase("osnpok", "finplan")]
		[TestCase("finres", "rashsn")]
		[TestCase("finres", "prib2011")]
		[TestCase("finres", "balans_rsim")]
		[TestCase("f7", "prib2011")]
		[TestCase("f7", "balans2011")]
		[TestCase("f8", "prib2011")]
		[TestCase("inv", "balans_rns")]
		[TestCase("inv", "finact")]
		[TestCase("zatr", "nalog")]
		[TestCase("zatr", "balans2011")]
		[TestCase("zatr", "zatr_am")]
		[TestCase("zatr", "balans_rbp")]
		[TestCase("zatr", "balans_rsim")]
		[TestCase("zatr_pr", "f7")]
		[TestCase("zatr_pr", "zatr")]
		[TestCase("rashsn", "prib2011")]
		[TestCase("ras_dk", "balans2011")]
		[TestCase("finplan", "krzaim_lite")]
		[TestCase("finplan", "ras_dk")]
		[TestCase("finplan", "finact")]
		[TestCase("finplan", "nalog_nds")]
		[TestCase("finplan", "finres")]
		[TestCase("finplan", "balans2011")]
		[TestCase("finplan", "nalog")]
		[TestCase("finplan", "prib2011")]
		[TestCase("finplan", "zatr")]
		[TestCase("nalog", "ras_dk")]
		[TestCase("nalog", "prib2011")]
		[TestCase("nalog_nds19", "balans2011")]
		[TestCase("nalog_nds", "zatr")]
		[TestCase("nalog_nds", "inv")]
		[TestCase("nalog_nds", "balans_rsim")]
		[TestCase("finact", "balans2011")]
		public void TestIsDependent(string targetCode, string sourceCode) {
			var target = factory.Get(targetCode);
			var source = factory.Get(sourceCode);
			Assert.NotNull(target);
			Assert.NotNull(source);
			Assert.NotNull(target.IncomeLinks.FirstOrDefault(_=>_.Source == source && _.Type=="biz.dep" ));
			Assert.NotNull(source.OutcomeLinks.FirstOrDefault(_ => _.Target == target && _.Type == "biz.dep"));
		}
		[TestCase("balans_rnma", "balans2011")]
		[TestCase("balans_ros", "balans2011")]
		[TestCase("balans_rns", "balans2011")]
		[TestCase("balans_rsim", "balans2011")]
		[TestCase("balans_rbp", "balans2011")]
		[TestCase("balans_rdbp", "balans2011")]
		[TestCase("controlpointall", "balans")]
		[TestCase("controlpointall", "balans2011")]
		[TestCase("controlpointall", "nalog")]
		[TestCase("controlpointall", "prib")]
		[TestCase("controlpointall", "prib2011")]
		[TestCase("controlpointall", "f8")]
		[TestCase("controlpointall", "f7")]
		[TestCase("controlpointall", "zatr_pr")]
		[TestCase("controlpointall", "zatr")]
		[TestCase("controlpointall", "osnpok")]
		[TestCase("controlpointall", "finres")]
		[TestCase("controlpointall", "ras_dk")]
		[TestCase("controlpointall", "balans_rns")]
		[TestCase("controlpointall", "rashsn")]
		[TestCase("controlpointall", "inv")]
		[TestCase("controlpointall", "zatr_am")]
		[TestCase("controlpointall", "trud_fzp_nespis")]
		[TestCase("controlpointall", "trud_cent_vipl")]
		[TestCase("controlpointall", "trud_obosn")]
		[TestCase("controlpointall", "trud_zatr_lehr_new")]
		[TestCase("controlpointall", "trud_soc_vipl")]
		[TestCase("controlpointall", "trud_zatr_pers")]
		[TestCase("controlpointall", "balans_rdbp")]
		[TestCase("controlpointall", "balans_rsim")]
		[TestCase("controlpointall", "balans_ro")]
		[TestCase("prib_cons", "finres_cons")]
		[TestCase("free_active", "balans2011")]
		[TestCase("osnpok", "rashsn")]
		[TestCase("osnpok", "inv")]
		[TestCase("osnpok", "trud")]
		[TestCase("osnpok", "zatr")]
		[TestCase("osnpok", "nalog")]
		[TestCase("osnpok", "prib2011")]
		[TestCase("osnpok", "finplan")]
		[TestCase("finres", "rashsn")]
		[TestCase("finres", "prib2011")]
		[TestCase("finres", "balans_rsim")]
		[TestCase("f7", "prib2011")]
		[TestCase("f7", "balans2011")]
		[TestCase("f8", "prib2011")]
		[TestCase("inv", "balans_rns")]
		[TestCase("inv", "finact")]
		[TestCase("zatr", "nalog")]
		[TestCase("zatr", "balans2011")]
		[TestCase("zatr", "zatr_am")]
		[TestCase("zatr", "balans_rbp")]
		[TestCase("zatr", "balans_rsim")]
		[TestCase("zatr_pr", "f7")]
		[TestCase("zatr_pr", "zatr")]
		[TestCase("rashsn", "prib2011")]
		[TestCase("ras_dk", "balans2011")]
		[TestCase("finplan", "krzaim_lite")]
		[TestCase("finplan", "ras_dk")]
		[TestCase("finplan", "finact")]
		[TestCase("finplan", "nalog_nds")]
		[TestCase("finplan", "finres")]
		[TestCase("finplan", "balans2011")]
		[TestCase("finplan", "nalog")]
		[TestCase("finplan", "prib2011")]
		[TestCase("finplan", "zatr")]
		[TestCase("nalog", "ras_dk")]
		[TestCase("nalog", "prib2011")]
		[TestCase("nalog_nds19", "balans2011")]
		[TestCase("nalog_nds", "zatr")]
		[TestCase("nalog_nds", "inv")]
		[TestCase("nalog_nds", "balans_rsim")]
		[TestCase("finact", "balans2011")]
		public void TestIsDependentSoft(string targetCode, string sourceCode)
		{
			var target = factory.Get(targetCode);
			var source = factory.Get(sourceCode);
			Assert.NotNull(target);
			Assert.NotNull(target.IncomeLinks.FirstOrDefault(_ => (_.Source == source && _.Type == "biz.dep")||(_.Value==sourceCode && _.Type == "biz.dep.orphan")));
			if (null != source) {
				Assert.NotNull(source.OutcomeLinks.FirstOrDefault(_ => _.Target == target && _.Type == "biz.dep"));
			}
		}
	}
}

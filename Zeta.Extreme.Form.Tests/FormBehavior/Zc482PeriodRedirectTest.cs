using NUnit.Framework;
using Zeta.Extreme.Form.InputTemplates;
using Zeta.Extreme.FrontEnd;
using Zeta.Extreme.Model;

namespace Zeta.Extreme.Form.Tests.FormBehavior
{
	[TestFixture]
	public class Zc482PeriodRedirectTest {
		private const string sampleredirect =
			@"PR_KVART:11=1
	|
	:11=12";

		private const string realredirect =
			@"PR_KVART:11=1,12=1,13=1,14=2,15=2,16=2,17=3,18=3,19=3,110=4,111=4,112=4|:251=-99,252=-99,253=-99,254=-99,303=-99,306=-99,309=-99,301=-99";
		[Test]
		public void CanRedirectFormsWithSimplePeriodRedirect() {
			var template = new InputTemplate();
			template.PeriodRedirect = sampleredirect;
			var prepared = template.PrepareForPeriod(2012, 11, Qorpent.QorpentConst.Date.Begin, new Obj());
			Assert.AreEqual(12,prepared.Period);
		}
		[Test]
		public void CanRedirectFormsWithKvartPeriodRedirect()
		{
			var template = new InputTemplate();
			template.PeriodRedirect = sampleredirect;
			var prepared = template.PrepareForPeriod(2012, 11, Qorpent.QorpentConst.Date.Begin, new Obj{GroupCache = "/PR_KVART/"});
			Assert.AreEqual(1, prepared.Period);
		}
		[Test]
		public void CanRedirectFormsWithSimpleRealPeriodRedirect()
		{
			var template = new InputTemplate();
			template.PeriodRedirect = realredirect;
			var prepared = template.PrepareForPeriod(2012, 11, Qorpent.QorpentConst.Date.Begin, new Obj());
			Assert.AreEqual(11, prepared.Period);
		}
		[Test]
		public void CanRedirectFormsWithKvartRealPeriodRedirect()
		{
			var template = new InputTemplate();
			template.PeriodRedirect = realredirect;
			var prepared = template.PrepareForPeriod(2012, 11, Qorpent.QorpentConst.Date.Begin, new Obj { GroupCache = "/PR_KVART/" });
			Assert.AreEqual(1, prepared.Period);
		}
		[Test]
		public void FormSessionStartsWell() {
			var session = new FormSession(new InputTemplate{PeriodRedirect = realredirect},2012,11,new Obj{GroupCache = "/PR_KVART/"} );
			Assert.AreEqual(1,session.Period);
		}
	}
}

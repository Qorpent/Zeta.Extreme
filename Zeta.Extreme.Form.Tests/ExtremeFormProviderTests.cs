using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Zeta.Extreme.Form.Themas;

namespace Zeta.Extreme.Form.Tests
{
	[TestFixture]
	public class ExtremeFormProviderTests
	{
		private ExtremeFormProvider _efp;

		[TestFixtureSetUp]
		public void FixtureSetup() {
			_efp = new ExtremeFormProvider(@"c:\apps\eco\tmp\compiled_themas");

		}
		[TestCase("prib2011A.in",true)]
		[TestCase("balans2011A.in",true)]
		[TestCase("zatrA.in",true)]
		[TestCase("free_activeA.in",false)]
		public void CanAccessThemas(string code, bool existed) {
			var form = _efp.Get(code);
			if(existed) {
				Assert.NotNull(form);
			}else {
				Assert.Null(form);
			}
		}
		[Test]
		public void LimitThemaSet()
		{
			Assert.AreEqual(6,_efp.Factory.GetAll().Count());
		} 
	}
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		[TestCase("free_activeA.in",true)]
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
			Assert.AreEqual(129,_efp.Factory.GetAll().Count());
		}

		[Test]
		[Explicit]
		public void TimeToReloadMustBeMinimal() {
			var sw = Stopwatch.StartNew();
			for(var i=0;i<20;i++) {
				_efp.Reload();
				var f = _efp.Factory;
			}
			sw.Stop();
			Console.WriteLine(sw.Elapsed);
			Assert.Less(sw.ElapsedMilliseconds,20000);
		} 
	}
}

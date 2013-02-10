using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Comdiv.Zeta.Model;
using NUnit.Framework;

namespace Zeta.Extreme.Core.Tests.SubQuery
{
	[TestFixture]
	public class ZetaVirtualSumHelperTest
	{
		private ZetaVirtualSumHelper h;

		[SetUp]
		public void setup() {
			h = new ZetaVirtualSumHelper();
		}

		[Test]
		public void GeneralIsSumTest([Values(true,false)]bool marksum) {
			var r = new row();
			if(marksum) r.MarkCache = "/0SA/";
			Assert.AreEqual(marksum,h.IsSum(r));
			
		}

		[TestCase("$rcode@colcode.Y-1.P3? + $ddd11@fd22 ", false)]
		[TestCase("$rcode@colcode.Y-1.P3? + $ddd11@fd22? ", true)]
		[TestCase("$rcode@colcode.P3.Y-1? + @fd? ", false)]
		[TestCase("$rcode@colcode? ", true)]
		[TestCase("$rcode@colcode.Y-1? ", true)]
		[TestCase("$rcode@colcode.Y-1.P-301? ", true)]
		[TestCase("$rcode@colcode.Y-1.P3? ", true)]
		[TestCase("$rcode?+$r1? * $r2?", false)]
		[TestCase("$rcode?+$r1? - $r2?", true)]
		[TestCase("$rcode?+$r1?", true)]
		[TestCase("$rcode? - $r1?", true)]
		[TestCase("$rcode?",true)]
		[TestCase("$rcode", false)]
		[TestCase(" $rcode? ", true)]
		[TestCase("-$rcode1? ", true)]
		[TestCase(" - $rcode1? ", true)]
		public void FormulaIsSumTest(string formula, bool result)
		{
			var r = new row {Formula = formula, IsFormula = true, FormulaEvaluator = "boo"};
			Assert.AreEqual(result, h.IsSum(r));

		}

		[Test]
		public void CanRetrieveValidDeltasFromFormula() {
			const string formula = "-$r1@c1.Y-1.P-3? + $r2@c2.Y2014? - $r3.P1?";
			var r = new row {IsFormula = true, Formula = formula, FormulaEvaluator = "boo"};
			var result = h.GetSumDelta(r);
			Assert.AreEqual(3,result.Length);
			Assert.AreEqual("r1",result[0].RowCode);
			Assert.AreEqual("c1", result[0].ColumCode);
			Assert.AreEqual(-1, result[0].Year);
			Assert.AreEqual(-3, result[0].Period);
			Assert.AreEqual(-1,result[2].Multiplicator);
			Assert.AreEqual(-1, result[0].Multiplicator);
		}


	}
}

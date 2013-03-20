#region LICENSE
// Copyright 2007-2013 Qorpent Team - http://github.com/Qorpent
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// PROJECT ORIGIN: Zeta.Extreme.Core.Tests/ZetaVirtualSumHelperTest.cs
#endregion
using NUnit.Framework;
using Zeta.Extreme.Model;

namespace Zeta.Extreme.Core.Tests.SubQuery {
	[TestFixture]
	public class ZetaVirtualSumHelperTest {
		[SetUp]
		public void setup() {
			h = new StrongSumProvider();
		}

		private StrongSumProvider h;

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
		[TestCase("$rcode?", true)]
		[TestCase("$rcode", false)]
		[TestCase(" $rcode? ", true)]
		[TestCase("-$rcode1? ", true)]
		[TestCase(" - $rcode1? ", true)]
		public void FormulaIsSumTest(string formula, bool result) {
			var r = new Row {Formula = formula, IsFormula = true, FormulaType = "boo"};
			Assert.AreEqual(result, h.IsSum(r));
		}

		[Test]
		public void CanRetrieveValidDeltasFromFormula() {
			const string formula = "-$r100@c1.Y-1.P-3? + $r2@c2.Y2014? - $r3.P1?";
			var r = new Row {IsFormula = true, Formula = formula, FormulaType = "boo"};
			var result = h.GetSumDelta(r);
			Assert.AreEqual(3, result.Length);
			Assert.AreEqual("r100", result[0].RowCode);
			Assert.AreEqual("c1", result[0].ColCode);
			Assert.AreEqual(-1, result[0].Year);
			Assert.AreEqual(-3, result[0].Period);
			Assert.AreEqual(-1, result[2].Multiplicator);
			Assert.AreEqual(-1, result[0].Multiplicator);
		}

		[Test]
		public void GeneralIsSumTest([Values(true, false)] bool marksum) {
			var r = new Row();
			if (marksum) {
				r.MarkCache = "/0SA/";
			}
			Assert.AreEqual(marksum, h.IsSum(r));
		}
	}
}
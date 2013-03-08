using NUnit.Framework;

namespace Zeta.Extreme.Form.Tests.ZC371TestBench {
	[TestFixture]

	public class Zc371TestBench {
		[TestCase("A|B|C", "A", true, true)]
		[TestCase("A|B|C", "B", true, true)]
		[TestCase("A|B|C", "C", true, true)]
		[TestCase("A|B|C", "C,B", true, true)]
		[TestCase("A,B,C", "C,B", false, true)]
		[TestCase("A,B,C", "C,B,A", true, true)]
		[TestCase("A,B|C", "C", true, true)]
		[TestCase("A,B|C", "A,B", true, true)]
		[TestCase("A,B|C", "B", false, true)]
		[TestCase("A|B|C", "", false, true)]
		[TestCase("A,B,C", "D", false, true)]

		[TestCase("A|B|C", "A", true, false)]
		[TestCase("A|B|C", "B", true, false)]
		[TestCase("A|B|C", "C", true, false)]
		[TestCase("A|B|C", "C,B", true, false)]
		[TestCase("A,B,C", "C,B", false, false)]
		[TestCase("A,B,C", "C,B,A", true, false)]
		[TestCase("A,B|C", "C", true, false)]
		[TestCase("A,B|C", "A,B", true, false)]
		[TestCase("A,B|C", "B", false, false)]
		[TestCase("A|B|C", "", false, false)]
		[TestCase("A,B,C", "D", false,false)]
		public void AllMatchersWorkOnListValid(string condition, string conditions, bool value,bool usenew) {
			PerformSingleConditionTest(condition, conditions, value, usenew);
		}

		private static void PerformSingleConditionTest(string condition, string conditions, bool value, bool usenew) {
			var matcher = usenew
				              ? (ConditionMatcherBase) new NewConditionMatcherImplementation()
				              : new OldConditionMatcherImplementation();
			var result = matcher.Match(condition, conditions.Split(','));
			Assert.AreEqual(value, result, "old matcher problem");
			Assert.AreEqual(0, matcher.EvaluatedByScriptCount, "old matcher script");
		}


		[TestCase("A and B","A",false)]
		[TestCase("A and B", "B", false)]
		[TestCase("A and B", "A,B", false)]
		[TestCase("A and (B or C) and not D", "A", false)]
		[TestCase("A and (B or C) and not D", "A,B", true)]
		[TestCase("A and (B or C) and not D", "A,C", true)]
		[TestCase("A and (B or C) and not D", "B", false)]
		[TestCase("A and (B or C) and not D", "C", false)]
		[TestCase("A and (B or C) and not D", "B,C", false)]
		[TestCase("A and (B or C) and not D", "A,B,C", true)]
		[TestCase("A and (B or C) and not D", "A,B,C,D", false)]
		public void OldMatcherCanEvalFormulas (string condition, string conditions, bool value) {
			PerformSingleConditionTest(condition, conditions, value, false);
		}
		[TestCase("A and B", "A", false)]
		[TestCase("A and B", "B", false)]
		[TestCase("A and B", "A,B", false)]
		[TestCase("A and (B or C) and not D", "A", false)]
		[TestCase("A and (B or C) and not D", "A,B", true)]
		[TestCase("A and (B or C) and not D", "A,C", true)]
		[TestCase("A and (B or C) and not D", "B", false)]
		[TestCase("A and (B or C) and not D", "C", false)]
		[TestCase("A and (B or C) and not D", "B,C", false)]
		[TestCase("A and (B or C) and not D", "A,B,C", true)]
		[TestCase("A and (B or C) and not D", "A,B,C,D", false)]
		public void NewMatcherCanEvalFormulas(string condition, string conditions, bool value)
		{
			PerformSingleConditionTest(condition, conditions, value, true);
		}


		
	}
}
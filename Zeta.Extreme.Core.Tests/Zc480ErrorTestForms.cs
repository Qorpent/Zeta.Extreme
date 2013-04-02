using NUnit.Framework;
using Zeta.Extreme.Core.Tests.CoreTests;
using Zeta.Extreme.Model;

namespace Zeta.Extreme.Core.Tests {
	/// <summary>
	/// </summary>
	[TestFixture]
	public class Zc480ErrorTestForms : SessionTestBase {
		[Test]
		public void FormulaWorksAsExpectedInNotErrorMode() {
			var q = new Query
				{
					Row =
						{
							Native =
								new Row
									{
										Code = "r1",
										IsFormula = true,
										Formula = "f.If ( periodin(11) , { raise('11') } , { 1 } )",
										FormulaType = "boo"
									}
						},
					Col = {Code = "Á1"},
					Time = {Year = 2013, Period = 12}
				};
			var result = _serial.Eval(q);
			Assert.True(result.IsComplete);
			Assert.AreEqual(1, result.NumericResult);
		}

		[Test]
		public void FormulaWorksAsExpectedInErrorMode() {
			var q = new Query
				{
					Row =
						{
							Native =
								new Row
									{
										Code = "r1",
										IsFormula = true,
										Formula = "f.If ( periodin(11) , { raise('11') } , { 1 } )",
										FormulaType = "boo"
									}
						},
					Col = {Code = "Á1"},
					Time = {Year = 2013, Period = 11}
				};
			var result = _serial.Eval(q);
			Assert.False(result.IsComplete);
			Assert.AreEqual("11", result.Error.InnerException.Message);
		}

		[TestCase("TEST_ERRORS_SUM", 11, 0, false)]
		[TestCase("TEST_ERRORS_SUM", 12, 0, false)]
		[TestCase("TEST_ERRORS_SUM", 13, 0, false)]
		[TestCase("TEST_ERRORS_SUM", 14, 6, true)]

		[TestCase("TEST_ERRORS_11", 11, 0, false)]
		[TestCase("TEST_ERRORS_11", 12, 1, true)]
		[TestCase("TEST_ERRORS_11", 13, 1, true)]
		[TestCase("TEST_ERRORS_11", 14, 1, true)]

		[TestCase("TEST_ERRORS_12", 11, 2, true)]
		[TestCase("TEST_ERRORS_12", 12, 0, false)]
		[TestCase("TEST_ERRORS_12", 13, 2, true)]
		[TestCase("TEST_ERRORS_12", 14, 2, true)]

		[TestCase("TEST_ERRORS_13", 11, 3, true)]
		[TestCase("TEST_ERRORS_13", 12, 3, true)]
		[TestCase("TEST_ERRORS_13", 13, 0, false)]
		[TestCase("TEST_ERRORS_13", 14, 3, true)]
		public void TestWithSavedForm(string code, int period, int value, bool proceed) {
			var q = new Query
				{
					Row = {Code = code},
					Col = {Code = "Á1"},
					Time = {Year = 2013, Period = period},
				};
			var result = _serial.Eval(q);
			if (!proceed) {
				Assert.False(result.IsComplete);
				Assert.NotNull(result.Error);
			}
			else {
				Assert.True(result.IsComplete);
				Assert.Null(result.Error);
				Assert.AreEqual(value, result.NumericResult);
			}
		}
	}
}
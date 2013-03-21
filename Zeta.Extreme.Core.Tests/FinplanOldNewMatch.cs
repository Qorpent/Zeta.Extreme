using NUnit.Framework;
using Zeta.Extreme.Core.Tests.CoreTests;

namespace Zeta.Extreme.Core.Tests {
	/// <summary>
	/// </summary>
	[TestFixture]
	public class FinplanOldNewMatch:SessionTestBase {

		[TestCase("f1102312",1028800)]
		public void DoTest(string code, decimal value) {
			var result = _serial.Eval(new Query
				{
					Row = { Code =code},
					Col = { Code = "PLAN" },
					Time = { Year = 2013, Period = 251 },
					Obj = { Id = 354 },
				});
			Assert.AreEqual(value, result.NumericResult);
		}
	}
}
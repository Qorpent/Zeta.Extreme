using NUnit.Framework;
using Zeta.Extreme.Core.Tests.CoreTests;

namespace Zeta.Extreme.Core.Tests {
	[TestFixture]
	public class Af575ContoSupport : SessionTestBase {
		[Test]
		public void CanEvalConto() {
			var query = new Query
				{
					Row = {Code = "z111911"},
					Col = {Code = "SUMMA"},
					Obj = {Id = 467},
					Time = {Year = 2013, Period = 13}
				};
			var realquery = session.Register(query);
			var result = _serial.Eval(realquery);
			Assert.AreEqual(164.0318, result.NumericResult);
		}
	}
}
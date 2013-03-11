using System;
using NUnit.Framework;
using Zeta.Extreme.Core.Tests.CoreTests;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Core.Tests {
	[TestFixture]
	public class CanCalculateValuta : SessionTestBase
	{
		[Test]
		public void SimpleUsdCalculation()
		{
			var query = new Query
				{
					Row = { Code = "m260113" },
					Col = { Code = "Á1" },
					Obj = { Id = 352, Type = ObjType.Obj },
					Time = { Year = 2012, Period = 1 }
				}
				;
			var result = _serial.Eval(query);
			var rubresult = result.NumericResult;
			query = new Query
				{
					Row = {Code = "m260113"},
					Col = {Code = "Á1"},
					Obj = {Id = 352, Type = ObjType.Obj},
					Time = {Year = 2012, Period = 1},
					Valuta = "USD",
				};
			result = _serial.Eval(query);
			const decimal usdrate = 29.32820m;
			Assert.AreEqual(Math.Round(rubresult /usdrate, 6), result.NumericResult);
		}
	}
}
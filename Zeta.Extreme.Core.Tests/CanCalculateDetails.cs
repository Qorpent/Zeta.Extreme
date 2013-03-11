using NUnit.Framework;
using Zeta.Extreme.Core.Tests.CoreTests;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.Querying;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Core.Tests {
	[TestFixture]
	public class CanCalculateDetails:SessionTestBase {
		[Test]
		public void SimpleDetailCount() {
			var query = new Query
				{
					Row = {Code="m1303111"},
					Col = {Code="Á1"},
					Obj = {Id=236, Type = ObjType.Detail},
					Time= {Year = 2012,Period = 1}
				}
				;
			var result = _serial.Eval(query);
			Assert.AreEqual(93.89m, result.NumericResult);
		}
	}
}
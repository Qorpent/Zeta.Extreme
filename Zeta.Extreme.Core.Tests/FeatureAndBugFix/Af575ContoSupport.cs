using NUnit.Framework;
using Zeta.Extreme.Core.Tests.CoreTests;
using Zeta.Extreme.Model;

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
		[Test]
		public void Af577CanEvalContoWithoutToObj()
		{
			var query = new Query
			{
				Row = { Native = new Row {Code="Af577CanEvalContoWithoutToObj", IsFormula  = true, FormulaType = "boo", Formula = "$m2060320@PDKOL.conto(\"BA010_91,BA010_92,BA003\")?" }},
				Col = { Code = "Б1" },
				Obj = { Id = 536 },
				Time = { Year = 2013, Period = 13 }
			};
			var realquery = session.Register(query);
			var result = _serial.Eval(realquery);
			Assert.AreEqual(3622.974m, result.NumericResult);
		}
	}
}
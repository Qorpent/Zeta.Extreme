using System;
using NUnit.Framework;
using Zeta.Extreme.Core.Tests.CoreTests;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.Core.Tests {
	[TestFixture]
	public class Af577ConsobjSupport : SessionTestBase
	{
		[Test]
		public void CanEvalConsobj()
		{
			var query = new Query
				{
					Row = { Code = "z111511" },
					Col = { Code = "SUMMA" },
					Obj = { Id = 467 },
					Time = { Year = 2013, Period = 13 }
				};
			var realquery = session.Register(query);
			var result = _serial.Eval(realquery);
            if (null != result.Error) Console.WriteLine(result.Error);
			Assert.True(result.IsComplete);
		}



		[Test]
		public void Af583RowByCode()
		{
			var query = new Query
			{
				Row = { Code = "z111813" },
				Col = { Code = "SUMMA" },
				Obj = { Id = 449 },
				Time = { Year = 2013, Period = 1 }
			};
			var realquery = session.Register(query);
			var result = _serial.Eval(realquery);

			Assert.AreEqual(47.958, result.NumericResult);
		}


		[Test]
		public void Af583RowBySimpleFormula()
		{
			var query = new Query
			{
				Row = { Native = new Row { Code = "nfiff", IsFormula = true, FormulaType = "boo", Formula = "$z1004182@KOL.consobj()?" } },
				Col = { Code = "SUMMA" },
				Obj = { Id = 449 },
				Time = { Year = 2013, Period = 1 }
			};
			var realquery = session.Register(query);
			var result = _serial.Eval(realquery);

			Assert.AreEqual(47.958, result.NumericResult);
		}

		[Test]
		public void Af583RowByFIFFormula()
		{
			var query = new Query
			{
				Row = { Native = new Row { Code = "fiff", IsFormula = true, FormulaType = "boo", Formula = "f.If ( colin ( \"SUMMA\" ) , { $z1004182@KOL.consobj()? } )" } },
				Col = { Code = "SUMMA" },
				Obj = { Id = 449 },
				Time = { Year = 2013, Period = 1 }
			};
			var realquery = session.Register(query);
			var result = _serial.Eval(realquery);

			Assert.AreEqual(47.958, result.NumericResult);
		}

		[Test]
		public void Af583FormulaParsing() {
			var r1 =
				FormulaStorage.Default.Register(new FormulaRequest
					{
						Key = "Af583FormulaParsing",
						Language = "boo",
						Formula = "f.If ( colin ( \"SUMMA\" ) , { $z1004182@KOL.consobj()? } )"
					
				});
			var r2 = FormulaStorage.Default.Register(new FormulaRequest
			{
				Key = "Af583FormulaParsing2",
				Language = "boo",
				Formula = "$z1004182@KOL.consobj()?"

			});
			Assert.AreEqual("Eval( new QueryDelta{ RowCode = \"z1004182\", ColCode = \"KOL\", UseConsolidateObject = true, })", FormulaStorage.Default.GetRequest(r2).PreprocessedFormula);
			Assert.AreEqual("f.If(()=> colin ( \"SUMMA\" ) , ()=>( Eval( new QueryDelta{ RowCode = \"z1004182\", ColCode = \"KOL\", UseConsolidateObject = true, }) ) )", FormulaStorage.Default.GetRequest(r1).PreprocessedFormula);
		}
	}
}
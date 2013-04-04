using System;
using NUnit.Framework;
using Zeta.Extreme.Core.Tests.CoreTests;

namespace Zeta.Extreme.Core.Tests {
	/// <summary>
	/// </summary>
	[TestFixture]
	public class Zc491RealData:SessionTestBase {
		[Test]
		public void Zc491_m2244201() {
			var q = new Query
				{
					Row = {Code = "m2244201"},
					Col = {Code = "CUSTOM", IsFormula = true, FormulaType = "boo", Formula = @"f.If ( treetagin('/nds_nob:*/'), {  @PLAN? - $__nds_nob@PLAN? - @NNOB? - @NOB18? - @NOB10? - @NOB08? - @NOB12? - @NOB20? }, { @PLAN? - @NNOB? - @NOB18? - @NOB12? - @NOB10? - @NOB08? - @NOB20? } ) "},
					Time = {Year = 2013, Period = 251},
					Obj = {Id = 449}
				};
			q = (Query) session.Register(q);
			q.WaitPrepare();
			Assert.Null(q.Result);
			var r = session.AsSerial().Eval(q);
			if (null != r.Error) {
				Console.WriteLine(r.Error);
			}
			Assert.True(r.IsComplete);
			Assert.Null(r.Error);
			Assert.AreEqual(0,r.NumericResult);
		}

		[Test]
		public void Zc491_z210211()
		{
			var q = new Query
			{
				Row = { Code = "z210211" },
				Col = { Code = "KOLEDFACTCALC" },
				Time = { Year = 2012, Period = 112 },
				Obj = { Id = 449 }
			};
			q = (Query)session.Register(q);
			q.WaitPrepare();
			Assert.Null(q.Result);
			var r = session.AsSerial().Eval(q);
			if (null != r.Error)
			{
				Console.WriteLine(r.Error);
			}
			Assert.True(r.IsComplete);
			Assert.Null(r.Error);
			Assert.AreEqual(0.0568m, Math.Round(r.NumericResult,4));
		}
	}
}
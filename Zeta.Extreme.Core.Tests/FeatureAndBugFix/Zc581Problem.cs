using System;
using NUnit.Framework;
using Zeta.Extreme.Core.Tests.CoreTests;

namespace Zeta.Extreme.Core.Tests {
    /// <summary>
	/// Не работает формула со сменой строки по настройкам в объекте
	/// </summary>
	[TestFixture]
	public class Zc581Problem:SessionTestBase {
		[Test]
		public void Zc581Main()
		{
			var result = _serial.Eval(new Query
				{
					Row = { Code = "z1001181" },
					Col = { Code = "KOLEDPLANCALC" },
					Time = { Year = 2013, Period = 251 },
					Obj = { Id = 3740 },
				});
			Assert.True(result.IsComplete);
			Assert.Null(result.Error);
		}

		[Test]
		public void Zc581Min1()
		{
			var result = _serial.Eval(new Query
			{
				Row = { Code = "z1001181" },
				Col = { Code = "CUSTOM", IsFormula = true, Formula = "$__OPS@__OPK.torootobj()?", FormulaType = "boo" },
				Time = { Year = 2013, Period = 251 },
				Obj = { Id = 3740 },
			});
			if (null != result.Error) {
				Console.WriteLine(result.Error.ToString());
			}
			Assert.True(result.IsComplete);
			Assert.Null(result.Error);
		}
	}
}
using NUnit.Framework;
using Zeta.Extreme.Core.Tests.CoreTests;

namespace Zeta.Extreme.Core.Tests {
    [TestFixture]
    public class ZC612KmultNotResolved:SessionTestBase {
        [Test]
        public void Zc612Main() {
            var result = _serial.Eval(new Query
            {
                Row = { Code = "z1002000" },
                Col = { Code = "SUMMAEDFACTCALC" },
                Time = { Year = 2013, Period = 406 },
                Obj = { Id = 3616 },
            });
            Assert.True(result.IsComplete);
            Assert.Null(result.Error);
        }

        [Test]
        public void Zc612FormulaCheck()
        {
            var result = _serial.Eval(new Query
            {
                Row = { Code = "z1002000" },
                Col = { Code = "c1", FormulaType = "boo", Formula = "@SUMMA? / f.choose( $__OFS@__OFK?, $__OFS@__OFK.torootobj()? ) * ___KMULT" },
                Time = { Year = 2013, Period = 406 },
                Obj = { Id = 3616 },
            });
            var result2 = _serial.Eval(new Query
            {
                Row = { Code = "z1002000" },
                Col = { Code = "c2", FormulaType = "boo", Formula = "@SUMMA? / f.choose( $__OFS@__OFK?, $__OFS@__OFK.torootobj()? ) * 1000" },
                Time = { Year = 2013, Period = 406 },
                Obj = { Id = 3616 },
            });
            Assert.Greater(result2.NumericResult,result.NumericResult);
        }
    }
}
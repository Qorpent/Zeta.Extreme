using System;
using NUnit.Framework;
using Zeta.Extreme.Core.Tests.CoreTests;

namespace Zeta.Extreme.Core.Tests {
    [TestFixture]
    public class ZC615NotCalcSUMMAEDFACTCALC_In_z1002820 : SessionTestBase
    {
        [Test]
		[Ignore("Формула у строки была отключена")]
        public void ZC615_Reproduce_Test() {
            var q = session.Register(new Query {
                Row = { Code = "z1002820" },
                Col = {Code = "SUMMAEDFACTCALC"},
                Time = {Year = 2013, Period = 406},
                Obj = {Id = 3800},
            });
            session.WaitPreparation();
            session.WaitEvaluation();
            var result = q.Result;
            Assert.Null(result.Error);
            Console.WriteLine(result.NumericResult);
            Assert.AreNotEqual(0, Math.Round(result.NumericResult, 4));
        }

       
        
        [Test]
        public void ZC615_Test_Right_No_Colin()
        {
            var q = session.Register(new Query
            {
                Row = { Code = "ZC615_Test_Right_No_Colin", IsFormula = true, FormulaType = "boo", Formula = "$m216250@Б1.toparentobj()? * $z1002500@SUMMA? / $z1002500@SUMMA.consobj(\"_PARENT.CLASS\")? / f.choose( $__OFS@__OFK?, $__OFS@__OFK.torootobj()? ) * ___KMULT " },
                Col = { Code = "SUMMAEDFACTCALC" },
                Time = { Year = 2013, Period = 406 },
                Obj = { Id = 3800 },
            });
            session.WaitPreparation();
            session.WaitEvaluation();
            var result = q.Result;
            Assert.Null(result.Error);
            Console.WriteLine(result.NumericResult);
            Assert.AreNotEqual(0, Math.Round(result.NumericResult, 4));
        }

        private static int idx = 0;
        [TestCase("$m216250@Б1.toparentobj()?")]
        [TestCase("$z1002500@SUMMA?")]
        [TestCase("$z1002500@SUMMA.consobj(\"_PARENT.CLASS\")?")]
        [TestCase("getn(\"KMULT\")")]
        [TestCase("f.choose( $__OFS@__OFK?, $__OFS@__OFK.torootobj()? )")]
        public void ZC615_Test_Mult(string formula)
        {
            var q = session.Register(new Query
            {
                Row = { Code = "ZC615_Test_Mult"+idx, IsFormula = true, FormulaType = "boo", Formula = formula },
                Col = { Code = "SUMMAEDFACTCALC" },
                Time = { Year = 2013, Period = 406 },
                Obj = { Id = 3800 },
            });
            
            session.WaitPreparation();
            session.WaitEvaluation();
            var result = q.Result;
            Assert.Null(result.Error);
            Console.WriteLine(result.NumericResult);
            Assert.AreNotEqual(0, Math.Round(result.NumericResult, 4));
        }

    }
}
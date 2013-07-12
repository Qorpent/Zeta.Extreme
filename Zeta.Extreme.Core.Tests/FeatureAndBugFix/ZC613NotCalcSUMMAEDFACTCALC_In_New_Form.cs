using System;
using NUnit.Framework;
using Zeta.Extreme.Core.Tests.CoreTests;
using Zeta.Extreme.Model;

namespace Zeta.Extreme.Core.Tests {
    [TestFixture]
    public class ZC613NotCalcSUMMAEDFACTCALC_In_New_Form : SessionTestBase
    {
        [Test]
        public void ZC613_Reproduce_Test() {
            var q = session.Register(new Query
            {
                Row = { Code = "z1002410" },
                Col = { Code = "SUMMAEDFACTCALC" },
                Time = { Year = 2013, Period = 406 },
                Obj = { Id = 3800 },
            });
            session.WaitPreparation();
            session.WaitEvaluation();
            var result = q.Result;
            Assert.Null(result.Error);
            Assert.AreEqual(9.9984,Math.Round(result.NumericResult,4));
        }

        [Test]
        public void ZC613_Reproduce_Test_3616()
        {
            var result = _serial.Eval(new Query
            {
                Row = { Code = "z1002410" },
                Col = { Code = "SUMMAEDFACTCALC" },
                Time = { Year = 2013, Period = 406 },
                Obj = { Id = 3616 },
            });
            Assert.Null(result.Error);
            Assert.AreEqual(10.4084, Math.Round(result.NumericResult, 4));
        }

        [Test]
        public void ZC613_Reproduce_Test_3799()
        {
            var result = _serial.Eval(new Query
            {
                Row = { Code = "z1002410" },
                Col = { Code = "SUMMAEDFACTCALC" },
                Time = { Year = 2013, Period = 406 },
                Obj = { Id = 3799 },
            });
            Assert.Null(result.Error);
            Assert.AreEqual(0, Math.Round(result.NumericResult, 4));
        }

        

        [Test]
        public void ZC613Test_KMULT() {
            var q = session.Register(new Query
            {
                Row = { Code = "z1002410" },
                Col = { Native = new Column() { Code = "ZC613_Decomposition_2", IsFormula = true,FormulaType = "boo", Formula = "getn(\"KMULT\")",MarkCache = "/AGGREGATEOBJ/"} },
                Time = { Year = 2013, Period = 406 },
                Obj = { Id = 3800 },
            });
            session.WaitEvaluation();
            Assert.AreEqual(1,q.Result.NumericResult);
        }

        [Test]
        public void ZC613Test_KMULT_NOGG_SUM()
        {
            var q = session.Register(new Query
            {
                Row = { Code = "z1002410" },
                Col = { Native = new Column() { Code = "ZC613_Decomposition_2", IsFormula = true, FormulaType = "boo", Formula = "getn(\"KMULT\")", MarkCache = "/NO_AGGREGATEOBJ/" } },
                Time = { Year = 2013, Period = 406 },
                Obj = { Id = 3800 },
            });
            session.WaitEvaluation();
            Assert.AreEqual(2, q.Result.NumericResult);
        }

        [TestCase(3799, 10)]
        [TestCase(3616, 10.4084)]
        [TestCase(3800, 9.9984)]
        [Ignore("Ќужно было только дл€ исследовани€ проблемы")]
        public void ZC613_Decomposition(int obj, decimal result)
        {
            var item1 = _serial.Eval(new Query
            {
                Row = { Code = "z1002410" },
                Col = { Code = "SUMMA" },
                Time = { Year = 2013, Period = 406 },
                Obj = { Id = obj },
            });
            Console.WriteLine("1. SUMMA = "+item1.NumericResult);
           
            var item2 = _serial.Eval(new Query
            {
                Row = { Code = "z1002410" },
                Col = { Code = "ZC613_Decomposition_1", FormulaType = "boo", Formula = "$__OFS@__OFK?" },
                Time = { Year = 2013, Period = 406 },
                Obj = { Id = obj },
            });
            Console.WriteLine("2. ($__OFS@__OFK?) = " + item2.NumericResult);

            var item3 = _serial.Eval(new Query
            {
                Row = { Code = "z1002410" },
                Col = { Code = "ZC613_Decomposition_2", FormulaType = "boo", Formula = "___KMULT" },
                Time = { Year = 2013, Period = 406 },
                Obj = { Id = obj },
            });

            Console.WriteLine("3. (___KMULT) = " + item3.NumericResult);

            var expected = item1.NumericResult/item2.NumericResult*item3.NumericResult;
            Console.WriteLine("Expected = " + expected);

            Assert.AreEqual(0, Math.Round(expected, 4));
        }


    }
}
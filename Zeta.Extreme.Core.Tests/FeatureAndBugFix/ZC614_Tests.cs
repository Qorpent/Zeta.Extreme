using System;
using System.Collections.Generic;
using NUnit.Framework;
using Zeta.Extreme.Core.Tests.DatabaseIgnorancePureZeta;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Querying;
using Zeta.Extreme.Primary;

namespace Zeta.Extreme.Core.Tests {
    [TestFixture]
    public class ZC614_Tests : PureZetaTestFixtureBase
    {
       

        [Test]
        public void QueryCacheString_Changed_If_LockFormula() {
            var q1 = new Query {Obj = {LockFormula = false}};
            var q2 = new Query {Obj = {LockFormula = true}};
            Assert.AreNotEqual(q1.GetCacheKey(),q2.GetCacheKey());
            
        }

        [Test]
        public void Can_Determaine_That_Is_LockFormula()
        {
            var q1 = new Query { Obj = { Id=1,IsFormula = true,Formula = "1,2,3"},Col = {Native = new Column{MarkCache = "/AGGREGATEOBJ/"}}};
            var q2 = new Query { Obj = { Id = 1, IsFormula = true, Formula = "1,2,3" }, Col = { Native = new Column { MarkCache = "/NO_AGGREGATEOBJ/" } } };
            var q3 = new Query { Obj = { Id = 1 }, Col = { Native = new Column { MarkCache = "/AGGREGATEOBJ/" } } };
            q1.Normalize();
            q2.Normalize();
            Assert.True(q1.Obj.LockFormula);
            Assert.False(q2.Obj.LockFormula);
            Assert.False(q3.Obj.LockFormula);
            Assert.True(q1.Obj.IsPrimary());
            Assert.False(q2.Obj.IsPrimary());
            Assert.True(q3.Obj.IsPrimary());
            Assert.AreNotEqual(q1.GetCacheKey(), q2.GetCacheKey());
        }

        [Test]
        public void Bug_Cannot_Determine_Sum_Delta()
        {
            var _sumh = new StrongSumProvider();
            Assert.AreEqual(3,
                _sumh.GetSumDelta(new ObjHandler { Id = 1, IsFormula = true, Formula = "1,2,3" }).Length);
        }

        [Test]
        public void Bug_Cannot_Determine_Sum_Object() {
            var _sumh = new StrongSumProvider();
            Assert.True(_sumh.IsSum(new ObjHandler { Id = 1, IsFormula = true, Formula = "1,2,3" }));
        }

        [Test]
        public void Bug_Cannot_Reproduce_Formula()
        {
           
            var q2 = new Query
            {
                Obj = { Id = 1, IsFormula = true, Formula = "1,2,3" }
                ,
                Col = { Native = new Column { Code = "y", MarkCache = "/NO_AGGREGATEOBJ/" } }
            };
      
            var realq2 = _session.Register(q2);
            _session.WaitPreparation();
            Assert.AreEqual(3, ((Query)realq2).SummaDependency.Count);
        }

        [Test]
        public void Lock_Formula_Prevent_ObjFormula() {
            var q1 = new Query {
                Obj = {Id = 1, IsFormula = true, Formula = "1,2,3"}
                ,
                Col = {Native = new Column {Code="x",MarkCache = "/AGGREGATEOBJ/"}}
            };
            var q2 = new Query {
                Obj = {Id = 1, IsFormula = true, Formula = "1,2,3"}
                ,
                Col = {Native = new Column {Code="y",MarkCache = "/NO_AGGREGATEOBJ/"}}
            };
            var realq1 = _session.Register(q1);
            var realq2 = _session.Register(q2);
            _session.WaitPreparation();
            Assert.AreNotSame(realq1,realq2);
            Assert.AreEqual(0,((Query)realq1).SummaDependency.Count);
            Assert.AreEqual(3,((Query)realq2).SummaDependency.Count);
        }

        [Test]
        public void Lock_Formula_Keep_Lock_Formula_On_Direct_Changes() {
            _session.MetaCache.Set(new Column {Code = "SUMMA"});
            var q1 = new Query
            {
                Obj = { Id = 1, IsFormula = true, Formula = "1,2,3" }
                ,
                Col = { Native = new Column { Code = "x", IsFormula = true, Formula = "@SUMMA?", FormulaType = "boo", MarkCache = "/AGGREGATEOBJ/" } }
            };
           
            var realq1 = _session.Register(q1);
            _session.WaitPreparation();
           Assert.True(realq1.Obj.LockFormula);
        }

        [Test]
        public void Prototype_Query_Test() {
            var q1 = new Query
            {
                Obj = { Id = 333, IsFormula = true, Formula = "1,2,3" }
                ,
                Col = { Native = new Column {Id=2, Code = "x", MarkCache = "/AGGREGATEOBJ/" } }
            };
            q1.Normalize();
            var p = q1.GetPrototype();
            Assert.True(p.UseSum);
            Assert.True(p.UseLockedObject);
        }

        [Test]
        public void Valid_Sql_Generation() {
            var sqlg = new Zeta2SqlScriptGenerator();
            var q1 = new Query
            {
                Obj = { Id = 333, IsFormula = true, Formula = "1,2,3" }
                ,
                Col = { Native = new Column { Id = 2, Code = "x", MarkCache = "/AGGREGATEOBJ/" } }
            };
            q1.Normalize();
            var result = sqlg.Generate(new IQuery[] {q1}, q1.GetPrototype());
            Console.WriteLine(result);
            Assert.AreEqual("SELECT 0,col,row,333,year,0, SUM(decimalvalue) , 1 ,':'   FROM cell  WHERE period=0 and year=0 and  col in ( 2) and obj=333 and row in (0) and detail is null  GROUP BY col,row,obj,year,period", result.Trim());
        }

        [Test]
        public void Lock_Formula_Keeped_In_Formulas()
        {
            _session.MetaCache.Set(new Column { Code = "SUMMA" });
            var q1 = new Query
            {
                Obj = { Id = 333, IsFormula = true, Formula = "1,2,3" }
                ,
                Col = { Native = new Column { Code = "x", IsFormula = true,Formula = "@SUMMA? * @SUMMA?", FormulaType = "boo", MarkCache = "/AGGREGATEOBJ/" } }
            };

            var realq1 = _session.Register(q1);
            _session.WaitPreparation();
            Assert.AreEqual(2,((Query)realq1).FormulaDependency.Count);
            Assert.AreEqual(333,((Query)realq1).FormulaDependency[0].Obj.Id);
            Assert.True(((Query)realq1).FormulaDependency[0].Obj.LockFormula);
               
        }

        protected override IEnumerable<Query> BuildModel() {
            Add(new Query
            {
                Obj = { Id = 333, IsFormula = true, Formula = "1,2,3",LockFormula = true}
                ,
                Col = { Native = new Column { Code = "SUMMA"  } }
                ,
                Row= {Code="a"}
            },10);
            yield return new Query {
                Obj = {Id = 333, IsFormula = true, Formula = "1,2,3"}
                ,
                
                Row= {Code="a"},
                Col = {
                    Native =
                        new Column {
                            Code = "x",
                            IsFormula = true,
                            Formula = "@SUMMA? * @SUMMA?",
                            FormulaType = "boo",
                            MarkCache = "/AGGREGATEOBJ/"
                        }
                }
            };
        }

        protected override void Examinate(Query query) {
            Assert.AreEqual(100,query.Result.NumericResult);
        }
    }
}
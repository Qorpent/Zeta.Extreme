using System.Collections.Generic;
using NUnit.Framework;

namespace Zeta.Extreme.Core.Tests.DatabaseIgnorancePureZeta {
    [TestFixture]
    public class SumPeriodFormulaSupport : PureZetaTestFixtureBase {
        protected override IEnumerable<Query> BuildModel() {
            Add(new Query{Row={Code="x"},Col={Code="y"},Obj = {Id=1},Time = {Year = 2013,Periods = new[]{1,2}}},2);
            yield return new Query{Row={Formula = "$x.P(1,2)?",IsFormula = true,FormulaType = "boo"},Col = {Code="y"},
                Obj={Id=1},Time={Year = 2013,Period = 1000}};
        }

        protected override void Examinate(Query query) {
            Assert.AreEqual(2,query.Result.NumericResult);
        }
    }
}
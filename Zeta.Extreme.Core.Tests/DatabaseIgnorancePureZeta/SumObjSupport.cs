using System.Collections.Generic;
using NUnit.Framework;
using Zeta.Extreme.Model;

namespace Zeta.Extreme.Core.Tests.DatabaseIgnorancePureZeta {
    /// <summary>
	/// </summary>
	[TestFixture]
	public class SumObjSupport : PureZetaTestFixtureBase
	{
		protected override IEnumerable<Query> BuildModel() {
			Add(new Query { Row = { Code = "x" }, Col = { Code = "a" } ,Obj={Id=1}}, 1);
			Add(new Query { Row = { Code = "x" }, Col = { Code = "a" }, Obj = { Id = 2 } }, 2);
			yield return new Query
				{
					Row = { Code = "x"},
					Col = { Code = "a"},
					Obj = { Native = new Obj{Id =3, Formula = "1,2",IsFormula = true}}
				};
		}

		protected override void Examinate(Query query) {
			Assert.AreEqual(3,query.Result.NumericResult);
		}
	}
}
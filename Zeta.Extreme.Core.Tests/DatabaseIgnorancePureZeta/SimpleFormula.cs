using System.Collections.Generic;
using NUnit.Framework;

namespace Zeta.Extreme.Core.Tests.DatabaseIgnorancePureZeta {
	[TestFixture]
	public class SimpleFormula : PureZetaTestFixtureBase {
		/// <summary>
		/// 	строит модель и возвращает нужные запросы
		/// </summary>
		/// <returns> </returns>
		protected override IEnumerable<Query> BuildModel() {
			Add(new Query {Row = {Code = "x"}}, 5);
			Add(new Query {Row = {Code = "y"}}, 5);
			Add(new Query {Row = {Code = "y"}, Col = {Code = "u"}}, 6);
			yield return new Query { Row = { Code = "PureZetaTestFixtureBase", Formula = "$x? * $y@u?", FormulaType = "boo" } };
		}

		protected override void Examinate(Query query) {
			Assert.AreEqual(30, query.Result.NumericResult);
		}
	}
}
using System.Collections.Generic;
using NUnit.Framework;

namespace Zeta.Extreme.Core.Tests.DatabaseIgnorancePureZeta {
	[TestFixture]
	public class BothColumnAndRowFormula : PureZetaTestFixtureBase {
		/// <summary>
		/// 	строит модель и возвращает нужные запросы
		/// </summary>
		/// <returns> </returns>
		protected override IEnumerable<Query> BuildModel() {
			Add(new Query {Row = {Code = "x"}, Col = {Code = "a"}}, 1);
			Add(new Query {Row = {Code = "y"}, Col = {Code = "a"}}, 2);
			Add(new Query {Row = {Code = "z"}, Col = {Code = "a"}}, 3);
			Add(new Query {Row = {Code = "x"}, Col = {Code = "b"}}, 4);
			Add(new Query {Row = {Code = "y"}, Col = {Code = "b"}}, 5);
			Add(new Query {Row = {Code = "z"}, Col = {Code = "b"}}, 6);
			yield return new Query
				{
					Row = {Code = "myf", Formula = "$x? * $y?", FormulaType = "boo"},
					Col = {Code = "sum", Formula = "@a? * @b? + $z@b? / $z@a? ", FormulaType = "boo"}
					/*
					 (xa*ya) * (xb*yb) + zb / za == (1*2)*(4*5) + 6 / 3 = 2 * 20 + 2 = 42
					 */

					 /*
					  * инверсный был бы вариант
					  *  (xa * xb + zb / za ) * (ya * yb + zb / za ) = (4 + 2 ) * (10 + 2) = 6 * 12 = 72
					  *  
					  */

					 // ОТСЮДА - ПОРЯДОК ПРИМЕНЕНИЯ КРАЙНЕ ВАЖЕН
				};
		}

		protected override void Examinate(Query query) {
			Assert.AreEqual(42, query.Result.NumericResult);
		}
	}
}
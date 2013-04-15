using NUnit.Framework;
using Zeta.Extreme.Core.Tests.CoreTests;

namespace Zeta.Extreme.Core.Tests {
	[TestFixture]
	[Ignore("Пока есть впечатление что сами формулы в БД были неверными")]
	public class Zc496RealDataTests:SessionTestBase {
		[Test]
		public void ZC490m217940()
		{
			var q = (Query)session.Register(new Query("m217940", "Б1", 520, 2012, 4));
			Eval(q);
			Assert.True(q.Result.IsComplete);
			Assert.AreNotEqual(0m,q.Result.NumericResult);
		}

		[Test]
		public void ZC490m2174110() {
			var q = (Query)session.Register(new Query("m217940", "Б1", 520, 2012, 4));
			Eval(q);
			Assert.True(q.Result.IsComplete);
			Assert.AreNotEqual(0m, q.Result.NumericResult);
		}
	}

	[TestFixture]
	public class Zc489ErrorInFormula:SessionTestBase {
		[Test]
		public void ZC489m260940()
		{
			var q = (Query)session.Register(new Query("m260940", "Б1", 441, 2013, 11));
			Eval(q);
		}
	}
}
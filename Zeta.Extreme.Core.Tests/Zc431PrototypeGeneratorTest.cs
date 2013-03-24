using NUnit.Framework;
using Zeta.Extreme.Model.Querying;
using Zeta.Extreme.Primary;

namespace Zeta.Extreme.Core.Tests {
	/// <summary>
	/// </summary>
	[TestFixture]
	public class  Zc431PrototypeGeneratorTest {
		[Test]
		public void ValutaBasedQueryAreZetaEvalBased() {
			var q = new Query {Currency = "RUB"};
			var p = q.GetPrototype();
			Assert.True(p.UseZetaEval);
		}
		[Test]
		public void NonValutaBasedQueryAreNotZetaEvalBased()
		{
			var q = new Query { Currency = PrimaryConstants.VALUTA_NONE };
			var p = q.GetPrototype();
			Assert.False(p.UseZetaEval);
		}

		[Test]
		public void DetailModeSumObjForcesSum() {
			var q = new Query {Obj = {Id = 1, DetailMode = DetailMode.SafeSumObject}};
			var p = q.GetPrototype();
			Assert.True(p.UseSum);
			Assert.True(p.RequireDetails);
		}

		[Test]
		public void ContragentModeForcesSum()
		{
			var q = new Query { Reference = {Contragents = "1"}};
			var p = q.GetPrototype();
			Assert.True(p.UseSum);
		}
	}
}
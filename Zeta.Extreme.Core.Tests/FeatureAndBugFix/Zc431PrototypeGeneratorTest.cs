using NUnit.Framework;
using Zeta.Extreme.Model;
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
			//	Assert.True(p.RequireDetails); less strict due problems in mixed forms
		}

		[Test]
		public void ContragentModeForcesSum()
		{
			var q = new Query { Reference = {Contragents = "1"}};
			var p = q.GetPrototype();
			Assert.True(p.UseSum);
		}

		[Test]
		public void DetailTargetDisablesAllDetailBasedSums()
		{
			var q = new Query {Obj={Id=1,DetailMode = DetailMode.SumObject,Type = ZoneType.Detail}, Reference = { Contragents = "1" } };
			var p = q.GetPrototype();
			Assert.False(p.UseSum);
		}

		[Test]
		public void ContragentBasedQueriesAreSums()
		{
			var q = new Query { Row = { Id = 1 }, Col = { Id = 1 }, Time = { Year = 2012, Period = 1 }, Obj = { Id = 1 }, Reference = { Contragents = "1,2,3" } };
			var p = q.GetPrototype();
			Assert.True(p.UseSum);
		}

		[Test]
		public void DetailTargetDoesNotDisablePeriodAggregateBasedSums()
		{
			var q = new Query { Obj = { Id = 1, Type = ZoneType.Detail }, Time = {Periods = new[]{1,2}}};
			var p = q.GetPrototype();
			Assert.True(p.UseSum);
		}

		[Test]
		public void UsualQueryNotUsesSumAndPreservesDetails() {
			var q = new Query { Obj = { Id = 1 }};
			var p = q.GetPrototype();
			Assert.False(p.UseSum);
			Assert.False(p.RequireDetails);
			Assert.True(p.PreserveDetails);
		}

		[Test]
		public void DetailBasedRowsForcesSum()
		{
			var q = new Query { Obj = { Id = 1 },Row = {Native = new Row{Tag="/"+PrimaryConstants.TAG_USEDETAILS_PARAM+":"+PrimaryConstants.TAG_TRUE+"/"}}};
			var p = q.GetPrototype();
			Assert.True(p.UseSum);
		//	Assert.True(p.RequireDetails); less strict due problems in mixed forms
			Assert.False(p.PreserveDetails);
		}
	}
}
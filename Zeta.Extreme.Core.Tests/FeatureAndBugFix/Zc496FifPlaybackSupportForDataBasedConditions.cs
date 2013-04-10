using System;
using System.Linq.Expressions;
using NUnit.Framework;

namespace Zeta.Extreme.Core.Tests {
	[TestFixture]
	public class Zc496FifPlaybackSupportForDataBasedConditions {

		public class UsualFifFormula : BackwardCompatibleFormulaBase
		{
			protected override object EvaluateExpression()
			{
				return f.If( ()=>q.Time.Year == 2011, () => Eval(new QueryDelta { Year = 2013 }),
							() => Eval(new QueryDelta { Year = 2014 }));
			}
		}

		public class DatabasedFifFormula:BackwardCompatibleFormulaBase {
			protected override object EvaluateExpression() {
				return f.If(()=>Eval(new QueryDelta {Year = 2012}) == 100, 
							() =>  Eval(new QueryDelta {Year = 2013}),
				            () =>  Eval(new QueryDelta {Year = 2014}) );
			}
		}

		[Test]
		public void PlayBackWorksWellInUsualCaseTrue() {
			var q = new Query {Time = {Year = 2011}};
			var f = new UsualFifFormula();
			f.Init(q);
			f.Playback(q);
			Assert.AreEqual(1,q.FormulaDependency.Count);
			Assert.AreEqual(2013,q.FormulaDependency[0].Time.Year);
		}
		[Test]
		public void PlayBackWorksWellInUsualCaseFalse()
		{
			var q = new Query { Time = { Year = 2012 } };
			var f = new UsualFifFormula();
			f.Init(q);
			f.Playback(q);
			Assert.AreEqual(1, q.FormulaDependency.Count);
			Assert.AreEqual(2014, q.FormulaDependency[0].Time.Year);
		}

		[Test]
		public void PlayBackWorksWellInDataConditionCase()
		{
			var q = new Query { Time = { Year = 2011 } }; //doesn't matter what to ask
			var f = new DatabasedFifFormula();
			f.Init(q);
			f.Playback(q);
			Assert.AreEqual(3, q.FormulaDependency.Count);
			Assert.AreEqual(2012, q.FormulaDependency[0].Time.Year);
			Assert.AreEqual(2013, q.FormulaDependency[1].Time.Year);
			Assert.AreEqual(2014, q.FormulaDependency[2].Time.Year);
			
		}
	}
}
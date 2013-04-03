using System;
using NUnit.Framework;
using Zeta.Extreme.Model;

namespace Zeta.Extreme.Core.Tests {
	[TestFixture]
	public class Zc486PlayBackHang {
		[Test]
		[Timeout(6000)]
		public void PlaybackRaisedErrorNotHangAndPropagated() {
			var q = new Query
				{
					Row =
						{
							Native =
								new Row {Code = "r1", Formula = "f.If ( (2 / q.Col.Id ) > 3 , {1}, {2} )", FormulaType = "boo", IsFormula = true}
						},
					Col = {Code = "X"},
					Time = {Year = 2013, Period = 1},
					Obj = {Id = 1}
				};
			var s = new Session(true);
			q = (Query) s.Register(q);
			s.WaitPreparation();
			Assert.NotNull(q.Result);
			Console.WriteLine(q.Result.Error);
			Assert.False(q.Result.IsComplete);
			Assert.NotNull(q.Result.Error);
		}
	}
}
using NUnit.Framework;
using Zeta.Extreme.Core.Tests.CoreTests;

namespace Zeta.Extreme.Core.Tests {
	[TestFixture]
	public class Af577ConsobjSupport : SessionTestBase
	{
		[Test]
		public void CanEvalConsobj()
		{
			var query = new Query
				{
					Row = { Code = "z111513" },
					Col = { Code = "SUMMA" },
					Obj = { Id = 467 },
					Time = { Year = 2013, Period = 13 }
				};
			var realquery = session.Register(query);
			var result = _serial.Eval(realquery);

			Assert.True(result.IsComplete);
		}
		
	}
}
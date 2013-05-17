using NUnit.Framework;
using Zeta.Extreme.Core.Tests.CoreTests;

namespace Zeta.Extreme.Core.Tests {
	/// <summary>
	/// </summary>
	[TestFixture]
	public class Zc493AltDivTest:SessionTestBase {
		[Test]
		[Timeout(5000)]
		public void ZC493m231910() {
			var q = Eval(new Query("m231910", "ZDk", 449, 2012, 112) { IgnoreCheckPrimaryExistence = true });
			Assert.True(q.Result.IsComplete);
			Assert.Null(q.Result.Error);
		}

		[Test]
		[Timeout(5000)]
		public void ZC493m230790()
		{
			var q = Eval(new Query("m230790", "ZDk", 449, 2012, 112) { IgnoreCheckPrimaryExistence = true });
			Assert.True(q.Result.IsComplete);
			Assert.Null(q.Result.Error);
		}


		[Test]
		[Timeout(5000)]
		public void ZC493m230750()
		{
			var q = Eval(new Query("m230750", "ZDk", 449, 2012, 112){IgnoreCheckPrimaryExistence = true});
			Assert.True(q.Result.IsComplete);
			Assert.Null(q.Result.Error);
		}

		[Test]
		[Timeout(5000)]
		public void ZC493m230795()
		{
			var q = Eval(new Query("m230795", "ZDk", 449, 2012, 112) { IgnoreCheckPrimaryExistence = true });
			Assert.True(q.Result.IsComplete);
			Assert.Null(q.Result.Error);
		}
	}
}
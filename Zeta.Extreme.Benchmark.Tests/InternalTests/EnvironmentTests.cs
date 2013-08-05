using NUnit.Framework;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Benchmark.Tests.InternalTests {
	[TestFixture]
	public class EnvironmentTests {
		[Test]
		public void DefaultServicesExists() {
			TestEnvironment.Init();
			Assert.NotNull(TestEnvironment.DefaultThemaFactory);
			Assert.NotNull(TestEnvironment.DefaultMetaCache);
		}
		[Test]
		public void DefaultServicesContainsKnownValues()
		{
			TestEnvironment.Init();
			var balans = TestEnvironment.DefaultThemaFactory.GetForm("balans2011A.in");
			Assert.NotNull(balans);
			var row = TestEnvironment.DefaultMetaCache.Get<IZetaRow>("m111");
			Assert.NotNull(row);
			var obj = TestEnvironment.DefaultMetaCache.Get<IZetaMainObject>(352);
			Assert.NotNull(obj);
		}
	}
}
using System;
using NUnit.Framework;
using Qorpent.Serialization;
using Zeta.Extreme.Benchmark.Probes;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.Benchmark.Tests.ProbesTests {
	[TestFixture]
	public class FormLoadProbeTest {
		[Test]
		[Explicit]
		[Repeat(10)]
		public void MultiFormBenchmarkTest() {
			CanLoadFormAndCollectStats();
		}

		[Test]
		public void CanLoadFormAndCollectStats() {
			TestEnvironment.Init();
			var cfg = new ProbeConfig {
				MetaCache = TestEnvironment.DefaultMetaCache,
				ThemaFactory = TestEnvironment.DefaultThemaFactory,
				FormTemplate = "balans2011A.in",
				FormObj = 352,
				FormYear = 2013,
				FormPeriod = 1
			};
			var probe = new FormLoadProbe();
			probe.Initialize(cfg);

			var result = probe.ExecuteSync();
			if (null != result.Error) {
				Console.WriteLine(result.Error);
			}
			Assert.AreEqual(ProbeResultType.Success,result.ResultType);
			var stats = result.Get<SessionStatistics>("stats");
			Assert.NotNull(stats);
			Console.WriteLine(new XmlSerializer().Serialize("stats",stats));
			foreach (var p in result.GetNames(true)) {
				Console.WriteLine("{0} = {1}", p, result.Get(p));
			}

		}
	}
}
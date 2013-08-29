using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Zeta.Extreme.Benchmark.Probes;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.Benchmark.Tests.ProbesTests
{
	[TestFixture]
	public class SingleQueryProbeTest
	{
		[Test]
		public void CanPropagateValidResultInfo() {
			var cfg = new ProbeConfig();
			var probe = new SingleQueryProbe();
			var session = new FakeSession(new QueryResult(10) );
			var query = new Query();
			cfg.Query = query;
			cfg.Session = session;
			probe.Initialize(cfg);
			var result = probe.ExecuteSync();
			if (null != result.Error) {
				Console.WriteLine(result.Error);
			}
			Assert.AreEqual(ProbeResultType.Success,result.ResultType);
			Assert.AreEqual(10, result.Get<int>("value"));
		}

		[Test]
		public void CanPropagateErrorResultInfo()
		{
			var cfg = new ProbeConfig();
			var probe = new SingleQueryProbe();
			var session = new FakeSession(new QueryResult{Error = new Exception("error")});
			var query = new Query();
			cfg.Query = query;
			cfg.Session = session;
			probe.Initialize(cfg);
			var result = probe.ExecuteSync();
			
			Assert.AreEqual(ProbeResultType.Error, result.ResultType);
			Assert.AreEqual("error",result.Error.InnerException.Message);
		}
	}
}

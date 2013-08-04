using System;

namespace Zeta.Extreme.Benchmark.Tests {
	public class ErrorProbe : ProbeBase
	{
		protected override void ExecuteSelf(IProbeResult result, bool async)
		{
			throw new Exception("an error");
		}
	}
}
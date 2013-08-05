namespace Zeta.Extreme.Benchmark.Tests {
	public class IgnoreProbe : ProbeBase
	{
		protected override bool CheckIgnore(ProbeResult result, out string message)
		{
			message = "ignored";
			return true;
		}
	}
}
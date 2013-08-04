using System;
using System.Threading;

namespace Zeta.Extreme.Benchmark.Tests {
	public class TimeOutProbe : ProbeBase {
		private TimeSpan _timeout;

		public TimeOutProbe() : this(new TimeSpan()) {}

		public TimeOutProbe(TimeSpan span) {
			if (span.TotalMilliseconds < 10) {
				span = TimeSpan.FromMilliseconds(300);
			}
			this._timeout = span;
		}
		protected override void ExecuteSelf(IProbeResult result, bool async)
		{
			Thread.Sleep(_timeout);
		}
	}
}
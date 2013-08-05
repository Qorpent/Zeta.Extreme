using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Zeta.Extreme.Benchmark.Tests.AbstractFrameworkWorks
{
	/// <summary>
	/// Фикстура проверяет работу базового шаблона пробы
	/// </summary>
	[TestFixture]
	public class BaseProbeTest
	{
		[TestCase(typeof (ErrorProbe), ProbeResultType.Error)]
		[TestCase(typeof (IgnoreProbe), ProbeResultType.Ignored)]
		[TestCase(typeof (SimpleProbe), ProbeResultType.Success)]
		[TestCase(typeof (TimeOutProbe), ProbeResultType.Success)]
		public void CanRunWithValidReturnType(Type probeType, ProbeResultType expectedResult) {
			var probe = (IProbe) Activator.CreateInstance(probeType);
			var result = probe.ExecuteSync();
			Assert.AreEqual(expectedResult,result.ResultType);
			var asyncresult = probe.ExecuteAsync();
			asyncresult.Wait();
			result = asyncresult.Result;
			Assert.AreEqual(expectedResult, result.ResultType);
		}

		[TestCase(100)]
		[TestCase(200)]
		[TestCase(300)]
		[TestCase(400)]
		[TestCase(500)]
		public void TimeoutIsCheckedTest(int milliseconds) {
			var probe = new TimeOutProbe(TimeSpan.FromMilliseconds(milliseconds));
			var result = probe.ExecuteSync();
			// нужна коррекция в миллисекунду из за косяков с замерами в Stopwatch
			Assert.Greater(result.TotalDuration.Add(TimeSpan.FromMilliseconds(1)), TimeSpan.FromMilliseconds(milliseconds));
			var asyncresult = probe.ExecuteAsync();
			asyncresult.Wait();
			result = asyncresult.Result;
			Assert.Greater(result.TotalDuration.Add(TimeSpan.FromMilliseconds(1)), TimeSpan.FromMilliseconds(milliseconds));
		}

	}
}

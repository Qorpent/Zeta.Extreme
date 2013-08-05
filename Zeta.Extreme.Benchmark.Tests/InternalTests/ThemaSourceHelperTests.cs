using System;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;

namespace Zeta.Extreme.Benchmark.Tests.InternalTests
{
	/// <summary>
	/// Проверяем корректность загрузки тем из ZIP-файла
	/// </summary>
	[TestFixture]
	public class ThemaSourceHelperTests
	{
		[Test]
		public void SourceIsNotNull() {
			Assert.NotNull(ThemaSourceHelper.GetSource());
		}

		[Test]
		public void SourceIsAboutValidFileCount() {
			var src = ThemaSourceHelper.GetSource();
			Assert.Greater(src.GetFileNames().Count(),100);
		}

		[Test]
		public void CanReadSingleFile() {
			var src = ThemaSourceHelper.GetSource();
			var fname = src.GetFileNames().First(_ => _.Contains("balans2011.xml"));
			Assert.NotNull(fname);
			Console.WriteLine(fname);
			var xml = XElement.Load(src.Open(fname));
			Console.WriteLine(xml);
			StringAssert.Contains("баланс",xml.ToString());
		}
	}
}

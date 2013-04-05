using NUnit.Framework;
using Zeta.Extreme.Core.Tests.CoreTests;

namespace Zeta.Extreme.Core.Tests {
	/// <summary>
	/// </summary>
	[TestFixture]
	public class Zc483OnPockOtkl:SessionTestBase
	{
		[TestCase("m201100")]
		[TestCase("m201110")]
		[TestCase("m201120")]
		[TestCase("m201200")]
		[TestCase("m201210")]
		[TestCase("m201220")]
		[TestCase("m201300")]
		[TestCase("m201310")]
		[TestCase("m201320")]
		[TestCase("m201400")]
		[TestCase("m201410")]
		[TestCase("m201420")]	
		[Timeout(5000)]
		public void CheckRowOnTimeout(string code) {
			_serial.Eval(new Query
				{
					Row = {Code = code},
					Col = {Code = "CONTROBSNG"},
					Time = {Year = 2012, Period = 13},
					Obj = {Id = 352}
				});
		}

		[Test]
		public void T110400() {
			var q = session.Register(new Query
				{
					Row = {Code = "t110400"},
					Col = {Code = "CONTROBSNG"},
					Time = {Year = 2012, Period = 13},
					Obj = {Id = 352}
				});
			session.WaitPreparation();
			var result = _serial.Eval(q);

			Assert.True(result.IsComplete);
		}
	}
}
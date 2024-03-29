using System;
using NUnit.Framework;
using Zeta.Extreme.Core.Tests.CoreTests;
using Zeta.Extreme.Model.Extensions;
using Zeta.Extreme.Model.MetaCaches;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.Core.Tests {
	/// <summary>
	/// </summary>
	[TestFixture]
	public class FinplanOldNewMatch:SessionTestBase {

		[TestCase("r211140", 4914781, "Pd")]
		[TestCase("m224614", 4914781, "PLAN")]
		[TestCase("f1102322", 0,"PLAN")]
		[TestCase("f1102312",905000,"PLAN")]
		public void DoTest(string code, decimal value,string col) {
			var q = new Query
				{
					Row = {Code = code},
					Col = {Code = col},
					Time = {Year = 2013, Period = 251},
					Obj = {Id = 354},
				};
			q = (Query) session.Register(q);
			var result = _serial.Eval(q);
			Assert.AreEqual(value, result.NumericResult);
		}

		[Test]
		public void M224614_PLAN_REDIRECTS_TO_PD()
		{
			var q = new Query
			{
				Row = { Code = "m224614" },
				Col = { Code = "PLAN" },
				Time = { Year = 2013, Period = 251 },
				Obj = { Id = 354 },
			};
			q.Normalize(session);
			Assert.AreEqual("Pd", q.Col.Code);
		}


		[Test]
		public void R510_IS_USEDETAIL() {
			Assert.True(RowCache.Bycode["R510"].ResolveTag("usedetails")=="1");
		}

		[Test]
		public void R510110_IS_USEDETAIL()
		{
			Assert.True(RowCache.Bycode["R510110"].ResolveTag("usedetails") == "1");
		}

		

		[Test]
		public void QueryHasCorrectDetailMode() {
			var q = new Query
				{
					Row = {Code = "r510110"},
					Col = {Code = "Pd"},
					Time = {Year = 2013, Period = 251},
					Obj = {Id = 354},
				};
				q.Normalize(session);
			Assert.AreEqual(DetailMode.SafeSumObject,q.Obj.DetailMode);
		}
		
		[Test]
		public void TestFormulaInternalNoAltObj()
		{
			var result = _serial.Eval(new Query
			{
				Row = { Code = "r510110" },
				Col = { Code = "Pd" },
				Time = { Year = 2013, Period = 251 },
				Obj = { Id = 354 },
			});
			Assert.AreEqual(2319543m, result.NumericResult);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="code"></param>
		/// <param name="value"></param>
		[Test]
		public void TestFormulaInternal()
		{
			var result = _serial.Eval(new Query
			{
				Row = { Code = "r510110" },
				Col = { Code = "Pd" },
				Time = { Year = 2013, Period = 251 },
				Obj = { Id = 354}, 
				Reference = {Contragents = "1788"},
			});
			Assert.AreEqual(905000, result.NumericResult);
		}

		[Test]
		[Explicit("��� ������ ��� ����� ��������")]
		public void CanEvalInGroupOfOtherSerial() {
			foreach (var rc in new[] { "f110230", "f110231", "f1102311", "f1102312", "f1102313", "f1102314" })
			{
				var q = new Query
					{
						Row = {Code = rc},
						Col = {Code = "PLAN"},
						Time = {Year = 2013, Period = 251},
						Obj = {Id = 354},
					};
				q = (Query) session.Register(q);
				var res = _serial.Eval(q);
				Console.WriteLine(res.NumericResult);
			}
		}
	}
}
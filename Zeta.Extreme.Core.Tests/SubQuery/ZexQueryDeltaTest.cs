using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Core;

namespace Zeta.Extreme.Core.Tests.SubQuery
{
	[TestFixture]
	public class ZexQueryDeltaTest
	{
		[Test]
		public void CanConvertToCSharp() {
			var d = new ZexQueryDelta { RowCode = "Y" , };
			Assert.AreEqual(" new Zeta.Extreme.ZexQueryDelta{ RowCode = \"Y\", }",d.ToCSharpString());
			Assert.AreEqual("Eval( new Zeta.Extreme.ZexQueryDelta{ RowCode = \"Y\", })", d.ToCSharpString("Eval"));
		}

		[Test]
		public void CanBeParsedFromRealWorldFormula() {
			var preprocessor = new DefaultDeltaPreprocessor();
			var result =
				preprocessor.Preprocess(@"f.If ( year == 2011 and periodin ( 4,112 ) and colin (""Б1"",""PLAN"") , { - $m1111216@DELTA? - $r2161111@Ok.P4? - $r2161131@Ok.P4? } , {
f.If ( year == 2011 or (  year == 2012 and periodin ( 301,251,252,303,306,309 ) ) , { f.If ( colin(""PLANC"") , { $m1111216@PLANC? }, { f.If ( colin (""Б1"",""PLAN""), { - $m1111216@DELTA? } ) } ) }, { f.If ( year < 2011 , { f.If ( colin(""PLANC"") , { $m211216@PLANC? }, { f.If ( colin (""Б1"",""PLAN""), { - $m211216@DELTA? } ) } ) } , { f.If ( colin (""Б1"",""PLAN""), { $r2161200@Rd? - $r2161200@Pd? } ) } ) } ) } )
", new FormulaRequest {Language = "boo"});
			Console.WriteLine(result);
			Assert.AreEqual(@"f.If ( year == 2011 and periodin ( 4,112 ) and colin (""Б1"",""PLAN"") , { - EvalDelta( new Zeta.Extreme.ZexQueryDelta{ RowCode = ""m1111216"", ColCode = ""DELTA"", }) - EvalDelta( new Zeta.Extreme.ZexQueryDelta{ RowCode = ""r2161111"", ColCode = ""Ok"", Period = 4, }) - EvalDelta( new Zeta.Extreme.ZexQueryDelta{ RowCode = ""r2161131"", ColCode = ""Ok"", Period = 4, }) } , {
f.If ( year == 2011 or (  year == 2012 and periodin ( 301,251,252,303,306,309 ) ) , { f.If ( colin(""PLANC"") , { EvalDelta( new Zeta.Extreme.ZexQueryDelta{ RowCode = ""m1111216"", ColCode = ""PLANC"", }) }, { f.If ( colin (""Б1"",""PLAN""), { - EvalDelta( new Zeta.Extreme.ZexQueryDelta{ RowCode = ""m1111216"", ColCode = ""DELTA"", }) } ) } ) }, { f.If ( year < 2011 , { f.If ( colin(""PLANC"") , { EvalDelta( new Zeta.Extreme.ZexQueryDelta{ RowCode = ""m211216"", ColCode = ""PLANC"", }) }, { f.If ( colin (""Б1"",""PLAN""), { - EvalDelta( new Zeta.Extreme.ZexQueryDelta{ RowCode = ""m211216"", ColCode = ""DELTA"", }) } ) } ) } , { f.If ( colin (""Б1"",""PLAN""), { EvalDelta( new Zeta.Extreme.ZexQueryDelta{ RowCode = ""r2161200"", ColCode = ""Rd"", }) - EvalDelta( new Zeta.Extreme.ZexQueryDelta{ RowCode = ""r2161200"", ColCode = ""Pd"", }) } ) } ) } ) } )".Trim(),result.Trim());
		}


		[Test]
		public void RowMove() {
			var q = new ZexQuery {Row = {Code = "X"}};
			var d = new ZexQueryDelta {RowCode = "Y"};
			var dq = d.Apply(q);
			Assert.AreNotSame(q,dq);
			Assert.AreSame(q.Col,dq.Col);
			Assert.AreNotSame(q.Row,dq.Row);
			Assert.AreEqual("Y",dq.Row.Code);
		}
		[Test]
		public void ColMove()
		{
			var q = new ZexQuery { Col = { Code = "X" } };
			var d = new ZexQueryDelta { ColCode = "Y" };
			var dq = d.Apply(q);
			Assert.AreNotSame(q, dq);
			Assert.AreSame(q.Row, dq.Row);
			Assert.AreNotSame(q.Col, dq.Col);
			Assert.AreEqual("Y", dq.Col.Code);
		}

		[Test]
		public void ObjMove()
		{
			var q = new ZexQuery { Obj = { Id = 24 } };
			var d = new ZexQueryDelta { ObjId = 35 };
			var dq = d.Apply(q);
			Assert.AreNotSame(q, dq);
			Assert.AreSame(q.Row, dq.Row);
			Assert.AreNotSame(q.Obj, dq.Obj);
			Assert.AreEqual(35, dq.Obj.Id);
		}

		[Test]
		public void YearMove()
		{
			var q = new ZexQuery { Time = { Year = 2012 } };
			var d = new ZexQueryDelta { Year = 1 };
			var dq = d.Apply(q);
			Assert.AreNotSame(q, dq);
			Assert.AreSame(q.Row, dq.Row);
			Assert.AreNotSame(q.Time, dq.Time);
			Assert.AreEqual(2013, dq.Time.Year);
		}

		[Test]
		public void YearPeriodMove()
		{
			var q = new ZexQuery { Time = { Year = 2012 } };
			var d = new ZexQueryDelta { Year = 2014,Period = 1};
			var dq = d.Apply(q);
			Assert.AreNotSame(q, dq);
			Assert.AreSame(q.Row, dq.Row);
			Assert.AreNotSame(q.Time, dq.Time);
			Assert.AreEqual(2014, dq.Time.Year);
			Assert.AreEqual(1, dq.Time.Period);
		}

		[Test]
		[Explicit]
		public void BenchMarkTest() {
			var sw = Stopwatch.StartNew();
			int i=0;
			for(var r = 0;r<500;r++) { //rowcodes
				for(var c = 0;c<10;c++) { //колонки
					for(var o=1;o<15;o++) { //объекты
						for (var y=2010;y<2014;y++) { //годы
							for(var p=0;p<10;p++) { //периоды
								i++;
								var q = new ZexQuery
									{
										Row = {Code = r.ToString()},
										Col = {Code = c.ToString()},
										Obj = {Id = o},
										Time = {Year = y, Period = p}
									};
								var d = new ZexQueryDelta
									{
										RowCode = r + "+",
										ColCode = c + "+",
										ObjId = o + 10,
										Year = y + 1,
										Period = p + 1
									};
								q = d.Apply(q);
								//Assert.AreEqual(r+"+",q.Row.Code);
								//Assert.AreEqual(c + "+", q.Col.Code);
								//Assert.AreEqual(o + 10, q.Obj.Id);
								//Assert.AreEqual(y+1,q.Time.Year);
								//Assert.AreEqual(p + 1, q.Time.Period);
							}
						}
					}
				}
			}
			sw.Stop();
			Console.WriteLine(i);
			Console.WriteLine(sw.Elapsed);
			Console.WriteLine(sw.ElapsedMilliseconds/(double)i);
		}
	}
}

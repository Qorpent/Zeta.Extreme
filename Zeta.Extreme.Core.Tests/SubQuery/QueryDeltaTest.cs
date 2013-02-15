#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : ZexQueryDeltaTest.cs
// Project: Zeta.Extreme.Core.Tests
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Diagnostics;
using NUnit.Framework;

namespace Zeta.Extreme.Core.Tests.SubQuery {
	[TestFixture]
	public class QueryDeltaTest {
		[Test]
		[Explicit]
		public void BenchMarkTest() {
			var sw = Stopwatch.StartNew();
			var i = 0;
			for (var r = 0; r < 500; r++) {
				//rowcodes
				for (var c = 0; c < 10; c++) {
					//колонки
					for (var o = 1; o < 15; o++) {
						//объекты
						for (var y = 2010; y < 2014; y++) {
							//годы
							for (var p = 0; p < 10; p++) {
								//периоды
								i++;
								var q = new Query
									{
										Row = {Code = r.ToString()},
										Col = {Code = c.ToString()},
										Obj = {Id = o},
										Time = {Year = y, Period = p}
									};
								var d = new QueryDelta
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
			Console.WriteLine(sw.ElapsedMilliseconds/(double) i);
		}

		[Test]
		public void CanConvertToCSharp() {
			var d = new QueryDelta {RowCode = "Y",};
			Assert.AreEqual(" new QueryDelta{ RowCode = \"Y\", }", d.ToCSharpString());
			Assert.AreEqual("Eval( new QueryDelta{ RowCode = \"Y\", })", d.ToCSharpString(true,"Eval"));
		}

		[Test]
		public void CanBuildSubTreeOnAllFormulaSplits() {
			
		}


		[Test]
		public void Can_Keep_Simple_Deltas() {
			var storage = new FormulaStorage();
			var request = new FormulaRequest
				{
					Language = "boo",
					Formula =
						@"f.choose ( $Ф2190р , $Ф2190ф )"
				};
			storage.Preprocess(request);

			Console.WriteLine(request.PreprocessedFormula);
			Assert.AreEqual(
				@"f.choose (  new QueryDelta{ RowCode = ""Ф2190р"", } ,  new QueryDelta{ RowCode = ""Ф2190ф"", } )"
					.Trim(), request.PreprocessedFormula.Trim());
		}


		[Test]
		public void ColMove() {
			var q = new Query {Col = {Code = "X"}};
			var d = new QueryDelta {ColCode = "Y"};
			var dq = d.Apply(q);
			Assert.AreNotSame(q, dq);
			Assert.AreSame(q.Row, dq.Row);
			Assert.AreNotSame(q.Col, dq.Col);
			Assert.AreEqual("Y", dq.Col.Code);
		}

		[Test]
		public void ObjMove() {
			var q = new Query {Obj = {Id = 24}};
			var d = new QueryDelta {ObjId = 35};
			var dq = d.Apply(q);
			Assert.AreNotSame(q, dq);
			Assert.AreSame(q.Row, dq.Row);
			Assert.AreNotSame(q.Obj, dq.Obj);
			Assert.AreEqual(35, dq.Obj.Id);
		}

		[Test]
		public void RowMove() {
			var q = new Query {Row = {Code = "X"}};
			var d = new QueryDelta {RowCode = "Y"};
			var dq = d.Apply(q);
			Assert.AreNotSame(q, dq);
			Assert.AreSame(q.Col, dq.Col);
			Assert.AreNotSame(q.Row, dq.Row);
			Assert.AreEqual("Y", dq.Row.Code);
		}

		[Test]
		public void YearMove() {
			var q = new Query {Time = {Year = 2012}};
			var d = new QueryDelta {Year = 1};
			var dq = d.Apply(q);
			Assert.AreNotSame(q, dq);
			Assert.AreSame(q.Row, dq.Row);
			Assert.AreNotSame(q.Time, dq.Time);
			Assert.AreEqual(2013, dq.Time.Year);
		}

		[Test]
		public void YearPeriodMove() {
			var q = new Query {Time = {Year = 2012}};
			var d = new QueryDelta {Year = 2014, Period = 1};
			var dq = d.Apply(q);
			Assert.AreNotSame(q, dq);
			Assert.AreSame(q.Row, dq.Row);
			Assert.AreNotSame(q.Time, dq.Time);
			Assert.AreEqual(2014, dq.Time.Year);
			Assert.AreEqual(1, dq.Time.Period);
		}
	}
}
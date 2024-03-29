﻿#region LICENSE
// Copyright 2007-2013 Qorpent Team - http://github.com/Qorpent
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// PROJECT ORIGIN: Zeta.Extreme.Core.Tests/QueryDeltaTest.cs
#endregion
using System;
using System.Diagnostics;
using NUnit.Framework;
using Zeta.Extreme.Model.Querying;

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
								q = (Query) d.Apply(q);
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
		public void Can_Use_Conto_In_Formulas_Deltas()
		{
			var storage = new FormulaStorage();
			var request = new FormulaRequest
			{
				Language = "boo",
				Formula =
					@"$X@Y.conto('Z')"
			};
			storage.Preprocess(request);

			Console.WriteLine(request.PreprocessedFormula);
			Assert.AreEqual(
				@"new QueryDelta{ RowCode = ""X"", ColCode = ""Y"", Types = ""Z"", }"
					.Trim(), request.PreprocessedFormula.Trim());
		}



		[Test]
		public void TypesMove()
		{
			var q = new Query { Reference = { Types = "X" } };
			var d = new QueryDelta { Types = "Y" };
			var dq = d.Apply(q);
			Assert.AreNotSame(q, dq);
			Assert.AreSame(q.Row, dq.Row);
			Assert.AreNotSame(q.Reference, dq.Reference);
			Assert.AreEqual("Y", dq.Reference.Types);
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

		[Test]
		public void Zc248AltObjFilterMove() {
			var q = new Query {Reference = {Contragents = ""}};
			var d = new QueryDelta {Contragents = "1878"};
			var dq = d.Apply(q);
			Assert.AreEqual("1878",dq.Reference.Contragents);
		}
	}
}
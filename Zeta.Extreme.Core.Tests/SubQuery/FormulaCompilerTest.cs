#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : FormulaCompilerTest.cs
// Project: Zeta.Extreme.Core.Tests
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using NUnit.Framework;

namespace Zeta.Extreme.Core.Tests.SubQuery {
	[TestFixture]
	public class FormulaCompilerTest {
		private string _formula =
			@"f.If ( year == 2011 and periodin ( 4,112 ) and colin ('Á1',""PLAN"") , { - $m1111216@DELTA? - $r2161111@Ok.P4? - $r2161131@Ok.P4? } , {
f.If ( year == 2011 or (  year == 2012 and periodin ( 301,251,252,303,306,309 ) ) , { f.If ( colin(""PLANC"") , { $m1111216@PLANC? }, { f.If ( colin (""Á1"",""PLAN""), { - $m1111216@DELTA? } ) } ) }, { f.If ( year < 2011 , { f.If ( colin(""PLANC"") , { $m211216@PLANC? }, { f.If ( colin (""Á1"",""PLAN""), { - $m211216@DELTA? } ) } ) } , { f.If ( colin (""Á1"",""PLAN""), { $r2161200@Rd? - $r2161200@Pd? } ) } ) } ) } )
";

		[Test]
		public void CanBeParsed_Deltas_And_Lambdas_FromRealWorldFormula() {
			var storage = new FormulaStorage();
			var request = new FormulaRequest
				{
					Language = "boo",
					Formula =
						_formula
				};
			storage.Preprocess(request);

			Console.WriteLine(request.PreprocessedFormula);
			Assert.AreEqual(@"f.If ( year == 2011 && periodin ( 4,112 ) && colin (""Á1"",""PLAN"") , ()=>( - EvalDelta( new Zeta.Extreme.QueryDelta{ RowCode = ""m1111216"", ColCode = ""DELTA"", }) - EvalDelta( new Zeta.Extreme.QueryDelta{ RowCode = ""r2161111"", ColCode = ""Ok"", Period = 4, }) - EvalDelta( new Zeta.Extreme.QueryDelta{ RowCode = ""r2161131"", ColCode = ""Ok"", Period = 4, }) ) , ()=>(
f.If ( year == 2011 || (  year == 2012 && periodin ( 301,251,252,303,306,309 ) ) , ()=>( f.If ( colin(""PLANC"") , ()=>( EvalDelta( new Zeta.Extreme.QueryDelta{ RowCode = ""m1111216"", ColCode = ""PLANC"", }) ), ()=>( f.If ( colin (""Á1"",""PLAN""), ()=>( - EvalDelta( new Zeta.Extreme.QueryDelta{ RowCode = ""m1111216"", ColCode = ""DELTA"", }) ) ) ) ) ), ()=>( f.If ( year < 2011 , ()=>( f.If ( colin(""PLANC"") , ()=>( EvalDelta( new Zeta.Extreme.QueryDelta{ RowCode = ""m211216"", ColCode = ""PLANC"", }) ), ()=>( f.If ( colin (""Á1"",""PLAN""), ()=>( - EvalDelta( new Zeta.Extreme.QueryDelta{ RowCode = ""m211216"", ColCode = ""DELTA"", }) ) ) ) ) ) , ()=>( f.If ( colin (""Á1"",""PLAN""), ()=>( EvalDelta( new Zeta.Extreme.QueryDelta{ RowCode = ""r2161200"", ColCode = ""Rd"", }) - EvalDelta( new Zeta.Extreme.QueryDelta{ RowCode = ""r2161200"", ColCode = ""Pd"", }) ) ) ) ) ) ) ) )
".Trim(), request.PreprocessedFormula.Trim());
		}

		[Test]
		public void CanBeParsed_Deltas_FromRealWorldFormula() {
			var preprocessor = new DefaultDeltaPreprocessor();
			var result =
				preprocessor.Preprocess(@"f.If ( year == 2011 and periodin ( 4,112 ) and colin (""Á1"",""PLAN"") , { - $m1111216@DELTA? - $r2161111@Ok.P4? - $r2161131@Ok.P4? } , {
f.If ( year == 2011 or (  year == 2012 and periodin ( 301,251,252,303,306,309 ) ) , { f.If ( colin(""PLANC"") , { $m1111216@PLANC? }, { f.If ( colin (""Á1"",""PLAN""), { - $m1111216@DELTA? } ) } ) }, { f.If ( year < 2011 , { f.If ( colin(""PLANC"") , { $m211216@PLANC? }, { f.If ( colin (""Á1"",""PLAN""), { - $m211216@DELTA? } ) } ) } , { f.If ( colin (""Á1"",""PLAN""), { $r2161200@Rd? - $r2161200@Pd? } ) } ) } ) } )
", new FormulaRequest {Language = "boo"});
			Console.WriteLine(result);
			Assert.AreEqual(
				@"f.If ( year == 2011 and periodin ( 4,112 ) and colin (""Á1"",""PLAN"") , { - EvalDelta( new Zeta.Extreme.QueryDelta{ RowCode = ""m1111216"", ColCode = ""DELTA"", }) - EvalDelta( new Zeta.Extreme.QueryDelta{ RowCode = ""r2161111"", ColCode = ""Ok"", Period = 4, }) - EvalDelta( new Zeta.Extreme.QueryDelta{ RowCode = ""r2161131"", ColCode = ""Ok"", Period = 4, }) } , {
f.If ( year == 2011 or (  year == 2012 and periodin ( 301,251,252,303,306,309 ) ) , { f.If ( colin(""PLANC"") , { EvalDelta( new Zeta.Extreme.QueryDelta{ RowCode = ""m1111216"", ColCode = ""PLANC"", }) }, { f.If ( colin (""Á1"",""PLAN""), { - EvalDelta( new Zeta.Extreme.QueryDelta{ RowCode = ""m1111216"", ColCode = ""DELTA"", }) } ) } ) }, { f.If ( year < 2011 , { f.If ( colin(""PLANC"") , { EvalDelta( new Zeta.Extreme.QueryDelta{ RowCode = ""m211216"", ColCode = ""PLANC"", }) }, { f.If ( colin (""Á1"",""PLAN""), { - EvalDelta( new Zeta.Extreme.QueryDelta{ RowCode = ""m211216"", ColCode = ""DELTA"", }) } ) } ) } , { f.If ( colin (""Á1"",""PLAN""), { EvalDelta( new Zeta.Extreme.QueryDelta{ RowCode = ""r2161200"", ColCode = ""Rd"", }) - EvalDelta( new Zeta.Extreme.QueryDelta{ RowCode = ""r2161200"", ColCode = ""Pd"", }) } ) } ) } ) } )"
					.Trim(), result.Trim());
		}

		[Test]
		public void CanCompileInBatchMode() {
			var storage = new FormulaStorage();
			var request = new FormulaRequest
				{
					Key = "test",
					Language = "boo",
					Formula = _formula
				};
			storage.Register(request);
			var request2 = new FormulaRequest
				{
					Key = "test2",
					Language = "boo",
					Formula = _formula + "/*2*/"
				};
			storage.Register(request2);
			var request3 = new FormulaRequest
				{
					Key = "test3",
					Language = "boo",
					Formula = _formula + "/*3*/"
				};
			storage.Register(request3);
			var request4 = new FormulaRequest
				{
					Key = "test4",
					Language = "boo",
					Formula = _formula + "/*4*/"
				};
			storage.Register(request4);
			var request5 = new FormulaRequest
				{
					Key = "test5",
					Language = "boo",
					Formula = _formula + "/*5*/"
				};
			storage.Register(request5);

			storage.GetFormula("test");
			Assert.NotNull(request.PreparedType);
			Assert.NotNull(request2.PreparedType);
			Assert.NotNull(request3.PreparedType);
			Assert.NotNull(request4.PreparedType);
			Assert.NotNull(request5.PreparedType);
			Assert.AreEqual(request.PreparedType.Assembly, request2.PreparedType.Assembly);
		}

		[Test]
		public void CanCompileInForcedMode() {
			var storage = new FormulaStorage();
			var request = new FormulaRequest
				{
					Key = "test",
					Language = "boo",
					Formula = _formula
				};
			storage.Register(request);
			var formula = storage.GetFormula("test");
			Assert.NotNull(formula);
		}

		[Test]
		public void CanCompileRealWorldFormula() {
			var storage = new FormulaStorage();
			var request = new FormulaRequest
				{
					Key = "test",
					Language = "boo",
					Formula = _formula
				};
			storage.Preprocess(request);

			Console.WriteLine(request.PreprocessedFormula);

			var compiler = new FormulaCompiler();
			compiler.Compile(new[] {request});
			Assert.NotNull(request.PreparedType);
		}
	}
}
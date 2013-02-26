#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : FormulaCompilerTest.cs
// Project: Zeta.Extreme.Core.Tests
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Linq;
using Comdiv.Application;
using Comdiv.Persistence;
using NUnit.Framework;
using Zeta.Extreme.Meta;
using Zeta.Extreme.Poco;

namespace Zeta.Extreme.Core.Tests.SubQuery {
	[TestFixture]
	public class FormulaCompilerTest {
		private string _formula =
			@"f.If ( year == 2011 and periodin ( 4,112 ) and colin ('Б1',""PLAN"") , { - $m1111216@DELTA? - $r2161111@Ok.P4? - $r2161131@Ok.P4? } , {
f.If ( year == 2011 or (  year == 2012 and periodin ( 301,251,252,303,306,309 ) ) , { f.If ( colin(""PLANC"") , { $m1111216@PLANC? }, { f.If ( colin (""Б1"",""PLAN""), { - $m1111216@DELTA? } ) } ) }, { f.If ( year < 2011 , { f.If ( colin(""PLANC"") , { $m211216@PLANC? }, { f.If ( colin (""Б1"",""PLAN""), { - $m211216@DELTA? } ) } ) } , { f.If ( colin (""Б1"",""PLAN""), { $r2161200@Rd? - $r2161200@Pd? } ) } ) } ) } )
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
			Assert.AreEqual(@"f.If ( year == 2011 && periodin ( 4,112 ) && colin (""Б1"",""PLAN"") , ()=>( - Eval( new QueryDelta{ RowCode = ""m1111216"", ColCode = ""DELTA"", }) - Eval( new QueryDelta{ RowCode = ""r2161111"", ColCode = ""Ok"", Period = 4, }) - Eval( new QueryDelta{ RowCode = ""r2161131"", ColCode = ""Ok"", Period = 4, }) ) , ()=>(
f.If ( year == 2011 || (  year == 2012 && periodin ( 301,251,252,303,306,309 ) ) , ()=>( f.If ( colin(""PLANC"") , ()=>( Eval( new QueryDelta{ RowCode = ""m1111216"", ColCode = ""PLANC"", }) ), ()=>( f.If ( colin (""Б1"",""PLAN""), ()=>( - Eval( new QueryDelta{ RowCode = ""m1111216"", ColCode = ""DELTA"", }) ) ) ) ) ), ()=>( f.If ( year < 2011 , ()=>( f.If ( colin(""PLANC"") , ()=>( Eval( new QueryDelta{ RowCode = ""m211216"", ColCode = ""PLANC"", }) ), ()=>( f.If ( colin (""Б1"",""PLAN""), ()=>( - Eval( new QueryDelta{ RowCode = ""m211216"", ColCode = ""DELTA"", }) ) ) ) ) ) , ()=>( f.If ( colin (""Б1"",""PLAN""), ()=>( Eval( new QueryDelta{ RowCode = ""r2161200"", ColCode = ""Rd"", }) - Eval( new QueryDelta{ RowCode = ""r2161200"", ColCode = ""Pd"", }) ) ) ) ) ) ) ) )
".Trim(), request.PreprocessedFormula.Trim());
		}

		[Test]
		public void Bug_Lost_Parsing()
		{
			var storage = new FormulaStorage();
			var request = new FormulaRequest
			{
				Language = "boo",
				Formula =
					@"f.If ( not colin( ""REVISION"" ), { $m111110? - $m111130? } )"
			};
			storage.Preprocess(request);

			Console.WriteLine(request.PreprocessedFormula);
			Assert.AreEqual(@"f.If ( ! colin( ""REVISION"" ), ()=>( Eval( new QueryDelta{ RowCode = ""m111110"", }) - Eval( new QueryDelta{ RowCode = ""m111130"", }) ) )
".Trim(), request.PreprocessedFormula.Trim());
		}

		[Test]
		public void ColumnFirstParsing()
		{
			var storage = new FormulaStorage();
			var request = new FormulaRequest
			{
				Language = "boo",
				Formula =
					@"f.If ( not colin( ""REVISION"" ), { @m111110? - @m111130? } )"
			};
			storage.Preprocess(request);

			Console.WriteLine(request.PreprocessedFormula);
			Assert.AreEqual(@"f.If ( ! colin( ""REVISION"" ), ()=>( Eval( new QueryDelta{ ColCode = ""m111110"", }) - Eval( new QueryDelta{ ColCode = ""m111130"", }) ) )
".Trim(), request.PreprocessedFormula.Trim());
		}

		[Test]
		public void ToObjParsing()
		{
			var storage = new FormulaStorage();
			var request = new FormulaRequest
			{
				Language = "boo",
				Formula =
					@"f.If ( not colin( ""REVISION"" ), { @m111110.toobj(345)? - $m111130.toobj(333)? } )"
			};
			storage.Preprocess(request);

			Console.WriteLine(request.PreprocessedFormula);
			Assert.AreEqual(@"f.If ( ! colin( ""REVISION"" ), ()=>( Eval( new QueryDelta{ ColCode = ""m111110"", ObjId = 345, }) - Eval( new QueryDelta{ RowCode = ""m111130"", ObjId = 333, }) ) )
".Trim(), request.PreprocessedFormula.Trim());
		}


		[Test]
		public void CanBeParsed_Deltas_FromRealWorldFormula() {
			var preprocessor = new DefaultDeltaPreprocessor();
			var result =
				preprocessor.Preprocess(@"f.If ( year == 2011 and periodin ( 4,112 ) and colin (""Б1"",""PLAN"") , { - $m1111216@DELTA? - $r2161111@Ok.P4? - $r2161131@Ok.P4? } , {
f.If ( year == 2011 or (  year == 2012 and periodin ( 301,251,252,303,306,309 ) ) , { f.If ( colin(""PLANC"") , { $m1111216@PLANC? }, { f.If ( colin (""Б1"",""PLAN""), { - $m1111216@DELTA? } ) } ) }, { f.If ( year < 2011 , { f.If ( colin(""PLANC"") , { $m211216@PLANC? }, { f.If ( colin (""Б1"",""PLAN""), { - $m211216@DELTA? } ) } ) } , { f.If ( colin (""Б1"",""PLAN""), { $r2161200@Rd? - $r2161200@Pd? } ) } ) } ) } )
", new FormulaRequest {Language = "boo"});
			Console.WriteLine(result);
			Assert.AreEqual(
				@"f.If ( year == 2011 and periodin ( 4,112 ) and colin (""Б1"",""PLAN"") , { - Eval( new QueryDelta{ RowCode = ""m1111216"", ColCode = ""DELTA"", }) - Eval( new QueryDelta{ RowCode = ""r2161111"", ColCode = ""Ok"", Period = 4, }) - Eval( new QueryDelta{ RowCode = ""r2161131"", ColCode = ""Ok"", Period = 4, }) } , {
f.If ( year == 2011 or (  year == 2012 and periodin ( 301,251,252,303,306,309 ) ) , { f.If ( colin(""PLANC"") , { Eval( new QueryDelta{ RowCode = ""m1111216"", ColCode = ""PLANC"", }) }, { f.If ( colin (""Б1"",""PLAN""), { - Eval( new QueryDelta{ RowCode = ""m1111216"", ColCode = ""DELTA"", }) } ) } ) }, { f.If ( year < 2011 , { f.If ( colin(""PLANC"") , { Eval( new QueryDelta{ RowCode = ""m211216"", ColCode = ""PLANC"", }) }, { f.If ( colin (""Б1"",""PLAN""), { - Eval( new QueryDelta{ RowCode = ""m211216"", ColCode = ""DELTA"", }) } ) } ) } , { f.If ( colin (""Б1"",""PLAN""), { Eval( new QueryDelta{ RowCode = ""r2161200"", ColCode = ""Rd"", }) - Eval( new QueryDelta{ RowCode = ""r2161200"", ColCode = ""Pd"", }) } ) } ) } ) } )"
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

		[Test]
		[Explicit]
		public void AllIsCompilable() {
			//myapp.ioc.Clear();
			//myapp.ioc.setupHibernate(
			//	new NamedConnection("Default",
			//						"Data Source=assoibdx;Initial Catalog=eco;Persist Security Info=True;User ID=sfo_home;Password=rhfcysq$0;Application Name=zeta3"),
			//	//"Data Source=(local);Initial Catalog=eco;Integrated Security=True;Min Pool Size=5;Application Name=zeta3"),
			//	new ZetaMinimalMode());
			Periods.Get(12);
			RowCache.start("m111", "m112", "m260", "m250", "m218", "r590", "m220");
			var storage = new FormulaStorage {AutoBatchCompile = false};
			var _sumh = new StrongSumProvider();
			var formulas = RowCache.Byid.Values.Where(_ => _.IsFormula && !_sumh.IsSum(_)).ToArray();
			bool fail = false;
			foreach (var f in formulas)
			{
				var req = new FormulaRequest { Key = f.Code, Formula = f.Formula, Language = f.FormulaEvaluator };
				storage.Register(req);
				try
				{
					storage.CompileAll(null);
				}
				catch (Exception e) {
					fail = true;
					Console.WriteLine(f.Code + ":" +f.Formula+" : " + e.Message);
					req.PreparedType = typeof(CompileErrorFormulaStub);
					req.ErrorInCompilation = e;
				}
			}

			var colformulas = (
								  from c in myapp.storage.AsQueryable<col>()
								  where c.IsFormula && c.FormulaEvaluator == "boo" && !c.Formula.Contains("~") && !c.Formula.Contains("*?") //unsupported feature
								  select new { c = c.Code, f = c.Formula, t =c.Tag }
							  ).ToArray();


			foreach (var c in colformulas)
			{
				var req = new FormulaRequest { Key = c.c, Formula = c.f, Language = "boo" ,Tags = c.t};
				storage.Register(req);
				try
				{
					storage.CompileAll(null);
				}
				catch (Exception e) {
					fail = true;
					Console.WriteLine(c.c + ":" +c.f+": " + e.Message);
					req.PreparedType = typeof(CompileErrorFormulaStub);
					req.ErrorInCompilation = e;
				}
			}

			if(fail)Assert.Fail("не все компилируется");
		}


		[Test]
		public void CanCheckNoExtremeTag() {
			var fs = new FormulaStorage();
			var req = new FormulaRequest {Key = "test", Formula = "xxx", Tags = "/noextreme:1/", Language = "boo"};
			var r = fs.Register(req);
			Assert.AreEqual(typeof(NoExtremeFormulaStub),req.PreparedType);
			var formula = fs.GetFormula(r);
			Assert.IsInstanceOf<NotSupportedException>(formula.Eval().Error);
		}

		[Test]
		public void CanMarkNotCompiledMark()
		{
			var fs = new FormulaStorage();
			var req = new FormulaRequest { Key = "test", Formula = "error code",  Language = "boo" };
			var r = fs.Register(req);
			var formula = fs.GetFormula(r);
			Assert.IsInstanceOf<CompileErrorFormulaStub>(formula);
			Assert.NotNull(formula.Eval().Error);
		}
	}

	
}
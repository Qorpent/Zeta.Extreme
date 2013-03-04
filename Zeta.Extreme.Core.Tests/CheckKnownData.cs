#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : CheckKnownData.cs
// Project: Zeta.Extreme.Core.Tests
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Zeta.Extreme.Core.Tests.CoreTests;
using Zeta.Extreme.Meta;
using Zeta.Extreme.Poco.NativeSqlBind;

namespace Zeta.Extreme.Core.Tests {
	[TestFixture]
	public class CheckKnownData : SessionTestBase {

	

		[TestCase("m260830", "PLAN", 1046, 2012, 301, 596254.16794090452660986449002)]
		[TestCase("m260", "PLAN", 1046, 2012, 301, 301749.000000)]
		[TestCase("m260100", "PLAN", 536, 2012, 301, 604812.000000)]
		[TestCase("m260110", "PLAN", 536, 2012, 301, 50.000000)]
		[TestCase("m260111", "PLAN", 467, 2012, 301, 465805.000000)]
		[TestCase("m260113", "PLAN", 536, 2012, 301, 50.000000)]
		[TestCase("m260130", "PLAN", 1046, 2012, 301, 1100.000000)]
		[TestCase("m260131", "PLAN", 536, 2012, 301, 199992.000000)]
		[TestCase("m2601311", "PLAN", 536, 2012, 301, 100493.000000)]
		[TestCase("m2601312", "PLAN", 536, 2012, 301, 45481.000000)]
		[TestCase("m2601313", "PLAN", 536, 2012, 301, 54018.000000)]
		[TestCase("m2601315", "PLAN", 449, 2012, 301, 2549.000000)]
		[TestCase("m2601316", "PLAN", 536, 2012, 301, 176294.000000)]
		[TestCase("m260132", "PLAN", 536, 2012, 301, 10185.000000)]
		[TestCase("m2601321", "PLAN", 536, 2012, 301, 5616.000000)]
		[TestCase("m260133", "PLAN", 467, 2012, 301, 15435.000000)]
		[TestCase("m2601331", "PLAN", 467, 2012, 301, 15435.000000)]
		[TestCase("m2601332", "PLAN", 352, 2012, 301, 762688.000000)]
		[TestCase("m2601333", "PLAN", 352, 2012, 301, 547023.000000)]
		[TestCase("m2601336", "PLAN", 352, 2012, 301, 15000.000000)]
		[TestCase("m260134", "PLAN", 1046, 2012, 301, 1100.000000)]
		[TestCase("m2601341", "PLAN", 1046, 2012, 301, 1100.000000)]
		[TestCase("m260135", "PLAN", 536, 2012, 301, 2513.000000)]
		[TestCase("m260136", "PLAN", 352, 2012, 301, 557397.000000)]
		[TestCase("m260139", "PLAN", 467, 2012, 301, 214.000000)]
		[TestCase("m2601391", "PLAN", 467, 2012, 301, 214.000000)]
		[TestCase("m260140", "PLAN", 1046, 2012, 301, 7248.000000)]
		[TestCase("m260150", "PLAN", 536, 2012, 301, 117589.000000)]
		[TestCase("m260160", "PLAN", 536, 2012, 301, 72694.000000)]
		[TestCase("m260170", "PLAN", 1046, 2012, 301, 2008.000000)]
		[TestCase("m260171", "PLAN", 536, 2012, 301, 167131.000000)]
		[TestCase("m260172", "PLAN", 467, 2012, 301, 12861.000000)]
		[TestCase("m260173", "PLAN", 536, 2012, 301, 1347.000000)]
		[TestCase("m260174", "PLAN", 536, 2012, 301, 249549.000000)]
		[TestCase("m260175", "PLAN", 352, 2012, 301, 1111.000000)]
		[TestCase("m260176", "PLAN", 352, 2012, 301, 197.000000)]
		[TestCase("m260177", "PLAN", 1046, 2012, 301, 2008.000000)]
		[TestCase("m260178", "PLAN", 536, 2012, 301, 3113.000000)]
		[TestCase("m260179", "PLAN", 536, 2012, 301, 500.000000)]
		[TestCase("m2601791", "PLAN", 536, 2012, 301, 500.000000)]
		[TestCase("m260180", "PLAN", 536, 2012, 301, 973088.000000)]
		[TestCase("m260181", "PLAN", 536, 2012, 301, 961630.000000)]
		[TestCase("m260182", "PLAN", 449, 2012, 301, 405.000000)]
		[TestCase("m260183", "PLAN", 536, 2012, 301, 9052.000000)]
		[TestCase("m260184", "PLAN", 536, 2012, 301, 2406.000000)]
		[TestCase("m260185", "PLAN", 467, 2012, 301, 125328.000000)]
		[TestCase("m2601851", "PLAN", 467, 2012, 301, 125328.000000)]
		[TestCase("m260200", "PLAN", 1046, 2012, 301, 229836.000000)]
		[TestCase("m260210", "PLAN", 1046, 2012, 301, 188151.000000)]
		[TestCase("m260220", "PLAN", 1046, 2012, 301, 138.000000)]
		[TestCase("m260240", "PLAN", 1046, 2012, 301, 41547.000000)]
		[TestCase("m260400", "PLAN", 1046, 2012, 301, 6220.000000)]
		[TestCase("m260410", "PLAN", 536, 2012, 301, 5.000000)]
		[TestCase("m260420", "PLAN", 1046, 2012, 301, 6220.000000)]
		[TestCase("m260500", "PLAN", 1046, 2012, 301, 55337.000000)]
		[TestCase("m260510", "PLAN", 1046, 2012, 301, 10581.000000)]
		[TestCase("m260511", "PLAN", 536, 2012, 301, 562.000000)]
		[TestCase("m260530", "PLAN", 1046, 2012, 301, 741.000000)]
		[TestCase("m260531", "PLAN", 536, 2012, 301, 21600.000000)]
		[TestCase("m260532", "PLAN", 536, 2012, 301, 6313.000000)]
		[TestCase("m260533", "PLAN", 536, 2012, 301, 516.000000)]
		[TestCase("m260534", "PLAN", 1046, 2012, 301, 631.000000)]
		[TestCase("m260535", "PLAN", 1046, 2012, 301, 110.000000)]
		[TestCase("m260539", "PLAN", 536, 2012, 301, 2760.000000)]
		[TestCase("m260540", "PLAN", 536, 2012, 301, 12418.000000)]
		[TestCase("m260550", "PLAN", 1046, 2012, 301, 10283.000000)]
		[TestCase("m260560", "PLAN", 1046, 2012, 301, 5312.000000)]
		[TestCase("m260561", "PLAN", 1046, 2012, 301, 1016.000000)]
		[TestCase("m260562", "PLAN", 467, 2012, 301, 33748.000000)]
		[TestCase("m260563", "PLAN", 1046, 2012, 301, 4040.000000)]
		[TestCase("m260564", "PLAN", 1046, 2012, 301, 256.000000)]
		[TestCase("m260565", "PLAN", 467, 2012, 301, 8936.000000)]
		[TestCase("m260589", "PLAN", 1046, 2012, 301, 28420.000000)]
		[TestCase("m260600", "PLAN", 1046, 2012, 301, 301749.000000)]
		[TestCase("m260700", "PLAN", 1046, 2012, 301, 3758.000000)]
		[TestCase("m260701", "PLAN", 536, 2012, 301, 73527.000000)]
		[TestCase("m260702", "PLAN", 536, 2012, 301, 10393.000000)]
		//[TestCase("m260703", "PLAN", 467, 2012, 301, 4469.000000)] seems to be changed at data level
		[TestCase("m260705", "PLAN", 449, 2012, 301, 80.000000)]
		[TestCase("m2607051", "PLAN", 536, 2012, 301, 6312.000000)]
		[TestCase("m260706", "PLAN", 536, 2012, 301, 36309.000000)]
		[TestCase("m2607061", "PLAN", 536, 2012, 301, 32757.000000)]
		[TestCase("m260708", "PLAN", 1046, 2012, 301, 240.000000)]
		[TestCase("m260709", "PLAN", 1046, 2012, 301, 3518.000000)]
		[TestCase("m260710", "PLAN", 536, 2012, 301, 117589.000000)]
		[TestCase("m260711", "PLAN", 449, 2012, 301, 30696.000000)]
		[TestCase("m260719", "PLAN", 536, 2012, 301, 172520.000000)]
		[TestCase("m260720", "PLAN", 467, 2012, 301, -316481.000000)]
		[TestCase("m260721", "PLAN", 467, 2012, 301, -264040.000000)]
		[TestCase("m260722", "PLAN", 467, 2012, 301, -52441.000000)]
		[TestCase("m260800", "PLAN", 1046, 2012, 301, 297991.000000)]
		[TestCase("m260801", "PLAN", 1046, 2012, 301, 17200466.000000)]
		[TestCase("m260810", "PLAN", 1046, 2012, 301, 17498457.000000)]
		[TestCase("m260811", "PLAN", 1046, 2012, 301, 11063.000000)]
		[TestCase("m260812", "PLAN", 1046, 2012, 301, 274147.000000)]
		[TestCase("m260815", "PLAN", 1046, 2012, 301, 17213247.000000)]
		[TestCase("m260824", "PLAN", 352, 2012, 301, 29303.000000)]
		[TestCase("m260826", "PLAN", 1046, 2012, 301, 17514311.000000)]
		[TestCase("m260827", "PLAN", 1046, 2012, 301, 15854.000000)]
		[TestCase("m260828", "PLAN", 1046, 2012, 301, 99.909480)]
		[TestCase("m260831", "PLAN", 536, 2012, 301, 604862.000000)]
		[TestCase("m2608311", "PLAN", 536, 2012, 301, 604812.000000)]
		[TestCase("m2608312", "PLAN", 536, 2012, 301, 50.000000)]
		[TestCase("m260832", "PLAN", 1046, 2012, 301, 7248.000000)]
		[TestCase("m260833", "PLAN", 1046, 2012, 301, 2008.000000)]
		[TestCase("m260834", "PLAN", 1046, 2012, 301, 1100.000000)]
		[TestCase("m2608341", "PLAN", 536, 2012, 301, 961630.000000)]
		[TestCase("m2608342", "PLAN", 536, 2012, 301, 167131.000000)]
		[TestCase("m2608344", "PLAN", 467, 2012, 301, 12861.000000)]
		[TestCase("m2608345", "PLAN", 536, 2012, 301, 250896.000000)]
		[TestCase("m260835", "PLAN", 1046, 2012, 301, 229836.000000)]
		[TestCase("m260836", "PLAN", 1046, 2012, 301, 6220.000000)]
		[TestCase("m260837", "PLAN", 1046, 2012, 301, 741.000000)]
		[TestCase("m260838", "PLAN", 1046, 2012, 301, 54596.000000)]
		[TestCase("m260600", "PLAN", 1046, 2012, 301, 301749.00000)]
		[TestCase("m260610", "PLAN", 1046, 2012, 301, 301749.000000)]
		[TestCase("m2608391", "PLAN", 1046, 2012, 301, 76.167941)]
		[TestCase("m260839", "PLAN", 1046, 2012, 301, 71913.000000)]
		[TestCase("m260530", "PLAN", 1046, 2012, 301, 741.00000)]
		[TestCase("m2608392", "PLAN", 1046, 2012, 301, 294429.000000)]
		[Timeout(2000)]
		public void CheckZatr_352_PLAN_301(string rowcode, string colcode, int obj, int year, int period, decimal checkvalue) {
			UnifiedStandardExistedValueTest(rowcode, colcode, obj, year, period, checkvalue);
		}

		[TestCase("m260130", "PLAN", 1046, 2012, 301, 1100.000000)]
		public void CheckZatr_352_1PLAN_301b(string rowcode, string colcode, int obj, int year, int period, decimal checkvalue) {
			session.TraceQuery = true;
			UnifiedStandardExistedValueTest(rowcode, colcode, obj, year, period, checkvalue);
		}

		[TestCase("m2601341", "PLAN", 1046, 2012, 301, 1100.000000)]
		[Timeout(4000)]
		public void Zatr_Problem_Zone(string rowcode, string colcode, int obj, int year, int period, decimal checkvalue) {
			var row = RowCache.get(rowcode);
			Console.WriteLine(row.Formula);
			var req = new FormulaRequest {Formula = row.Formula, Language = "boo"};
			new FormulaStorage().Preprocess(req);
			Console.WriteLine(req.PreprocessedFormula);
			foreach (var r in new StrongSumProvider().CollectSumDelta(row)) {
				Console.WriteLine(r.Row.Code);
			}
			UnifiedStandardExistedValueTest(rowcode, colcode, obj, year, period, checkvalue);
		}

		private void UnifiedStandardExistedValueTest(string rowcode, string colcode, int obj, int year, int period,
		                                             decimal checkvalue) {
			var q = new Query
				{Row = {Code = rowcode}, Col = {Code = colcode}, Obj = {Id = obj}, Time = {Year = year, Period = period}};
			var t = session.RegisterAsync(q);
			session.Execute();
			if (null != t.Result) {
				var result = ((Query)t.Result).GetResult().NumericResult;
				Console.WriteLine(result);
				if (checkvalue != result) {
					foreach (var query in session.Registry) {
						Console.WriteLine(query.Value.Row.Code + "\t" + query.Value.Result.NumericResult);
					}
				}
				Assert.AreEqual(Math.Round(checkvalue, 2), Math.Round(result, 2));
			}
			else {
				Assert.Ignore("filtered rows");
			}
		}


		[Test]
		public void ProblemWithm2608342HaveNullInResult() {
			var q =
				session.Register(new Query
					{
						Row = {Native = RowCache.get("m2608342")},
						Col = {Code = "Б1"},
						Obj = {Id = 352},
						Time = {Year = 2012, Period = 13}
					},"x");
			session.Execute();
			Assert.True(session.Registry.ContainsKey("x"));

		}


		[TestCase("m1122100", "PLAN", 1046, 2012, 301, 301064.000000)]
		[TestCase("m1122110", "PLAN", 1046, 2012, 301, 17514311.000000)]
		[TestCase("m1122120", "PLAN", 1046, 2012, 301, 17213247.000000)]
		[TestCase("m1122210", "PLAN", 1046, 2012, 301, 11063.000000)]
		[TestCase("m1122220", "PLAN", 1046, 2012, 301, 274147.000000)]
		[TestCase("m1122200", "PLAN", 1046, 2012, 301, 15854.000000)]
		[TestCase("m1122320", "PLAN", 357, 2012, 301, 110000.000000)]
		[TestCase("m1122330", "PLAN", 1046, 2012, 301, 1500.000000)]
		[TestCase("m1122340", "PLAN", 1046, 2012, 301, 2000.000000)]
		[TestCase("m1122350", "PLAN", 1046, 2012, 301, 11840.000000)]
		[TestCase("m1122300", "PLAN", 1046, 2012, 301, 4514.000000)]
		[TestCase("m1122410", "PLAN", 1046, 2012, 301, 2787.000000)]
		[TestCase("m1122400", "PLAN", 1046, 2012, 301, 1727.000000)]
		[TestCase("m1122500", "PLAN", 1046, 2012, 301, 1727.000000)]
		[TestCase("m212203", "PLAN", 536, 2012, 301, 823417.000000)]
		[TestCase("m212270", "PLAN", 1046, 2012, 301, 9420.000000)]
		public void Check_Prib_352_PLAN_301(string rowcode, string colcode, int obj, int year, int period, decimal checkvalue) {
			UnifiedStandardExistedValueTest(rowcode, colcode, obj, year, period, checkvalue);
		}

		private void MakeSingleCallOnCases(string methodname) {
			var sources =
				GetType().GetMethod(methodname).GetCustomAttributes(typeof (TestCaseAttribute), true).OfType
					<TestCaseAttribute>().OrderBy(_ => _.Arguments[0]).ToArray();
			session = new Session(true);
			session.TraceQuery = true;
			var i = 0;
			IDictionary<string, decimal> checkvalues = new Dictionary<string, decimal>();
			foreach (var s in sources) {
				i++;
				var r = (string) s.Arguments[0];
				var c = (string) s.Arguments[1];
				var o = (int) s.Arguments[2];
				var y = (int) s.Arguments[3];
				var p = (int) s.Arguments[4];
				var v = (decimal) ((double) s.Arguments[5]);
				var key = r;
				checkvalues[key] = v;
				var q = new Query {Row = {Code = r}, Col = {Code = c}, Obj = {Id = o}, Time = {Year = y, Period = p}};
				session.RegisterAsync(q, key);
			}
			session.Execute();
			var fail = false;
			foreach (var v in checkvalues) {
				Assert.True(session.Registry.ContainsKey(v.Key), v.Key);
				var res = session.Registry[v.Key];
				var equal = Math.Round(v.Value, 2) == Math.Round(res.GetResult().NumericResult, 2);
				if (!equal) {
					fail = true;
					Console.WriteLine(v.Key + " " + res.UID + " : " + Math.Round(v.Value, 2) + " : " +
					                  Math.Round(res.GetResult().NumericResult, 2));
					if (null != res.TraceList) {
						foreach (var s in res.TraceList) {
							Console.WriteLine(s);
						}
					}
				}
			}
			if (fail) {
				Assert.Fail("errors in comparison");
			}
		}

		private void MakeSingleCallOnCasesSerial(string methodname) {
			var sources =
				GetType().GetMethod(methodname).GetCustomAttributes(typeof (TestCaseAttribute), true).OfType
					<TestCaseAttribute>().ToArray();
			session = new Session(true);
			var serial = session.AsSerial();
			var i = 0;
			IDictionary<string, decimal> checkvalues = new Dictionary<string, decimal>();
			var fail = false;
			foreach (var s in sources) {
				i++;

				var r = (string) s.Arguments[0];
				var c = (string) s.Arguments[1];
				var o = (int) s.Arguments[2];
				var y = (int) s.Arguments[3];
				var p = (int) s.Arguments[4];
				var v = (decimal) ((double) s.Arguments[5]);
				var q = new Query {Row = {Code = r}, Col = {Code = c}, Obj = {Id = o}, Time = {Year = y, Period = p}};
				var res = serial.Eval(q);
				var equal = Math.Round(v, 2) == Math.Round(res.NumericResult, 2);
				if (!equal) {
					fail = true;
					Console.WriteLine(r + " : " + Math.Round(v, 2) + " : " + Math.Round(res.NumericResult, 2));
				}
			}

			if (fail) {
				Assert.Fail("errors in comparison");
			}
		}

		[TestCase("m111110", "PLAN", 1046, 2012, 301, 361453.000000)]
		[TestCase("m1111100", "PLAN", 467, 2012, 301, 20033235.000000)]
		[TestCase("m1111110", "PLAN", 467, 2012, 301, 11990.000000)]
		[TestCase("m1111111", "PLAN", 467, 2012, 301, 11990.000000)]
		[TestCase("m1111130", "PLAN", 467, 2012, 301, 19788526.000000)]
		[TestCase("m1111131", "PLAN", 467, 2012, 301, 8078479.000000)]
		[TestCase("m1111132", "PLAN", 467, 2012, 301, 11710047.000000)]
		[TestCase("m1111154", "PLAN", 467, 2012, 301, 11710047.000000)]
		[TestCase("m1111140", "PLAN", 467, 2012, 301, 11586.000000)]
		[TestCase("m1111150", "PLAN", 467, 2012, 301, 221023.000000)]
		[TestCase("m1111160", "PLAN", 467, 2012, 301, 110.000000)]
		[TestCase("m1111200", "PLAN", 1046, 2012, 301, 361453.000000)]
		[TestCase("m1111210", "PLAN", 1046, 2012, 301, 303733.000000)]
		[TestCase("m1111213", "PLAN", 536, 2012, 301, 286005.000000)]
		[TestCase("m1111214", "PLAN", 1046, 2012, 301, 302279.000000)]
		[TestCase("m1111215", "PLAN", 536, 2012, 301, 117604.000000)]
		[TestCase("m1111216", "PLAN", 1046, 2012, 301, 1454.000000)]
		[TestCase("m1111218", "PLAN", 1046, 2012, 301, 302279.000000)]
		[TestCase("m1111220", "PLAN", 467, 2012, 301, 144291.000000)]
		[TestCase("m1111230", "PLAN", 467, 2012, 301, 2012264.000000)]
		[TestCase("m1111231", "PLAN", 467, 2012, 301, 1617922.000000)]
		[TestCase("m1111240", "PLAN", 467, 2012, 301, 202948.000000)]
		[TestCase("m1111250", "PLAN", 1046, 2012, 301, 57720.000000)]
		[TestCase("m111130", "PLAN", 1046, 2012, 301, 305460.000000)]
		[TestCase("m1111300", "PLAN", 1046, 2012, 301, 1727.000000)]
		[TestCase("m1111310", "PLAN", 467, 2012, 301, 309.000000)]
		[TestCase("m1111350", "PLAN", 467, 2012, 301, 1041240.000000)]
		[TestCase("m1111360", "PLAN", 467, 2012, 301, 15.000000)]
		[TestCase("m1111370", "PLAN", 1046, 2012, 301, 1727.000000)]
		[TestCase("m1111371", "PLAN", 467, 2012, 301, 16553061.000000)]
		[TestCase("m1111372", "PLAN", 1046, 2012, 301, 1727.000000)]
		[TestCase("m1111400", "PLAN", 467, 2012, 301, 3345710.000000)]
		[TestCase("m1111410", "PLAN", 467, 2012, 301, 2936283.000000)]
		[TestCase("m1111420", "PLAN", 467, 2012, 301, 409427.000000)]
		[TestCase("m1111500", "PLAN", 1046, 2012, 301, 303733.000000)]
		[TestCase("m1111510", "PLAN", 467, 2012, 301, 2154277.000000)]
		[TestCase("m1111520", "PLAN", 1046, 2012, 301, 303733.000000)]
		[TestCase("m1111521", "PLAN", 1046, 2012, 301, 303733.000000)]
		[TestCase("m1111522", "PLAN", 467, 2012, 301, 140259.000000)]
		[TestCase("m1111523", "PLAN", 467, 2012, 301, 55851.000000)]
		[TestCase("m1111524", "PLAN", 467, 2012, 301, 177331.000000)]
		[TestCase("m1111525", "PLAN", 467, 2012, 301, 61052.000000)]
		[TestCase("m1111530", "PLAN", 467, 2012, 301, 3795.000000)]
	//	[TestCase("m111800", "PLAN", 1046, 2012, 301, 113713.000000)] // непонятное поведение
		[TestCase("m111801", "PLAN", 1046, 2012, 301, 55993.000000)]
		public void Check_Balans_MIX_PLAN_301(string rowcode, string colcode, int obj, int year, int period,
		                                      decimal checkvalue) {
			var t = Task.Run(() => UnifiedStandardExistedValueTest(rowcode, colcode, obj, year, period, checkvalue));
			t.Wait(1000);
		}

		[TestCase("m111800", "PLAN", 1046, 2012, 301, 113713.000000)]
		[TestCase("m111801", "PLAN", 1046, 2012, 301, 55993.000000)]
		[TestCase("m111802", "PLAN", 1046, 2012, 301, 57720.000000)] //obsolete
		[Ignore("что-то не то с входными данными")]
		public void Check_Balans_BUG_PLAN_301(string rowcode, string colcode, int obj, int year, int period,
		                                      decimal checkvalue) {
			UnifiedStandardExistedValueTest(rowcode, colcode, obj, year, period, checkvalue);
		}

		[Test]
		public void CheckZatr_352_PLAN_301_Single_Call() {
			MakeSingleCallOnCases("CheckZatr_352_PLAN_301");
		}

		[Test]
		public void CheckZatr_352_PLAN_301_Single_Call_Serial() {
			MakeSingleCallOnCasesSerial("CheckZatr_352_PLAN_301");
		}

		[Test]
		public void Check_Balans_MIX_PLAN_301_Single_Call() {
			MakeSingleCallOnCases("Check_Balans_MIX_PLAN_301");
		}

		[Test]
		public void Check_Prib_352_PLAN_301_Single_Call() {
			MakeSingleCallOnCases("Check_Prib_352_PLAN_301");
		}

		//[TestCase("m260531", "PLAN", 536, 2012, 301, 21600.000000)]
	}
}
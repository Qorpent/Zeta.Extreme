#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : QueryCacheKeySensibilityTest.cs
// Project: Zeta.Extreme.Core.Tests
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Linq;
using Comdiv.Zeta.Data.Minimal;
using Comdiv.Zeta.Model;
using NUnit.Framework;
using Qorpent.Utils.Extensions;

namespace Zeta.Extreme.Core.Tests.CoreTests {
	[TestFixture(Description = "Проверка корректности и уникальности кэш строк")]
	public class QueryCacheKeySensibilityTest : SessionTestBase {
		public enum ManyPart {
			RowIds,
			RowCodes,
			ColIds,
			ColCodes,
			ObjIds,
			ObjCodes,
			Years,
			Periods
		}

		public enum ManyPartTestType {
			NotOrder,
			NotEqual,
			Ambigous,
			Nulltest,
		}

		[TestCase(-1, 2013, -204, 12, 1, 2011, 0, 11, true)]
		public void TimeNormalizationTest(
			int fstyear, int fstbaseyear, int fstperiod, int fstbaseperiod,
			int secondyear, int secondbaseyear, int secondperiod, int secondbaseperiod,
			bool areequal) {
			var q1 = new Query
				{
					Time = {Year = fstyear, BaseYear = fstbaseyear, Period = fstperiod, BasePeriod = fstbaseperiod},
					Row = {Code = "m260113"},
					Col = {Code = "Б1"},
					Obj = {Id = 352, Type = ObjType.Obj}
				};
			var q2 = new Query
				{
					Time = {Year = secondyear, BaseYear = secondbaseyear, Period = secondperiod, BasePeriod = secondbaseperiod},
					Row = {Code = "m260113"},
					Col = {Code = "Б1"},
					Obj = {Id = 352, Type = ObjType.Obj}
				};
			q1.Normalize();
			q2.Normalize();
			Console.WriteLine(q1.GetCacheKey());
			Console.WriteLine();
			Console.WriteLine(q2.GetCacheKey());
			Assert.AreEqual(areequal, q1.GetCacheKey() == q2.GetCacheKey());
		}

		[Test]
		public void HardLinkAndSingleRowTest() {
			var target = RowCache.get("m260113");
			var r = new row {Code = "x1", RefTo = target};
			RowCache.RegisterCustom(r);
			var r2 = new row {Code = "x2", Formula = " $x1? ", IsFormula = true, FormulaEvaluator = "boo"};
			RowCache.RegisterCustom(r2);

			var q1 = new Query {Row = {Code = "x1"}};
			var q2 = new Query {Row = {Code = "m260113"}};
			var q3 = new Query {Row = {Code = "x2"}};

			q1.Normalize();
			q2.Normalize();
			q3.Normalize();


			var key1 = q1.GetCacheKey();
			var key2 = q2.GetCacheKey();
			var key3 = q2.GetCacheKey();

			Console.WriteLine(key1);
			Console.WriteLine(key2);
			Console.WriteLine(key3);

			Assert.AreEqual(key1, key2);
			Assert.AreEqual(key2, key3);
		}

		[Test]
		[Combinatorial]
		[Description("Тест проверяет валидность формирования и уникальность кэш строк по множественным параметрам запроса")]
		public void ManyOrderTest(
			[Values(ManyPart.RowIds, ManyPart.RowCodes, ManyPart.ColCodes, ManyPart.ObjIds, ManyPart.ObjCodes, ManyPart.Years,
				ManyPart.Periods)] ManyPart part,
			[Values(ManyPartTestType.Nulltest, ManyPartTestType.NotOrder, ManyPartTestType.NotEqual, ManyPartTestType.Ambigous)] ManyPartTestType testtype) {
			if (testtype == ManyPartTestType.Nulltest &&
			    new[] {ManyPart.RowIds, ManyPart.ObjIds, ManyPart.Years, ManyPart.Periods}.Any(_ => _ == part)) {
				Assert.Ignore("cannot test nullable on integers");
			}
			var baseset = new[] {"2001", "2002", "2003", "2004", "2005"}; //avoid not defined year
			string[] otherset = null;
			var checkCondition = false;
			switch (testtype) {
				case ManyPartTestType.NotOrder:
					otherset = baseset.OrderByDescending(_ => _).ToArray();
					checkCondition = true;
					break;
				case ManyPartTestType.NotEqual:
					otherset = baseset.Skip(1).ToArray();
					break;
				case ManyPartTestType.Ambigous:
					var l = baseset.ToList();
					l.Add("2002");
					l.Add("2003");
					otherset = l.ToArray();
					checkCondition = true;
					break;
				case ManyPartTestType.Nulltest:
					otherset = baseset.Select(_ => _ == "2002" ? null : _).ToArray();
					break;
			}

			var fst = new Query();
			var sec = new Query();

			switch (part) {
				case ManyPart.Periods:
					fst.Time.Periods = baseset.Select(_ => _.ToInt()).ToArray();
					sec.Time.Periods = otherset.Select(_ => _.ToInt()).ToArray();
					break;
				case ManyPart.Years:
					fst.Time.Years = baseset.Select(_ => _.ToInt()).ToArray();
					sec.Time.Years = otherset.Select(_ => _.ToInt()).ToArray();
					break;
				case ManyPart.ObjIds:
					fst.Obj.Ids = baseset.Select(_ => _.ToInt()).ToArray();
					sec.Obj.Ids = otherset.Select(_ => _.ToInt()).ToArray();
					break;
				case ManyPart.RowIds:
					fst.Row.Ids = baseset.Select(_ => _.ToInt()).ToArray();
					sec.Row.Ids = otherset.Select(_ => _.ToInt()).ToArray();
					break;
				case ManyPart.ColIds:
					fst.Col.Ids = baseset.Select(_ => _.ToInt()).ToArray();
					sec.Col.Ids = otherset.Select(_ => _.ToInt()).ToArray();
					break;

				case ManyPart.ObjCodes:
					fst.Obj.Codes = baseset;
					sec.Obj.Codes = otherset;
					break;
				case ManyPart.RowCodes:
					fst.Row.Codes = baseset;
					sec.Row.Codes = otherset;
					break;
				case ManyPart.ColCodes:
					fst.Col.Codes = baseset;
					sec.Col.Codes = otherset;
					break;
			}
			var key1 = fst.GetCacheKey();
			var key2 = sec.GetCacheKey();
			Console.WriteLine("-- {0} {1} --", part, testtype);
			Console.WriteLine(key1);
			Console.WriteLine();
			Console.WriteLine(key2);
			Console.WriteLine("-------------------------");
			Assert.AreEqual(checkCondition, key1 == key2);
		}
	}
}
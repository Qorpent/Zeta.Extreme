﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Zeta.Extreme.Core.Tests.CoreTests;

namespace Zeta.Extreme.Core.Tests
{
	[TestFixture]
	public class CheckKnownData : SessionTestBase
	{
		[TestCase("m260", "PLAN", 352, 2012, 301, 17349543.000000)]
		[TestCase("m260100", "PLAN", 352, 2012, 301, 1820796.000000)]
		[TestCase("m260110", "PLAN", 352, 2012, 301, 143281.000000)]
		[TestCase("m260111", "PLAN", 352, 2012, 301, 141415.000000)]
		[TestCase("m260112", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260113", "PLAN", 352, 2012, 301, 1866.000000)]
		[TestCase("m260120", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260130", "PLAN", 352, 2012, 301, 4453656.000000)]
		[TestCase("m260131", "PLAN", 352, 2012, 301, 436626.000000)]
		[TestCase("m2601311", "PLAN", 352, 2012, 301, 422204.000000)]
		[TestCase("m2601312", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m2601313", "PLAN", 352, 2012, 301, 14422.000000)]
		[TestCase("m2601315", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m2601316", "PLAN", 352, 2012, 301, 105214.000000)]
		[TestCase("m260132", "PLAN", 352, 2012, 301, 376047.000000)]
		[TestCase("m2601321", "PLAN", 352, 2012, 301, 32455.000000)]
		[TestCase("m260133", "PLAN", 352, 2012, 301, 1329730.000000)]
		[TestCase("m2601331", "PLAN", 352, 2012, 301, 5019.000000)]
		[TestCase("m2601332", "PLAN", 352, 2012, 301, 762688.000000)]
		[TestCase("m2601333", "PLAN", 352, 2012, 301, 547023.000000)]
		[TestCase("m2601334", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m2601335", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m2601336", "PLAN", 352, 2012, 301, 15000.000000)]
		[TestCase("m260134", "PLAN", 352, 2012, 301, 280640.000000)]
		[TestCase("m2601341", "PLAN", 352, 2012, 301, 280640.000000)]
		[TestCase("m260135", "PLAN", 352, 2012, 301, 1459216.000000)]
		[TestCase("m260136", "PLAN", 352, 2012, 301, 557397.000000)]
		[TestCase("m260139", "PLAN", 352, 2012, 301, 14000.000000)]
		[TestCase("m260_1062", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260_1070", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m2601_1095", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260139.2_1067", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260139.2_1165", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260139.3_1070", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260139.3_1071", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260139.4_1070", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260139.4_1071", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260139.5_1070", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260139.6_1071", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260139_1057", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260139_1071", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m2601391", "PLAN", 352, 2012, 301, 14000.000000)]
		[TestCase("m2601392_1070", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260140_1099", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260140_539", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m2602_1095", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m2603_1095", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260392_1057", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260m260139_1062", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260m2601391_473", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260m2601392_1022", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260m2601393_1022", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260140", "PLAN", 352, 2012, 301, 1574823.000000)]
		[TestCase("m260150", "PLAN", 352, 2012, 301, 230673.000000)]
		[TestCase("m260160", "PLAN", 352, 2012, 301, 125735.000000)]
		[TestCase("m260170", "PLAN", 352, 2012, 301, 1319511.000000)]
		[TestCase("m260171", "PLAN", 352, 2012, 301, 492489.000000)]
		[TestCase("m260172", "PLAN", 352, 2012, 301, 23331.000000)]
		[TestCase("m260173", "PLAN", 352, 2012, 301, 509233.000000)]
		[TestCase("m260174", "PLAN", 352, 2012, 301, 1331.000000)]
		[TestCase("m260175", "PLAN", 352, 2012, 301, 1111.000000)]
		[TestCase("m260176", "PLAN", 352, 2012, 301, 197.000000)]
		[TestCase("m260177", "PLAN", 352, 2012, 301, 291819.000000)]
		[TestCase("m260178", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260179", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m2601791", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260180", "PLAN", 352, 2012, 301, 991922.000000)]
		[TestCase("m260181", "PLAN", 352, 2012, 301, 937288.000000)]
		[TestCase("m260182", "PLAN", 352, 2012, 301, 17399.000000)]
		[TestCase("m260183", "PLAN", 352, 2012, 301, 12169.000000)]
		[TestCase("m260184", "PLAN", 352, 2012, 301, 25066.000000)]
		[TestCase("m260185", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m2601851", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260200", "PLAN", 352, 2012, 301, 3860617.000000)]
		[TestCase("m260210", "PLAN", 352, 2012, 301, 2906009.000000)]
		[TestCase("m260220", "PLAN", 352, 2012, 301, 21708.000000)]
		[TestCase("m260240", "PLAN", 352, 2012, 301, 932900.000000)]
		[TestCase("m260400", "PLAN", 352, 2012, 301, 1144843.000000)]
		[TestCase("m260410", "PLAN", 352, 2012, 301, 38088.000000)]
		[TestCase("m260420", "PLAN", 352, 2012, 301, 1106755.000000)]
		[TestCase("m260500", "PLAN", 352, 2012, 301, 1683686.000000)]
		[TestCase("m260510", "PLAN", 352, 2012, 301, 112176.000000)]
		[TestCase("m260511", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260515", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260516", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260530", "PLAN", 352, 2012, 301, 271551.000000)]
		[TestCase("m260531", "PLAN", 352, 2012, 301, 16995.000000)]
		[TestCase("m260532", "PLAN", 352, 2012, 301, 54137.000000)]
		[TestCase("m260533", "PLAN", 352, 2012, 301, 35792.000000)]
		[TestCase("m260534", "PLAN", 352, 2012, 301, 160000.000000)]
		[TestCase("m260535", "PLAN", 352, 2012, 301, 4500.000000)]
		[TestCase("m260539", "PLAN", 352, 2012, 301, 127.000000)]
		[TestCase("m260540", "PLAN", 352, 2012, 301, 107808.000000)]
		[TestCase("m260550", "PLAN", 352, 2012, 301, 16030.000000)]
		[TestCase("m260560", "PLAN", 352, 2012, 301, 448857.000000)]
		[TestCase("m260561", "PLAN", 352, 2012, 301, 317921.000000)]
		[TestCase("m260562", "PLAN", 352, 2012, 301, 906.000000)]
		[TestCase("m260563", "PLAN", 352, 2012, 301, 44416.000000)]
		[TestCase("m260564", "PLAN", 352, 2012, 301, 85614.000000)]
		[TestCase("m260565", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260589", "PLAN", 352, 2012, 301, 727264.000000)]
		[TestCase("m260600", "PLAN", 352, 2012, 301, 17349543.000000)]
		[TestCase("m260610", "PLAN", 352, 2012, 301, 15528747.000000)]
		[TestCase("m260700", "PLAN", 352, 2012, 301, 732456.000000)]
		[TestCase("m260701", "PLAN", 352, 2012, 301, 39045.000000)]
		[TestCase("m260702", "PLAN", 352, 2012, 301, 39400.000000)]
		[TestCase("m260703", "PLAN", 352, 2012, 301, 13829.000000)]
		[TestCase("m260705", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m2607051", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260706", "PLAN", 352, 2012, 301, 156200.000000)]
		[TestCase("m2607061", "PLAN", 352, 2012, 301, 33722.000000)]
		[TestCase("m260707", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260708", "PLAN", 352, 2012, 301, 8484.000000)]
		[TestCase("m260709", "PLAN", 352, 2012, 301, 22033.000000)]
		[TestCase("m260710", "PLAN", 352, 2012, 301, 253623.000000)]
		[TestCase("m260711", "PLAN", 352, 2012, 301, 143309.000000)]
		[TestCase("m260712", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260718", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260719", "PLAN", 352, 2012, 301, 56533.000000)]
		[TestCase("m260720", "PLAN", 352, 2012, 301, 945538.000000)]
		[TestCase("m260721", "PLAN", 352, 2012, 301, 945538.000000)]
		[TestCase("m260722", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260723", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260724", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260725", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260726", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260800", "PLAN", 352, 2012, 301, 17562625.000000)]
		[TestCase("m260801", "PLAN", 352, 2012, 301, 50000.000000)]
		[TestCase("m260802", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260803", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260804", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260805", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260806", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260810", "PLAN", 352, 2012, 301, 17612625.000000)]
		[TestCase("m260811", "PLAN", 352, 2012, 301, 315150.000000)]
		[TestCase("m260812", "PLAN", 352, 2012, 301, 2180000.000000)]
		[TestCase("m260815", "PLAN", 352, 2012, 301, 15117475.000000)]
		[TestCase("m260820", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260821", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260822", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260823", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260824", "PLAN", 352, 2012, 301, 29303.000000)]
		[TestCase("m260825", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260826", "PLAN", 352, 2012, 301, 22082187.000000)]
		[TestCase("m260827", "PLAN", 352, 2012, 301, 4469562.000000)]
		[TestCase("m260828", "PLAN", 352, 2012, 301, 79.759423)]
		[TestCase("m260830", "PLAN", 352, 2012, 301, 30990679.861098)]
		[TestCase("m260831", "PLAN", 352, 2012, 301, 1964077.000000)]
		[TestCase("m2608311", "PLAN", 352, 2012, 301, 1820796.000000)]
		[TestCase("m2608312", "PLAN", 352, 2012, 301, 143281.000000)]
		[TestCase("m260832", "PLAN", 352, 2012, 301, 1931231.000000)]
		[TestCase("m260833", "PLAN", 352, 2012, 301, 2311433.000000)]
		[TestCase("m2608341", "PLAN", 352, 2012, 301, 937288.000000)]
		[TestCase("m2608342", "PLAN", 352, 2012, 301, 492489.000000)]
		[TestCase("m2608343", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m2608344", "PLAN", 352, 2012, 301, 23331.000000)]
		[TestCase("m2608345", "PLAN", 352, 2012, 301, 510564.000000)]
		[TestCase("m260834", "PLAN", 352, 2012, 301, 4453656.000000)]
		[TestCase("m260835", "PLAN", 352, 2012, 301, 3860617.000000)]
		[TestCase("m260836", "PLAN", 352, 2012, 301, 1144843.000000)]
		[TestCase("m260837", "PLAN", 352, 2012, 301, 271551.000000)]
		[TestCase("m260838", "PLAN", 352, 2012, 301, 1412135.000000)]
		[TestCase("m260839", "PLAN", 352, 2012, 301, 11668130.000000)]
		[TestCase("m2608391", "PLAN", 352, 2012, 301, 24.861098)]
		[TestCase("m2608392", "PLAN", 352, 2012, 301, 13641112.000000)]
		[TestCase("m260900", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("k260711", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260920", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260930", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260940", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260950", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260970", "PLAN", 352, 2012, 301, 0.000000)]
		[TestCase("m260980", "PLAN", 352, 2012, 301, 0.000000)]
		public void TestsExistedData(string rowcode, string colcode, int obj, int year, int period, decimal checkvalue) {
			var q = new ZexQuery
				{Row = {Code = rowcode}, Col = {Code = colcode}, Obj = {Id = obj}, Time = {Year = year, Period = period}};
			var t  = session.RegisterAsync(q);
			session.WaitPreparation();
			session.WaitEvaluation();
			var result = t.Result.GetResult().NumericResult;
			Console.WriteLine(result);
			if(checkvalue!=result) {
				foreach (var query in session.MainQueryRegistry) {
					Console.WriteLine(query.Value.Row.Code +"\t"+ query.Value.Result.NumericResult);
					
				}
			}
			Assert.AreEqual(checkvalue,result);
		}
	}
}

using System;
using System.Collections.Generic;
using NUnit.Framework;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.MetaCaches;

namespace Zeta.Extreme.Core.Tests
{
	/// <summary>
	/// Проверяем нормализацию AltObjFilter
	/// </summary>
	[TestFixture]
	public class Zc432AltObjFilterNormalization
	{
		private Session _session;
		private MetaCache _metacache;
		private Division _divA;
		private Division _divB;
		private Obj _obj1;
		private Obj _obj2;
		private Obj _obj3;

		[SetUp]
		public void setup() {
			_session = new Session(true);
			_metacache = new MetaCache();
			_session.MetaCache = _metacache;
			_divA = new Division{Code="A"};
			_divB = new Division{Code="B"};
			_obj1 = new Obj {Id = 1, Division = _divA, GroupCache ="/A/"};
			_obj2 = new Obj {Id = 2, Division = _divA, GroupCache ="/B/"};
			_obj3 = new Obj {Id = 3, Division = _divB, GroupCache ="/A/B/"};
			_divA.MainObjects = _divA.MainObjects ?? new List<IZetaMainObject>();
			_divB.MainObjects = _divB.MainObjects ?? new List<IZetaMainObject>();
			_divA.MainObjects.Add(_obj1);
			_divA.MainObjects.Add(_obj2);
			_divB.MainObjects.Add(_obj3);
			ObjCache.DivByCode["A"] = _divA;
			ObjCache.DivByCode["B"] = _divB;
			ObjCache.ObjById[1] = _obj1;
			ObjCache.ObjById[2] = _obj2;
			ObjCache.ObjById[3] = _obj3;
		}

		[TestCase("zdiv_A,3,2", true)]
		[TestCase("div_A,3,2",false)]
		[TestCase("1f,", true)]
		[TestCase("1,", false)]
		[TestCase("1,2", false)]
		[TestCase("1",false)]
		public void OnlyMatchedFormatSupported(string sample, bool throwerror) {
			var objh = new ReferenceHandler {Contragents = sample};
			if (throwerror) {
				Assert.Throws<FormatException>(() => objh.Normalize(null));
			}
			else {
				objh.Normalize(null);
			}
		}

		[TestCase("grp_B,1", "1,2,3", Description = "mix")]
		[TestCase("grp_B", "2,3", Description = "by grp B")]
		[TestCase("grp_A", "1,3", Description = "by grp A")]
		[TestCase("div_B", "3", Description = "by div B")]
		[TestCase("div_A", "1,2", Description = "by div A")]
		[TestCase("0", "", Description = "empties (obj 0)")]
		[TestCase("0,1,2,3", "1,2,3", Description = "empties (obj 0)")]
		[TestCase("1,2,2,2,3", "1,2,3", Description = "doubles")]
		[TestCase("  3,, 1, , 2,", "1,2,3",Description = "ws+delims")]
		[TestCase("3,1,2", "1,2,3",Description = "order")]
		[TestCase("1,2,3","1,2,3",Description = "basis")]
		public void GeneratesValidAltObjFilter(string sample, string result) {
			var objh = new ReferenceHandler {Contragents = sample};
			objh.Normalize(_session);
			Assert.AreEqual(result,objh.Contragents);
		}
	}
}

using System;
using System.Linq;
using NUnit.Framework;
using System.Threading.Tasks;
using Qorpent.Utils.Extensions;

namespace Zeta.Extreme.Core.Tests.CoreTests
{

	
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
		public enum ManyPartTestType
		{
			NotOrder,
			NotEqual,
			Ambigous,
			Nulltest,
		}	
		
		[Test]
		[Combinatorial]
		[Description("Тест проверяет валидность формирования и уникальность кэш строк по множественным параметрам запроса")]
		public void ManyOrderTest(
			[Values(ManyPart.RowIds,ManyPart.RowCodes,ManyPart.ColCodes,ManyPart.ObjIds,ManyPart.ObjCodes,ManyPart.Years,ManyPart.Periods)]ManyPart part,
			[Values(ManyPartTestType.Nulltest, ManyPartTestType.NotOrder, ManyPartTestType.NotEqual, ManyPartTestType.Ambigous)] ManyPartTestType testtype)
		{
			if(testtype==ManyPartTestType.Nulltest && new[]{ManyPart.RowIds,ManyPart.ObjIds,ManyPart.Years,ManyPart.Periods}.Any(_=>_==part)) {
				Assert.Ignore("cannot test nullable on integers");
			}
			var baseset = new[] {"2001", "2002", "2003", "2004", "2005"}; //avoid not defined year
			string[] otherset = null;
			bool checkCondition = false;
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

			var fst = new ZexQuery();
			var sec = new ZexQuery();

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
			Console.WriteLine("-- {0} {1} --",part,testtype);
			Console.WriteLine(key1);
			Console.WriteLine();
			Console.WriteLine(key2);
			Console.WriteLine("-------------------------");
			Assert.AreEqual(checkCondition,key1==key2);
		}
	}

	[TestFixture(Description = "Базовые тесты по регистрации запросов в сессии")]
	public class DefaultRegistryHelperTestBase : SessionTestBase {
		[Test(Description = "самая базовая проверка - просто поместить запрос сессию")]
		public  void CanRegisterOneQueryAynchronously()
		{
			var q = new ZexQuery { CustomHashPrefix = "CanRegisterOneQuerySynchronously" };
			var rqt =  session.RegisterAsync(q);
			session.WaitRegistration();
			var rq = rqt.Result;
			Assert.AreEqual(q, rq);
			Assert.AreSame(rq, session.MainQueryRegistry[q.GetCacheKey()]);
			Assert.AreSame(rq, session.ActiveSet[q.GetCacheKey()]);
			Assert.AreEqual(0, session.ProcessedSet.Count);
			Assert.AreEqual(1, session.ActiveSet.Count);
			Assert.AreEqual(1, session.MainQueryRegistry.Count);
		}

		[Test(Description = "самая базовая проверка - просто поместить запрос сессию")]
		public void CanRegisterAsynWithValidKeys()
		{
			for(var i=0;i<100;i++) {
				var q = new ZexQuery {CustomHashPrefix = i.ToString()};
				session.RegisterAsync(q, "uid"+i);
			}
			session.WaitRegistration();
			foreach (var zexQuery in session.MainQueryRegistry) {
				Assert.AreEqual(zexQuery.Value.CustomHashPrefix,zexQuery.Key.Substring(3));
			}
		}

		[Test(Description = "самая базовая проверка - просто поместить запрос сессию")]
		public void CanRegisterOneQuerySynchronously() {
			var q = new ZexQuery {CustomHashPrefix = "CanRegisterOneQuerySynchronously"};
			var rq = session.Register(q);
			Assert.AreEqual(q,rq);
			Assert.AreSame(rq,session.MainQueryRegistry[q.GetCacheKey()]);
			Assert.AreSame(rq, session.ActiveSet[q.GetCacheKey()]);
			Assert.AreEqual(0,session.ProcessedSet.Count);
			Assert.AreEqual(1,session.ActiveSet.Count);
			Assert.AreEqual(1, session.MainQueryRegistry.Count);
		}

		[Test(Description = "самая базовая проверка - просто поместить запрос сессию")]
		public void CanRegisterOneQuerySynchronouslyWithCustomUid()
		{
			var q = new ZexQuery { CustomHashPrefix = "hash" };
			var rq = session.Register(q, "CanRegisterOneQuerySynchronouslyWithCustomUid");
			Assert.AreEqual(q, rq);
			Assert.AreSame(rq, session.MainQueryRegistry["CanRegisterOneQuerySynchronouslyWithCustomUid"]);
			Assert.AreSame(rq, session.ActiveSet[q.GetCacheKey()]);
			Assert.AreEqual(0, session.ProcessedSet.Count);
			Assert.AreEqual(1, session.ActiveSet.Count);
			Assert.AreEqual(1, session.MainQueryRegistry.Count);
		}

		[Test(Description = "самая базовая проверка - просто поместить запрос сессию")]
		public void DontRegisterTwice()
		{
			var q = new ZexQuery { CustomHashPrefix = "hash" };
			var rq = session.Register(q, "CanRegisterOneQuerySynchronouslyWithCustomUid");
			q = new ZexQuery { CustomHashPrefix = "hash" }; //имитируем другой, но аналогичный запрос
			var rq2 = session.Register(q, "CanRegisterOneQuerySynchronouslyWithCustomUid2");
			Assert.AreSame(rq, rq2); // проверяем, что оба раза вернули тот-же запрос
			Assert.AreSame(rq, session.MainQueryRegistry["CanRegisterOneQuerySynchronouslyWithCustomUid"]);
			Assert.AreSame(rq, session.MainQueryRegistry["CanRegisterOneQuerySynchronouslyWithCustomUid2"]);
			//проверили что на всех "пользовательских ветках" один и тот же запрос
			Assert.AreSame(rq, session.ActiveSet[rq.GetCacheKey()]);
			Assert.AreEqual(0, session.ProcessedSet.Count);
			Assert.AreEqual(1, session.ActiveSet.Count);
			Assert.AreEqual(2, session.MainQueryRegistry.Count);
		}
	}
}

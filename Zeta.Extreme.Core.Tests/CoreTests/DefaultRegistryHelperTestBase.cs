using NUnit.Framework;
using System.Threading.Tasks;

namespace Zeta.Extreme.Core.Tests.CoreTests
{



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

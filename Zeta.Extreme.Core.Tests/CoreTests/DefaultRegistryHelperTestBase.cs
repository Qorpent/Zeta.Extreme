#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : DefaultRegistryHelperTestBase.cs
// Project: Zeta.Extreme.Core.Tests
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using NUnit.Framework;

namespace Zeta.Extreme.Core.Tests.CoreTests {
	[TestFixture(Description = "Базовые тесты по регистрации запросов в сессии")]
	public class DefaultRegistryHelperTestBase : SessionTestBase {
		[Test(Description = "самая базовая проверка - просто поместить запрос сессию")]
		public void CanRegisterAsynWithValidKeys() {
			for (var i = 0; i < 100; i++) {
				var q = new Query {CustomHashPrefix = i.ToString()};
				session.RegisterAsync(q, "uid" + i);
			}
			session.WaitPreparation();
			foreach (var zexQuery in session.Registry) {
				Assert.AreEqual(zexQuery.Value.CustomHashPrefix, zexQuery.Key.Substring(3));
			}
		}

		[Test(Description = "самая базовая проверка - просто поместить запрос сессию")]
		public void CanRegisterOneQueryAynchronously() {
			var q = new Query {CustomHashPrefix = "CanRegisterOneQuerySynchronously"};
			var rqt = session.RegisterAsync(q);
			session.WaitPreparation();
			var rq = rqt.Result;
			///	Assert.AreEqual(q, rq);
			Assert.AreSame(rq, session.Registry[q.GetCacheKey()]);
			Assert.AreSame(rq, session.ActiveSet[q.GetCacheKey()]);
			Assert.AreEqual(1, session.ActiveSet.Count);
			Assert.AreEqual(1, session.Registry.Count);
		}

		[Test(Description = "самая базовая проверка - просто поместить запрос сессию")]
		public void CanRegisterOneQuerySynchronously() {
			var q = new Query {CustomHashPrefix = "CanRegisterOneQuerySynchronously"};
			var rq = session.Register(q);
			Assert.AreSame(rq, session.Registry[q.GetCacheKey()]);
			Assert.AreSame(rq, session.ActiveSet[q.GetCacheKey()]);
			Assert.AreEqual(1, session.ActiveSet.Count);
			Assert.AreEqual(1, session.Registry.Count);
		}

		[Test(Description = "самая базовая проверка - просто поместить запрос сессию")]
		public void CanRegisterOneQuerySynchronouslyWithCustomUid() {
			var q = new Query {CustomHashPrefix = "hash"};
			var rq = session.Register(q, "CanRegisterOneQuerySynchronouslyWithCustomUid");
			//	Assert.AreEqual(q, rq);
			Assert.AreSame(rq, session.Registry["CanRegisterOneQuerySynchronouslyWithCustomUid"]);
			Assert.AreSame(rq, session.ActiveSet[q.GetCacheKey()]);
			Assert.AreEqual(1, session.ActiveSet.Count);
			Assert.AreEqual(1, session.Registry.Count);
		}

		[Test(Description = "самая базовая проверка - просто поместить запрос сессию")]
		public void DontRegisterTwice() {
			var q = new Query {CustomHashPrefix = "hash"};
			var rq = session.Register(q, "CanRegisterOneQuerySynchronouslyWithCustomUid");
			q = new Query {CustomHashPrefix = "hash"}; //имитируем другой, но аналогичный запрос
			var rq2 = session.Register(q, "CanRegisterOneQuerySynchronouslyWithCustomUid2");
			Assert.AreSame(rq, rq2); // проверяем, что оба раза вернули тот-же запрос
			Assert.AreSame(rq, session.Registry["CanRegisterOneQuerySynchronouslyWithCustomUid"]);
			Assert.AreSame(rq, session.Registry["CanRegisterOneQuerySynchronouslyWithCustomUid2"]);
			//проверили что на всех "пользовательских ветках" один и тот же запрос
			Assert.AreSame(rq, session.ActiveSet[rq.GetCacheKey()]);
			Assert.AreEqual(1, session.ActiveSet.Count);
			Assert.AreEqual(2, session.Registry.Count);
		}
	}
}
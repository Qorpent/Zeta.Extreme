using System;
using System.Diagnostics;
using System.Threading;
using NUnit.Framework;

namespace Zeta.Extreme.Core.Tests.CoreTests {
	[TestFixture]
	public class DefaultRegistryHelperTestBaseBenchmark:SessionTestBase {
		class OverheatDefaultRegistry : DefaultZexRegistryHelper {
			public OverheatDefaultRegistry(ZexSession session) : base(session) {}
			public override ZexQuery Register(ZexQuery query, string uid) {
			//	Console.WriteLine("Enter");
				var result = base.Register(query, uid);
				Thread.Sleep(0); //overheat
			//	Console.WriteLine("Leave");
				return result;
			}
		}
		[Explicit]
		[Test]
		public void SyncRegistryOf10000Queries() {
			session.CustomRegistryHelperClass = typeof (OverheatDefaultRegistry);
			var sw = Stopwatch.StartNew();
			for(var i=0;i<10000;i++) {
				var q = new ZexQuery {CustomHashPrefix = i.ToString()};
				session.Register(q);
			}
			sw.Stop();
			Console.WriteLine(sw.ElapsedMilliseconds);
			Console.WriteLine(session.MainQueryRegistry.Count);
			Assert.AreEqual(10000,session.MainQueryRegistry.Count);
			Assert.Less(sw.ElapsedMilliseconds, 100000);
		}

		[Explicit]
		[Test]
		public void ASyncRegistryOf10000Queries()
		{
			session.CustomRegistryHelperClass = typeof(OverheatDefaultRegistry);
			var sw = Stopwatch.StartNew();
			for (var i = 0; i < 10000; i++)
			{
				var q = new ZexQuery { CustomHashPrefix = i.ToString() };
				session.RegisterAsync(q);
			}
			session.WaitRegistration();
			sw.Stop();
			Console.WriteLine(sw.ElapsedMilliseconds);
			Console.WriteLine(session.MainQueryRegistry.Count);
			Assert.AreEqual(10000, session.MainQueryRegistry.Count);
			Assert.AreEqual(10000, session.ActiveSet.Count);
			Assert.Less(sw.ElapsedMilliseconds, 100000);
		}

		[Explicit]
		[Test]
		public void ASyncRegistryOf10000QueriesDoubles()
		{
			session.CustomRegistryHelperClass = typeof(OverheatDefaultRegistry);
			var sw = Stopwatch.StartNew();
			for (var i = 0; i < 10000; i++)
			{
				var q = new ZexQuery { CustomHashPrefix = (i % 500).ToString() };
				
				session.RegisterAsync(q);
			}
			session.WaitRegistration();
			sw.Stop();
			Console.WriteLine(sw.ElapsedMilliseconds);
			Console.WriteLine(session.MainQueryRegistry.Count);
			Assert.AreEqual(500, session.MainQueryRegistry.Count);
			Assert.AreEqual(500, session.ActiveSet.Count);
			Assert.Less(sw.ElapsedMilliseconds, 100000);
		}


		[Explicit]
		[Test]
		public void ComplexTest10000()
		{
			session.CustomRegistryHelperClass = typeof(OverheatDefaultRegistry);

			var sw = Stopwatch.StartNew();
			for (var i = 0; i < 10000; i++)
			{
				var q = new ZexQuery { CustomHashPrefix = i.ToString() };
				session.RegisterAsync(q);
			}
			session.WaitRegistration();
			sw.Stop();
			Console.WriteLine(sw.ElapsedMilliseconds);
			var asynctime = sw.ElapsedMilliseconds;
			Console.WriteLine(session.MainQueryRegistry.Count);
			Assert.AreEqual(10000, session.MainQueryRegistry.Count);
			Assert.Less(asynctime, 2000);

			session = new ZexSession();
			session.CustomRegistryHelperClass = typeof(OverheatDefaultRegistry);
			sw = Stopwatch.StartNew();
			for (var i = 0; i < 10000; i++)
			{
				var q = new ZexQuery { CustomHashPrefix = i.ToString() };
				session.Register(q);
			}
			sw.Stop();
			Console.WriteLine(sw.ElapsedMilliseconds);
			var synctime = sw.ElapsedMilliseconds;
			Console.WriteLine(session.MainQueryRegistry.Count);
			Assert.AreEqual(10000, session.MainQueryRegistry.Count);
			Assert.Less(synctime, 10500);


			Assert.Greater(synctime/3,asynctime);
		}
	}
}
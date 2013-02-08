using System;
using System.Diagnostics;
using System.Threading;
using NUnit.Framework;

namespace Zeta.Extreme.Core.Tests.CoreTests {
	[TestFixture]
	public class DefaultRegistryHelperTestBaseBenchmark:SessionTestBase {
	
		[Explicit]
		[Test]
		public void SyncRegistryOf10000Queries() {
			
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
		public void ComplexRegistryTest([Values(10000,20000,30000,40000,50000)]int cnt)
		{
			var sw = Stopwatch.StartNew();
			for (var i = 0; i < cnt; i++)
			{
				prepareQueries(i,true);
			}
			session.WaitRegistration();
			sw.Stop();
			Console.WriteLine(sw.ElapsedMilliseconds);
			var asynctime = sw.ElapsedMilliseconds;
			Console.WriteLine(session.MainQueryRegistry.Count);
			Assert.AreEqual(cnt*2, session.MainQueryRegistry.Count);
		
			session = new ZexSession();

			sw = Stopwatch.StartNew();
			for (var i = 0; i < cnt; i++)
			{
				prepareQueries(i,false);
			}
			sw.Stop();
			Console.WriteLine(sw.ElapsedMilliseconds);
			var synctime = sw.ElapsedMilliseconds;
			Console.WriteLine(session.MainQueryRegistry.Count);
			Assert.AreEqual(cnt*2, session.MainQueryRegistry.Count);
			Assert.Less(asynctime, synctime);


			//Assert.Greater(synctime/3,asynctime);
		}

		private void prepareQueries(int i, bool async) {
			var q = new ZexQuery
				{
					CustomHashPrefix = i.ToString(),
					Row = {Codes = new[] {"1", "2", "3"}},
					Col = {Codes = new[] {"1", "2", "3"}},
					Obj = {Codes = new[] {"1", "2", "3"}},
					Time = {Years = new[] {2001, 2002, 2003}, Periods = new[] {1, 2, 3, 4, 5}}
				};
			//it forces more complicated behavior
			if(async)session.RegisterAsync(q);
			else session.Register(q);
			q = new ZexQuery
				{CustomHashPrefix = i.ToString() + "x", Time = {BasePeriod = 12, Period = -101, BaseYear = 2012, Year = -1}};

			//it forces more complicated behavior
			if (async) session.RegisterAsync(q);
			else session.Register(q);
		}
	}
}
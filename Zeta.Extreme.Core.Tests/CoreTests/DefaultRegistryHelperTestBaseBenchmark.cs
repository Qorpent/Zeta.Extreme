using System;
using System.Diagnostics;
using System.Threading;
using Comdiv.Application;
using Comdiv.Persistence;
using Comdiv.Zeta.Data.Minimal;
using Comdiv.Zeta.Model;
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
			Console.WriteLine(session.Registry.Count);
			Assert.AreEqual(10000,session.Registry.Count);
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
			session.WaitPreparation();
			sw.Stop();
			Console.WriteLine(sw.ElapsedMilliseconds);
			Console.WriteLine(session.Registry.Count);
			Assert.AreEqual(10000, session.Registry.Count);
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
			session.WaitPreparation();
			sw.Stop();
			Console.WriteLine(sw.ElapsedMilliseconds);
			Console.WriteLine(session.Registry.Count);
			Assert.AreEqual(500, session.Registry.Count);
			Assert.AreEqual(500, session.ActiveSet.Count);
			Assert.Less(sw.ElapsedMilliseconds, 100000);
		}

		[Explicit]
		[Test]
		public void ComplexRegistryTest([Values(20000,10000, 30000, 40000, 50000, 100000)]int cnt)
		{
			
			
			var sw = Stopwatch.StartNew();
			for (var i = 0; i < cnt; i++)
			{
				prepareQueries(i,true);
			}
			session.WaitPreparation();
			sw.Stop();
			Console.WriteLine(sw.ElapsedMilliseconds);
			var asynctime = sw.ElapsedMilliseconds;
			Console.WriteLine(session.Registry.Count);
			Assert.AreEqual(cnt*2, session.Registry.Count);
			Console.WriteLine(session.GetStatisticString());
		
			session = new ZexSession(true);

			sw = Stopwatch.StartNew();
			for (var i = 0; i < cnt; i++)
			{
				prepareQueries(i,false);
			}
			sw.Stop();
			Console.WriteLine(sw.ElapsedMilliseconds);
			var synctime = sw.ElapsedMilliseconds;
			Console.WriteLine(session.Registry.Count);
			Assert.AreEqual(cnt*2, session.Registry.Count);
			Assert.Less(asynctime, synctime);
			Console.WriteLine(session.GetStatisticString());

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
				{
					CustomHashPrefix = i.ToString() + "x", 
					Time = {BasePeriod = 12, Period = -204, BaseYear = 2012, Year = -1},
					Row = { Code = "m260113" },
					Col = { Code = "Á1" },
					Obj = { Id = 352, Type = ObjType.Obj }
				};

			//it forces more complicated behavior
			if (async) session.RegisterAsync(q);
			else session.Register(q);
		}
	}
}
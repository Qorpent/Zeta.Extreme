#region LICENSE
// Copyright 2007-2013 Qorpent Team - http://github.com/Qorpent
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// PROJECT ORIGIN: Zeta.Extreme.Core.Tests/DefaultRegistryHelperTestBaseBenchmark.cs
#endregion
using System;
using System.Diagnostics;
using NUnit.Framework;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.Core.Tests.CoreTests {
	[TestFixture]
	public class DefaultRegistryHelperTestBaseBenchmark : SessionTestBase {
		private void prepareQueries(int i, bool async) {
			var q = new Query
				{
					CustomHashPrefix = i.ToString(),
					Row = {Codes = new[] {"1", "2", "3"}},
					Col = {Codes = new[] {"1", "2", "3"}},
					Obj = {Codes = new[] {"1", "2", "3"}},
					Time = {Years = new[] {2001, 2002, 2003}, Periods = new[] {1, 2, 3, 4, 5}}
				};
			//it forces more complicated behavior
			if (async) {
				session.RegisterAsync(q);
			}
			else {
				session.Register(q);
			}
			q = new Query
				{
					CustomHashPrefix = i.ToString() + "x",
					Time = {BasePeriod = 12, Period = -204, BaseYear = 2012, Year = -1},
					Row = {Code = "m260113"},
					Col = {Code = "�1"},
					Obj = {Id = 352, Type = ZoneType.Obj}
				};

			//it forces more complicated behavior
			if (async) {
				session.RegisterAsync(q);
			}
			else {
				session.Register(q);
			}
		}

		[Explicit]
		[Test]
		public void ASyncRegistryOf10000Queries() {
			var sw = Stopwatch.StartNew();
			for (var i = 0; i < 10000; i++) {
				var q = new Query {CustomHashPrefix = i.ToString()};
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
		public void ASyncRegistryOf10000QueriesDoubles() {
			var sw = Stopwatch.StartNew();
			for (var i = 0; i < 10000; i++) {
				var q = new Query {CustomHashPrefix = (i%500).ToString()};

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
		public void ComplexRegistryTest([Values(20000, 10000, 30000, 40000, 50000, 100000)] int cnt) {
			var sw = Stopwatch.StartNew();
			for (var i = 0; i < cnt; i++) {
				prepareQueries(i, true);
			}
			session.WaitPreparation();
			sw.Stop();
			Console.WriteLine(sw.ElapsedMilliseconds);
			var asynctime = sw.ElapsedMilliseconds;
			Console.WriteLine(session.Registry.Count);
			Assert.AreEqual(cnt*2, session.Registry.Count);
			Console.WriteLine(session.GetStatisticString());

			session = new Session(true);

			sw = Stopwatch.StartNew();
			for (var i = 0; i < cnt; i++) {
				prepareQueries(i, false);
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

		[Explicit]
		[Test]
		public void SyncRegistryOf10000Queries() {
			var sw = Stopwatch.StartNew();
			for (var i = 0; i < 10000; i++) {
				var q = new Query {CustomHashPrefix = i.ToString()};
				session.Register(q);
			}
			sw.Stop();
			Console.WriteLine(sw.ElapsedMilliseconds);
			Console.WriteLine(session.Registry.Count);
			Assert.AreEqual(10000, session.Registry.Count);
			Assert.Less(sw.ElapsedMilliseconds, 100000);
		}
	}
}
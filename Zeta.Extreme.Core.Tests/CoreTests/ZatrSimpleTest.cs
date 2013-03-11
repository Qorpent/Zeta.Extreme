#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : ZatrSimpleTest.cs
// Project: Zeta.Extreme.Core.Tests
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Extensions;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.MetaCaches;
using Zeta.Extreme.Model.Querying;
using Zeta.Extreme.Poco;
using Zeta.Extreme.Poco.Inerfaces;
using Zeta.Extreme.Primary;

namespace Zeta.Extreme.Core.Tests.CoreTests {
	[TestFixture(Description = "Начинаем работать с простыми формами")]
	public class ZatrSimpleTest : SessionTestBase {
		public row[] rows;
		public row[] rows1;
		public row[] rows2;
		public IZetaColumn col;
		public int[] periods;
		public IZetaMainObject obj;
		public ZatrSimpleTest() {}

		private ZatrSimpleTest(ZatrSimpleTest zatrSimpleTest) {
			rows = zatrSimpleTest.rows;
			col = zatrSimpleTest.col;
			obj = zatrSimpleTest.obj;
			periods = zatrSimpleTest.periods;
			objs = zatrSimpleTest.objs;
			colset = zatrSimpleTest.colset;
			rows1 = zatrSimpleTest.rows1;
			rows2 = zatrSimpleTest.rows2;
		}

		[TestFixtureSetUp]
		public override void FixtureSetup() {
			base.FixtureSetup();
			rows = RowCache.get("m260").AllChildren.OfType<row>().ToArray();
			;
			rows1 = RowCache.get("m111").AllChildren.OfType<row>().ToArray();
			;
			rows2 = RowCache.get("m112").AllChildren.OfType<row>().ToArray();
			;
			colset = new[]
				{
					new ColumnDesc("PLAN", 2012, 301),
					new ColumnDesc("PLAN", 2012, 303), //первичная основа для PLANSNG1, но формул у нас пока нет
					new ColumnDesc("Б1", 2012, 22),
					new ColumnDesc("Б1", 2012, 13),
					new ColumnDesc("Б1", 2012, 3), //для Fact 9m
					new ColumnDesc("Б1", 2012, 444),
				};
			col = ColumnCache.get("Б1");
			periods = new[] {11, 12, 13, 1, 401, 403};
			obj = MetaCache.Default.Get<IZetaMainObject>(352);
			objs = ObjCache.ObjById.Values.Where(_ => _.ShowOnStartPage).ToArray();
		}

		private void ExecuteFormBatch(int timespan, int rsn, int batchsize, int count, int qsize) {
			var sw = Stopwatch.StartNew();
			var waittime = new TimeSpan();
			var t = new List<Task>();
			for (var i = 0; i < count; i++) {
				if (rsn == 100) {
					t.Add(Task.Run(()
					               => new ZatrSimpleTest(this).RunForm(batchsize, i, true, 0))
						);
					t.Add(Task.Run(()
					               => new ZatrSimpleTest(this).RunForm(batchsize, i, true, 1))
						);
					t.Add(Task.Run(()
					               => new ZatrSimpleTest(this).RunForm(batchsize, i, true, 2))
						);
				}
				else {
					t.Add(Task.Run(()
					               => new ZatrSimpleTest(this).RunForm(batchsize, i, true, rsn))
						);
				}
				Thread.Sleep(timespan/2);
				var waitfirs = t.Where(x => !x.IsCompleted).ToArray();
				var ws = Stopwatch.StartNew();
				while (qsize <= waitfirs.Length) {
					Task.WaitAny(waitfirs);
					Thread.Sleep(timespan/2);
					waitfirs = t.Where(x => !x.IsCompleted).ToArray();
				}
				ws.Stop();
				waittime += ws.Elapsed;
				//Thread.Sleep(Math.Max(0, timespan - (int) waittime.TotalMilliseconds));
			}
			Task.WaitAll(t.ToArray());
			sw.Stop();
			var sessions = t.OfType<Task<Session>>().Select(_ => _.Result).ToArray();

			var sqltime = new TimeSpan();
			foreach (var task in t.OfType<Task<Session>>()) {
				sqltime += task.Result.GetStatistics().BatchTime;
			}

			Console.WriteLine();
			Console.WriteLine("STATISTICS");
			Console.WriteLine("===================================================");
			Console.WriteLine("fulltime: " + sw.Elapsed);
			Console.WriteLine("syncwaittime: " + waittime);
			Console.WriteLine("srvresponsetime: " + (sw.Elapsed - TimeSpan.FromMilliseconds(500*100)));
			var cntresponsetime = TimeSpan.FromMilliseconds(sessions.Select(x => x.GetStatistics().TimeTotal.TotalMilliseconds).Sum());
			Console.WriteLine("clntresponsetime: " + cntresponsetime);
			var avgclntresponce =
				TimeSpan.FromMilliseconds(sessions.Select(x => x.GetStatistics().TimeTotal.TotalMilliseconds).Average());
			Console.WriteLine("clnavgresponsetime: " + avgclntresponce);
			Console.WriteLine("clnminresponsetime: " +
							  TimeSpan.FromMilliseconds(sessions.Select(_ => _.GetStatistics().TimeTotal.TotalMilliseconds).Min()));
			Console.WriteLine("clnmaxresponsetime: " +
							  TimeSpan.FromMilliseconds(sessions.Select(_ => _.GetStatistics().TimeTotal.TotalMilliseconds).Max()));
			Console.WriteLine("sqlquerycnt: " + sessions.Select(_ => _.GetStatistics().BatchCount).Sum());
			Console.WriteLine("sqltotaltime: " +
							  TimeSpan.FromMilliseconds(sessions.Select(_ => _.GetStatistics().BatchTime.TotalMilliseconds).Sum()));
			Console.WriteLine("primarycount: " + sessions.Select(_ => _.GetStatistics().QueryTypePrimary).Sum());
			Console.WriteLine("getprimarycount: " + sessions.Select(_ => _.GetStatistics().PrimaryCatched).Sum());
			Console.WriteLine("useprimarycount: " + sessions.Select(_ => _.GetStatistics().PrimaryAffected).Sum());
			Console.WriteLine("wavgsrvcellcost: " +
			                  TimeSpan.FromMilliseconds(
								  sessions.Select(_ => _.GetStatistics().TimeTotal.TotalMilliseconds / _.GetStatistics().QueryTypePrimary).Sum() / count));
			Console.WriteLine("wavgclncellcost: " +
			                  TimeSpan.FromMilliseconds(
								  sessions.Select(_ => _.GetStatistics().TimeTotal.TotalMilliseconds / _.GetStatistics().QueryTypePrimary).Sum() / count));
			Console.WriteLine();
		}

		private static int formcount;
		private IZetaMainObject[] objs;
		private ColumnDesc[] colset;

		private ISession RunForm(int batchsize = 100, int objnum = -1, bool usecolset = false, int rowset = 0) {
			var sw = Stopwatch.StartNew();
			var rs = rows;
			if (rowset == 1) {
				rs = rows1;
			}
			if (rowset == 2) {
				rs = rows2;
			}
			formcount++;
			var id = formcount;
			Console.WriteLine("start form " + id);
			var _session = new Session(true);
			session = _session;
			((DefaultPrimarySource)_session.PrimarySource).BatchSize = batchsize;
			foreach (var row in rs
				.OrderBy(x =>
					{
						if (x.IsFormula) {
							return 100;
						}
						if (x.HasChildren()) {
							return 5 + x.Level;
						}
						return x.Level;
					}
				)) {
				var _obj = obj;
				if (-1 != objnum) {
					_obj = objs.ElementAt(objnum%objs.Length);
				}
				if (usecolset) {
					foreach (var c in colset) {
						var q = new Query
							{
								Row = {Native = row},
								Col = {Native = ColumnCache.get(c.Code)},
								Time = {Year = c.Year, Period = c.Period},
								Obj = {Native = _obj}
							};
						_session.RegisterAsync(q, string.Format("{0}_{1}_{2}_{3}", row.Code, c.Code, c.Year, c.Period));
					}
				}
				else {
					foreach (var period in periods) {
						var q = new Query
							{
								Row = {Native = row},
								Col = {Native = col},
								Time = {Year = 2012, Period = period},
								Obj = {Native = _obj}
							};
						_session.RegisterAsync(q, string.Format("{0}_{1}_{2}_{3}", row.Code, col.Code, 2012, period));
					}
				}
			}
			Console.WriteLine("wait prepare " + id);
			_session.WaitPreparation();
			Console.WriteLine("wait eval " + id);
			_session.WaitEvaluation();
			Console.WriteLine("finish " + id);
			sw.Stop();
			_session.GetStatistics().TimeTotal = sw.Elapsed;
			return _session;
		}


		private const int CAPT_COUNT = 1;
		private const int OBS_COUNT = 6;

		[Test]
		[Explicit]
		public void BalansBatch() {
			var batchsize = 500;
			var count = 200;
			var timespan = 100;
			var rsn = 1;
			ExecuteFormBatch(timespan, rsn, batchsize, count, 7);
		}

		[Test]
		public void Bug_Dont_Prceed_Redirect() {
			var q = new Query
				{
					Row = {Native = RowCache.get(7843)},
					Col = {Native = col},
					Time = {Year = 2012, Period = 11},
					Obj = {Native = obj}
				};
			session.RegisterAsync(q, "test");
			session.WaitPreparation();

			var q1 = session.Registry["test"];
			Assert.AreEqual("r590610", q1.Row.Code); //redirect performed
			Assert.AreEqual(1, session.GetStatistics().RowRedirections); //statistics acounted
		}

		[Test]
		public void Bug_Something_Bad_WithSums() {
			var q = new Query
				{
					Row = {Native = RowCache.get("m260400")},
					Col = {Native = col},
					Time = {Year = 2012, Period = 11},
					Obj = {Native = obj}
				};
			session.RegisterAsync(q, "test");
			session.Execute();
		}

		[Test]
		public void Bug_Something_Wrong_With_Sums() {
			var q = new Query
				{Row = {Code = "m260"}, Col = {Code = "Б1"}, Time = {Year = 2012, Period = 13}, Obj = {Id = 352}};
			session.RegisterAsync(q);
			session.WaitPreparation();
			//session.WaitEvaluation();
		}

		[Test]
		[Explicit]
		public void FindBestBatchSize() {
			var results = new List<TimeSpan>();
			for (var i = 50; i < 1000; i += 50) {
				Console.WriteLine(i);
				var sw = Stopwatch.StartNew();
				for (var j = 0; j < 5; j++) {
					Console.Write(".");
					RunForm(i);
				}
				sw.Stop();
				results.Add(sw.Elapsed);
				Console.WriteLine(sw.Elapsed);
			}
			foreach (var timeSpan in results) {
				Console.WriteLine(timeSpan);
			}
		}

		[Test]
		[Explicit]
		public void InsaneBatch() {
			var batchsize = 1000;
			var count = 107;
			var timespan = 500;
			var rsn = 100;
			ExecuteFormBatch(timespan, rsn, batchsize, count, 8);
		}

		[Test]
		[Explicit]
		public void NoIZatrBatch() {
			var batchsize = 2000;
			var count = 4;
			var timespan = 10000;
			var rsn = 0;
			ExecuteFormBatch(timespan, rsn, batchsize, count, 5);
		}

		[Test]
		[Explicit]
		public void PribBatch() {
			var batchsize = 500;
			var count = 200;
			var timespan = 100;
			var rsn = 2;
			ExecuteFormBatch(timespan, rsn, batchsize, count, 7);
		}

		[Test]
		[Ignore("metrics are not more equal (after formula fixes)")]
		public void RegisterInSimpleToComplexWay() {
			var sw = Stopwatch.StartNew();
			RunForm(2000);
			sw.Stop();
			Console.WriteLine(sw.ElapsedMilliseconds);
			Assert.AreEqual(990, session.Registry.Count(x => x.Key != x.Value.GetCacheKey()));
			Assert.AreEqual(1032, session.GetStatistics().RegistryStartedUser);
			Assert.AreEqual(1032
			                - CAPT_COUNT*periods.Length
			                - OBS_COUNT*periods.Length
			                , session.GetStatistics().RegistryUser);

			Assert.True(session.Registry.Values.Any(_ => _.Row.IsSum && _.Result.NumericResult > 0));
		}


		[Test]
		[Repeat(20)]
		[Explicit]
		public void RegisterInSimpleToComplexWay20Times() {
			var sw = Stopwatch.StartNew();
			RunForm(1000);
			sw.Stop();
			Console.WriteLine(sw.ElapsedMilliseconds);
			Assert.AreEqual(996, session.Registry.Where(x => x.Key != x.Value.GetCacheKey()).Count());
			Assert.AreEqual(1038, session.GetStatistics().RegistryStartedUser);
			Assert.AreEqual(1038
			                - CAPT_COUNT*periods.Length
			                - OBS_COUNT*periods.Length
			                , session.GetStatistics().RegistryUser);

			Assert.True(session.Registry.Values.Any(_ => _.Row.IsSum && _.GetResult().NumericResult > 0));
		}

		[Test]
		[Explicit]
		public void ZatrBatch() {
			var batchsize = 2000;
			var count = 100;
			var timespan = 500;
			var rsn = 0;
			ExecuteFormBatch(timespan, rsn, batchsize, count, 5);
		}
	}
}
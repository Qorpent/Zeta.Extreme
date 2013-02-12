using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Comdiv.Application;
using Comdiv.Zeta.Data.Minimal;
using Comdiv.Zeta.Model;
using NUnit.Framework;

namespace Zeta.Extreme.Core.Tests.CoreTests {
	[TestFixture(Description = "Начинаем работать с простыми формами")]
	public class ZatrSimpleTest : SessionTestBase {
		public row[] rows;
		public row[] rows1;
		public row[] rows2;
		public IZetaColumn col;
		public int[] periods;
		public IZetaMainObject obj;
		public ZatrSimpleTest(){}
		private ZatrSimpleTest(ZatrSimpleTest zatrSimpleTest) {
			this.rows = zatrSimpleTest.rows;
			this.col = zatrSimpleTest.col;
			this.obj = zatrSimpleTest.obj;
			this.periods = zatrSimpleTest.periods;
			this.objs = zatrSimpleTest.objs;
			this.colset = zatrSimpleTest.colset;
			this.rows1 = zatrSimpleTest.rows1;
			this.rows2 = zatrSimpleTest.rows2;
		}

		[TestFixtureSetUp]
		public override void FixtureSetup()
		{
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
					new ColumnDesc("Б1",2012,22), 
					new ColumnDesc("Б1",2012,13), 
					new ColumnDesc("Б1",2012,3) ,//для Fact 9m
					new ColumnDesc("Б1",2012,444), 


				};
			col = ColumnCache.get("Б1");
			periods = new[] { 11, 12, 13, 1, 401, 403 };
			obj = myapp.storage.AsQueryable<IZetaMainObject>().First(x => x.Id == 352);
			objs = myapp.storage.AsQueryable<IZetaMainObject>().Where(x => x.ShowOnStartPage).ToArray();
		}
		[Test]
		public void Bug_Something_Wrong_With_Sums() {
			var q = new ZexQuery
				{Row = {Code = "m260"}, Col = {Code = "Б1"}, Time = {Year = 2012, Period = 13}, Obj = {Id = 352}};
			session.RegisterAsync(q);
			session.WaitPreparation();
			//session.WaitEvaluation();
		}

		[Test]
		public void RegisterInSimpleToComplexWay() {
			var sw = Stopwatch.StartNew();
			RunForm(2000);
			sw.Stop();
			Console.WriteLine(sw.ElapsedMilliseconds);
			Assert.AreEqual(996,session.Registry.Where(x=>x.Key!=x.Value.GetCacheKey()).Count());
			Assert.AreEqual(1038,session.Stat_Registry_Started_User);
			Assert.AreEqual(1038 
				- CAPT_COUNT * periods.Length
				- OBS_COUNT * periods.Length 
				,session.Stat_Registry_User);

			Assert.True(session.Registry.Values.Any(_=>_.Row.IsSum && _.Result.NumericResult >0));

		}


		[Test]
		[Repeat(20)]
		[Explicit]
		public void RegisterInSimpleToComplexWay20Times()
		{
			var sw = Stopwatch.StartNew();
			RunForm(1000);
			sw.Stop();
			Console.WriteLine(sw.ElapsedMilliseconds);
			Assert.AreEqual(996, session.Registry.Where(x => x.Key != x.Value.GetCacheKey()).Count());
			Assert.AreEqual(1038, session.Stat_Registry_Started_User);
			Assert.AreEqual(1038
				- CAPT_COUNT * periods.Length
				- OBS_COUNT * periods.Length
				, session.Stat_Registry_User);

			Assert.True(session.Registry.Values.Any(_ => _.Row.IsSum && _.GetResult().NumericResult > 0));

		}

		[Test]
		[Explicit]
		public void ZatrBatch() {
			int batchsize = 2000;
			int count = 100;
			int timespan = 500;
			int rsn = 0;
			ExecuteFormBatch(timespan, rsn, batchsize, count,5);
		}
		[Test]
		[Explicit]
		public void NoIZatrBatch() {
			int batchsize = 2000;
			int count = 4;
			int timespan = 10000;
			int rsn = 0;
			ExecuteFormBatch(timespan, rsn, batchsize, count,5);
		}
		[Test]
		[Explicit]
		public void BalansBatch()
		{
			int batchsize = 500;
			int count = 200;
			int timespan = 100;
			int rsn = 1;
			ExecuteFormBatch(timespan, rsn, batchsize, count,7);
		}

		[Test]
		[Explicit]
		public void InsaneBatch()
		{
			int batchsize = 1000;
			int count = 107;
			int timespan = 500;
			int rsn = 100;
			ExecuteFormBatch(timespan, rsn, batchsize, count, 8);
		}

		[Test]
		[Explicit]
		public void PribBatch()
		{
			int batchsize = 500;
			int count = 200;
			int timespan = 100;
			int rsn = 2;
			ExecuteFormBatch(timespan, rsn, batchsize, count,7);
		}

		private void ExecuteFormBatch(int timespan, int rsn, int batchsize, int count, int qsize) {
			var sw = Stopwatch.StartNew();
			var waittime = new TimeSpan();
			var t = new List<Task>();
			for (var i = 0; i < count; i++) {

				if(rsn==100) {
					t.Add(Task.Run(()
								   => new ZatrSimpleTest(this).RunForm(batchsize, i, true, 0))
						);
					t.Add(Task.Run(()
								   => new ZatrSimpleTest(this).RunForm(batchsize, i, true, 1))
						);
					t.Add(Task.Run(()
								   => new ZatrSimpleTest(this).RunForm(batchsize, i, true, 2))
						);
				}else {
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
			var sessions = t.OfType<Task<ZexSession>>().Select(_ => _.Result).ToArray();

			var sqltime = new TimeSpan();
			foreach (var task in t.OfType<Task<ZexSession>>()) {
				sqltime += task.Result.Stat_Batch_Time;
			}

			Console.WriteLine();
			Console.WriteLine("STATISTICS");
			Console.WriteLine("===================================================");
			Console.WriteLine("fulltime: " + sw.Elapsed);
			Console.WriteLine("syncwaittime: " + waittime);
			Console.WriteLine("srvresponsetime: " + (sw.Elapsed - TimeSpan.FromMilliseconds(500*100)));
			TimeSpan cntresponsetime = TimeSpan.FromMilliseconds(sessions.Select(x => x.Stat_Time_Total.TotalMilliseconds).Sum());
			Console.WriteLine("clntresponsetime: " + cntresponsetime);
			TimeSpan avgclntresponce =
				TimeSpan.FromMilliseconds(sessions.Select(x => x.Stat_Time_Total.TotalMilliseconds).Average());
			Console.WriteLine("clnavgresponsetime: " + avgclntresponce);
			Console.WriteLine("clnminresponsetime: " +
			                  TimeSpan.FromMilliseconds(sessions.Select(x => x.Stat_Time_Total.TotalMilliseconds).Min()));
			Console.WriteLine("clnmaxresponsetime: " +
			                  TimeSpan.FromMilliseconds(sessions.Select(x => x.Stat_Time_Total.TotalMilliseconds).Max()));
			Console.WriteLine("sqlquerycnt: " + sessions.Select(_ => _.Stat_Batch_Count).Sum());
			Console.WriteLine("sqltotaltime: " +
			                  TimeSpan.FromMilliseconds(sessions.Select(_ => _.Stat_Batch_Time.TotalMilliseconds).Sum()));
			Console.WriteLine("primarycount: " + sessions.Select(_ => _.Stat_QueryType_Primary).Sum());
			Console.WriteLine("getprimarycount: " + sessions.Select(_ => _.Stat_Primary_Catched).Sum());
			Console.WriteLine("useprimarycount: " + sessions.Select(_ => _.Stat_Primary_Affected).Sum());
			Console.WriteLine("wavgsrvcellcost: " +
			                  TimeSpan.FromMilliseconds(
				                  sessions.Select(_ => _.Stat_Time_Total.TotalMilliseconds/_.Stat_QueryType_Primary).Sum()/count));
			Console.WriteLine("wavgclncellcost: " +
			                  TimeSpan.FromMilliseconds(
				                  sessions.Select(_ => _.Stat_Time_Total.TotalMilliseconds/_.Stat_QueryType_Primary).Sum()/count));
			Console.WriteLine();
		}

		[Test]
		[Explicit]
		public void FindBestBatchSize() {
			var results = new List<TimeSpan>();
			for(var i=50;i<1000;i+=50) {
				Console.WriteLine(i);
				var sw = Stopwatch.StartNew();
				for(var j=0;j<5;j++) {
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

		static int formcount = 0;
		private IZetaMainObject[] objs;
		private ColumnDesc[] colset;

		private ZexSession RunForm(int batchsize=100,  int objnum = -1, bool usecolset = false, int rowset =0) {
			var sw = Stopwatch.StartNew();
			var rs = rows;
			if(rowset==1) rs = rows1;
			if(rowset==2) rs = rows2;
			formcount++;
			var id = formcount;
			Console.WriteLine("start form "+id);
			var _session = new ZexSession(true);
			session = _session;
			_session.BatchSize = batchsize;
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
				if(-1!=objnum) {
					
					_obj = objs.ElementAt(objnum % objs.Length);
				}
				if(usecolset) {
					foreach (var c in colset) {
						var q = new ZexQuery
						{
							Row = { Native = row },
							Col = { Native = ColumnCache.get(c.Code) },
							Time = { Year = c.Year, Period = c.Period },
							Obj = { Native = _obj }
						};
						_session.RegisterAsync(q, string.Format("{0}_{1}_{2}_{3}", row.Code,  c.Code, c.Year, c.Period));		
					}
				}else{
				foreach (var period in periods) {
						var q = new ZexQuery
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
			_session.Stat_Time_Total = sw.Elapsed;
			return _session;
		}


		const int CAPT_COUNT =1;
		private const int OBS_COUNT = 6;
		[Test]
		public void Bug_Dont_Prceed_Redirect()
		{
			
			var q = new ZexQuery
			{
				Row = { Native = RowCache.get(7843) },
				Col = { Native = col },
				Time = { Year = 2012, Period = 11 },
				Obj = { Native = obj }
			};
			session.RegisterAsync(q, "test");
			session.WaitPreparation();

			var q1 = session.Registry["test"];
			Assert.AreEqual("r590610",q1.Row.Code); //redirect performed
			Assert.AreEqual(1,session.Stat_Row_Redirections); //statistics acounted


		}

		[Test]
		public void Bug_Something_Bad_WithSums()
		{

			var q = new ZexQuery
			{
				Row = { Native = RowCache.get("m260400") },
				Col = { Native = col },
				Time = { Year = 2012, Period = 11 },
				Obj = { Native = obj }
			};
			session.RegisterAsync(q, "test");
			session.WaitPreparation();
			session.WaitEvaluation();

			

		}


	}
}
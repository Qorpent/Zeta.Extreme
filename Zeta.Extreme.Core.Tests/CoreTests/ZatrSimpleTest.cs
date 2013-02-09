using System;
using System.Diagnostics;
using System.Linq;
using Comdiv.Application;
using Comdiv.Zeta.Data.Minimal;
using Comdiv.Zeta.Model;
using NUnit.Framework;

namespace Zeta.Extreme.Core.Tests.CoreTests {
	[TestFixture(Description = "Начинаем работать с простыми формами")]
	public class ZatrSimpleTest : SessionTestBase {
		public row[] rows;
		public IZetaColumn col;
		public int[] periods;
		public IZetaMainObject obj;

		[TestFixtureSetUp]
		public override void FixtureSetup()
		{
			base.FixtureSetup();
			rows = (from row in myapp.storage.AsQueryable<row>()
			        where row.Path.Contains("/m260/")
			        orderby row.Path
			        select row).ToArray();
			;
			col = ColumnCache.get("Б1");
			periods = new[] { 11, 12, 13, 1, 401, 403 };
			obj = myapp.storage.AsQueryable<IZetaMainObject>().First(x => x.Id == 352);
		}


		[Test]
		[Repeat(3)]
		public void Register_In_Simple_To_Complex_Way() {
			var sw = Stopwatch.StartNew();
			foreach (var row in rows
				//.Where(x=>!x.IsMarkSeted("0CAPTION")) -- if choosed Stat_Registry_Preprocessed == 1032
				// but we have to be less
				.OrderBy(x=>
					{
						if(x.IsFormula) return 100;
						if(x.HasChildren()) return 5+x.Level;
						return x.Level;
					}
				)) {
				foreach (var period in periods) {
					var q = new ZexQuery
						{
							Row = {Native = row},
							Col = {Native = col},
							Time = {Year = 2012, Period = period},
							Obj = {Native = obj}
						};
					session.RegisterAsync(q, string.Format("{0}_{1}_{2}_{3}", row.Code, col.Code, 2012, period));
				}
			}

			session.WaitPreparation();
			session.WaitEvaluation();

			sw.Stop();

			Console.WriteLine(sw.ElapsedMilliseconds);
			Assert.AreEqual(996,session.MainQueryRegistry.Where(x=>x.Key!=x.Value.GetCacheKey()).Count());
			Assert.AreEqual(1038,session.Stat_Registry_Started_User);
			Assert.AreEqual(1038 
				- CAPT_COUNT * periods.Length
				- OBS_COUNT * periods.Length 
				,session.Stat_Registry_User);

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

			var q1 = session.MainQueryRegistry["test"];
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
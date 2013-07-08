using System;
using System.Linq;
using NUnit.Framework;
using Zeta.Extreme.Core.Tests.CoreTests;

namespace Zeta.Extreme.Core.Tests {
	/// <summary>
	/// many issues found here
	/// </summary>
	[TestFixture]
	public class Zc478Problem : SessionTestBase {
		[Test]
		public void ZC478_f120420_SNDS()
		{
			var result = _serial.Eval(new Query
				{
					Row = { Code = "f120420" },
					Col = { Code = "SNDS" },
					Time = { Year = 2013, Period = 251 },
					Obj = { Id = 449 },
				});
			Assert.True(result.IsComplete);
			Assert.Null(result.Error);
			Assert.AreEqual("255", result.NumericResult.ToString().Substring(0, 3));
		}

		[Test]
		public void ZC478_f120420_PLAN()
		{
			var result = _serial.Eval(new Query
				{
					Row = { Code = "f120420" },
					Col = { Code = "PLAN" },
					Time = { Year = 2013, Period = 251 },
					Obj = { Id = 449 },
				});
			Assert.True(result.IsComplete);
			Assert.Null(result.Error);
			Assert.AreEqual("240", result.NumericResult.ToString().Substring(0, 3));
		}

		[Test]
		public void ZC478_f1102319_PLAN_ALTDIVPROBLEM()
		{
			var q = session.Register(new Query
				{
                    Row = { Code = "f1102318" },
					Col = { Code = "PLAN" },
					Time = { Year = 2013, Period = 251 },
					Obj = { Id = 449 },
				});
			session.WaitPreparation();
			var result = _serial.Eval(q);
            if(null!=result.Error)Console.WriteLine(result.Error);
			Assert.True(result.IsComplete);
			Assert.Null(result.Error);

		}

		[Test]
		public void ZC478_f120310()
		{

			var query = (Query)session.Register(new Query
				{
					Row = { Code = "f120310" },
					Col = { Code = "SNDS" },
					Time = { Year = 2013, Period = 251 },
					Obj = { Id = 449 },
				});
			session.WaitPreparation();
			var result = session.AsSerial().Eval(query);
			Assert.True(result.IsComplete);
			Assert.AreEqual(11574000m, result.NumericResult);
		}

		[Test]
		public void ZC478_f120310_PLAN_ONLY()
		{

			var query = (Query)session.Register(new Query
			{
				Row = { Code = "f120310" },
				Col = { Code = "PLAN" },
				Time = { Year = 2013, Period = 251 },
				Obj = { Id = 449 },
			});
			session.WaitPreparation();
			var result = session.AsSerial().Eval(query);
			Assert.True(result.IsComplete);
			Assert.AreEqual(11574000m, result.NumericResult);
		}

		[Test]
		public void ZC478_f120310_NDSLINK_ONLY()
		{

			var query = (Query)session.Register(new Query
			{
				Row = { Code = "f120310" },
				Col = { Code = "NDSLINK" },
				Time = { Year = 2013, Period = 251 },
				Obj = { Id = 449 },
			});
			session.WaitPreparation();
			var result = session.AsSerial().Eval(query);
			Assert.True(result.IsComplete);
			Assert.AreEqual(0m, result.NumericResult);
		}

		[Test]
		[Timeout(3000)]
		public void ZC478_f120310_HANGUP() {
			var query = (Query)session.Register(new Query
				{
					Row = { Code = "f120310" },
					Col = { Code = "SNDS" },
					Time = { Year = 2013, Period = 251 },
					Obj = { Id = 449 },
				});
			session.WaitPreparation();
			session.AsSerial().Eval(query);
		}


		[Test]
		public void ZC478_f120210()
		{

			var query = (Query)session.Register(new Query
				{
					Row = { Code = "f120210" },
					Col = { Code = "SNDS" },
					Time = { Year = 2013, Period = 251 },
					Obj = { Id = 449 },
				});
			var result = session.AsSerial().Eval(query);
			Assert.True(result.IsComplete);
			Assert.AreEqual(0m, result.NumericResult);
		}

		[Test]
		public void ZC478_f120110()
		{

			var query = (Query)session.Register(new Query
				{
					Row = { Code = "f120110" },
					Col = { Code = "SNDS" },
					Time = { Year = 2013, Period = 251 },
					Obj = { Id = 449 },
				});
			var result = session.AsSerial().Eval(query);
			Assert.True(result.IsComplete);
			Assert.AreEqual("140", result.NumericResult.ToString().Substring(0, 3));
		}

		[Test]
		public void ZC478_f110201()
		{

			var query = (Query)session.Register(new Query
				{
					Row = { Code = "f110201" },
					Col = { Code = "SNDS" },
					Time = { Year = 2013, Period = 251 },
					Obj = { Id = 449 },
				});
			var result = session.AsSerial().Eval(query);
			Assert.AreEqual(QueryEvaluationType.Formula, query.EvaluationType);
			Assert.AreEqual(2, query.FormulaDependency.Count);
			Assert.True(query.FormulaDependency.Any(_ => _.Col.Code == "Á1"));
			Assert.True(query.FormulaDependency.Any(_ => _.Col.Code == "NDSLINK"));
			Assert.AreEqual("140", result.NumericResult.ToString().Substring(0,3));
		}


		[Test]
		public void ZC478_f110201_PLAN()
		{

			var query = (Query)session.Register(new Query
				{
					Row = { Code = "f110201" },
					Col = { Code = "PLAN" },
					Time = { Year = 2013, Period = 251 },
					Obj = { Id = 449 },
				});
			var result = session.AsSerial().Eval(query);
			Assert.AreEqual(12514869m, result.NumericResult);
		}

	}
}
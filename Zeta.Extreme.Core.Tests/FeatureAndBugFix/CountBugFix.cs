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
// PROJECT ORIGIN: Zeta.Extreme.Core.Tests/CountBugFix.cs
#endregion
using System;
using NUnit.Framework;
using Zeta.Extreme.Core.Tests.CoreTests;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.MetaCaches;
using Zeta.Extreme.Model.Querying;
using Zeta.Extreme.Primary;

namespace Zeta.Extreme.Core.Tests {
	[TestFixture]
	public class CountBugFix :SessionTestBase{

		[Test]
		public void Check_Nested_EXREF()
		{
			var result = _serial.Eval(new Query
			{
				Row = { Code = "m224652" },
				Col = { Code = "NOB18" },
				Time = { Year =2013, Period = 251 },
				Obj = { Id = 449 },
			});
			Assert.AreEqual(905130m, result.NumericResult);
		}

	


		[Test]
		public void ZC_273_NO_INSUMMA_DOUBLING() {
			var result = _serial.Eval(new Query
				{
					Row = { Code = "m1111130" },
					Col = {Code = "PLAN"},
					Time = { Year = 2013, Period = 251 },
					Obj = {Id=1067},
				});
			Assert.AreEqual(46568901m, result.NumericResult);
		}

		[Test]
		public void ZC_233_Bug_PLANSNG1_Cause_0_Year_InPrimary_Test_One_Normalization()
		{
			var query = new Query
				{
					Row = { Code = "m2601311" },
					Col = { Code = "PLANSNG1" },
					Time = { Year = 2012, Period = 13 }
				};



			query = (Query)session.Register(query);
			query.WaitPrepare();
			Assert.AreEqual(1, query.FormulaDependency.Count);
			Assert.AreEqual("m2601311", query.FormulaDependency[0].Row.Code);
			Assert.AreEqual("�1", query.FormulaDependency[0].Col.Code);
			Assert.AreEqual(303, query.FormulaDependency[0].Time.Period);
			Assert.AreEqual(2012, query.FormulaDependency[0].Time.Year);
		}


		[Test]
		[Ignore("data changed")]
		public void ZC_252_BUG_NO_SUPPORT_DOSUM()
		{
			/*
			 * 
			 * ��������, ��� ������ m220912 ����������� 1067 ������ 251 ��� 2013 ��� ������� Ok ����� 0, � ������ 249 516
			 */
			var query = new Query
				{
					Obj = { Id = 1067 },
					Row = { Code = "m220912" },
					Col = { Code = "Ok" },
					Time = { Year = 2013, Period = 251 }
				};



			query = (Query)session.Register(query);
			query.WaitPrepare();

			_serial.Eval(query);
			Assert.AreEqual(249516m, query.Result.NumericResult);
		}

		[Test]
		[Ignore("������ ����������")]
		public void BUG_KRU_INVALID_VALUE()
		{
			/*
			 * 
			 * ��������, ��� ������ m220912 ����������� 1067 ������ 251 ��� 2013 ��� ������� Ok ����� 0, � ������ 249 516
			 */
			var query = new Query
			{
				Obj = { Id = 1067 },
				Row = { Code = "m2301000" },
				Col = { Code = "DZk" },
				Time = { Year = 2013, Period = 251 }
			};



			query = (Query)session.Register(query);
			query.WaitPrepare();

			_serial.Eval(query);
			Assert.AreEqual(-124473m, query.Result.NumericResult);
		}

		

		[Test]
		public void ZC_233_Bug_PLANSNG1_InvalidData()
		{
			var query = new Query
				{
					Row = { Code = "m2601311" },
					Col = { Code = "PLANSNG1" },
					Time = { Year = 2012, Period = 1 },
					Obj = { Id = 352 }
				};



			query = (Query)session.Register(query);
			query.WaitPrepare();
			Assert.AreEqual(1, query.FormulaDependency.Count);
			Assert.AreEqual("m2601311", query.FormulaDependency[0].Row.Code);
			Assert.AreEqual("�1", query.FormulaDependency[0].Col.Code);
			Assert.AreEqual(303, query.FormulaDependency[0].Time.Period);
			Assert.AreEqual(2012, query.FormulaDependency[0].Time.Year);

			var result = _serial.Eval(query);
			Assert.AreEqual(104343m, result.NumericResult);
		}

		[Test]
		public void ZC_199_Not_Count_m260722()
		{
			IQuery query = new Query
				{
					Obj = { Id = 352 },
					Row = { Code = "m260722" },
					Col = { Code = "�1" },
					Time = { Year = 2012, Period = 3 },
					Session = session,

				};
			query = session.Register(query);
			var res = _serial.Eval(query);
			Assert.AreEqual(-89485m, res.NumericResult);
		}

		[Test]
		public void ZC_236_Invalid_m501_Period_Normalization()
		{
			var query = new Query
				{
					Obj = { Id = 352 },
					Row = { Code = "m2601311" },
					Col = { Code = "�1" },
					Time = { Year = 2012, Period = 13 },
					Session = session,

				};
			query = (Query) new QueryDelta { Period = -501 }.Apply(query);
			query.Normalize();
			Console.WriteLine(query);
			Assert.NotNull(query.Time.Periods);
			Assert.AreEqual(3, query.Time.Periods.Length);
			Assert.True(query.IsPrimary);
			Assert.AreEqual(111213, query.Time.Period);

			var perssource = session.PrimarySource as DefaultPrimarySource;
			var script = perssource.GenerateScript(new[] { query });
			Console.WriteLine(script);

			var res = _serial.Eval(query);
			Assert.AreEqual(77732m, res.NumericResult);

		}

		[Test]
		public void ZC_235_Invalid_Result_In_Control()
		{
			var query = new Query
				{
					Obj = { Id = 352 },
					Row = { Code = "m2601311" },
					Col = { Code = "CUSTOM", Formula = " @�1.P-201? - @�1.P-501? ", IsFormula = true, FormulaType = "boo" },
					Time = { Year = 2012, Period = 13 },
					Session = session,

				};
			session.Register(query);
			var res = _serial.Eval(query);
			Assert.AreEqual(0, res.NumericResult);

		}

		[Test]
		public void ZC_427_OZHIDPREDGOD_NODOSUM_ROW() {
			var query = new Query
			{
				Obj = { Id = 473 },
				Row = { Code = "m260721" },
				Col = { Code = "OZHIDPREDGOD" },
				Time = { Year = 2012, Period = 251 },
				Session = session,

			};
			query = (Query) session.Register(query);
			var res = _serial.Eval(query);
			Assert.AreNotEqual(0, res.NumericResult);
		}
	}
}
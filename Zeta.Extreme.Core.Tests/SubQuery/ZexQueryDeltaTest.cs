using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Zeta.Extreme.Core.Tests.SubQuery
{
	

	[TestFixture]
	public class ZexQueryDeltaTest
	{
		[Test]
		public void RowMove() {
			var q = new ZexQuery {Row = {Code = "X"}};
			var d = new ZexQueryDelta {RowCode = "Y"};
			var dq = d.Apply(q);
			Assert.AreNotSame(q,dq);
			Assert.AreSame(q.Col,dq.Col);
			Assert.AreNotSame(q.Row,dq.Row);
			Assert.AreEqual("Y",dq.Row.Code);
		}
		[Test]
		public void ColMove()
		{
			var q = new ZexQuery { Col = { Code = "X" } };
			var d = new ZexQueryDelta { ColumCode = "Y" };
			var dq = d.Apply(q);
			Assert.AreNotSame(q, dq);
			Assert.AreSame(q.Row, dq.Row);
			Assert.AreNotSame(q.Col, dq.Col);
			Assert.AreEqual("Y", dq.Col.Code);
		}

		[Test]
		public void ObjMove()
		{
			var q = new ZexQuery { Obj = { Id = 24 } };
			var d = new ZexQueryDelta { ObjId = 35 };
			var dq = d.Apply(q);
			Assert.AreNotSame(q, dq);
			Assert.AreSame(q.Row, dq.Row);
			Assert.AreNotSame(q.Obj, dq.Obj);
			Assert.AreEqual(35, dq.Obj.Id);
		}

		[Test]
		public void YearMove()
		{
			var q = new ZexQuery { Time = { Year = 2012 } };
			var d = new ZexQueryDelta { Year = 1 };
			var dq = d.Apply(q);
			Assert.AreNotSame(q, dq);
			Assert.AreSame(q.Row, dq.Row);
			Assert.AreNotSame(q.Time, dq.Time);
			Assert.AreEqual(2013, dq.Time.Year);
		}

		[Test]
		public void YearPeriodMove()
		{
			var q = new ZexQuery { Time = { Year = 2012 } };
			var d = new ZexQueryDelta { Year = 2014,Period = 1};
			var dq = d.Apply(q);
			Assert.AreNotSame(q, dq);
			Assert.AreSame(q.Row, dq.Row);
			Assert.AreNotSame(q.Time, dq.Time);
			Assert.AreEqual(2014, dq.Time.Year);
			Assert.AreEqual(1, dq.Time.Period);
		}

		[Test]
		[Explicit]
		public void BenchMarkTest() {
			var sw = Stopwatch.StartNew();
			int i=0;
			for(var r = 0;r<500;r++) { //rowcodes
				for(var c = 0;c<10;c++) { //колонки
					for(var o=1;o<15;o++) { //объекты
						for (var y=2010;y<2014;y++) { //годы
							for(var p=0;p<10;p++) { //периоды
								i++;
								var q = new ZexQuery
									{
										Row = {Code = r.ToString()},
										Col = {Code = c.ToString()},
										Obj = {Id = o},
										Time = {Year = y, Period = p}
									};
								var d = new ZexQueryDelta
									{
										RowCode = r + "+",
										ColumCode = c + "+",
										ObjId = o + 10,
										Year = y + 1,
										Period = p + 1
									};
								q = d.Apply(q);
								//Assert.AreEqual(r+"+",q.Row.Code);
								//Assert.AreEqual(c + "+", q.Col.Code);
								//Assert.AreEqual(o + 10, q.Obj.Id);
								//Assert.AreEqual(y+1,q.Time.Year);
								//Assert.AreEqual(p + 1, q.Time.Period);
							}
						}
					}
				}
			}
			sw.Stop();
			Console.WriteLine(i);
			Console.WriteLine(sw.Elapsed);
			Console.WriteLine(sw.ElapsedMilliseconds/(double)i);
		}
	}
}

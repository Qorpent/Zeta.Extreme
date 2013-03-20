using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Zeta.Extreme.Core.Tests.CoreTests;
using Zeta.Extreme.Model.MetaCaches;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.Core.Tests.ControlpointBug
{
	[TestFixture]
	public class ControlpointAllTests:SessionTestBase
	{
		[Test]
		public void CanEvalAllControlpointsInAtomicSerialMode() {
			var allcontrolpoints = RowCache.Byid.Values.Where(_ => _.MarkCache.Contains("/CONTROLPOINT/")).ToArray();
			var waserror = false;
			foreach (var c in allcontrolpoints) {
				Console.Write(c.Code+":");
				var s = new Session(true);
				var ser = s.AsSerial();
				var q = new Query
					{
						Row = {Native = c},
						Col = {Native = ColumnCache.Bycode["CONTROLPD1"]},
						Time = {Year = 2013, Period = 251},
						Obj = {Native = ObjCache.ObjById[357]}
					};
				try {
					var result = ser.Eval(q, 10000);
					Console.Write("{0}:{1}:{2}", result.IsComplete, result.NumericResult, result.Error);
					if (null != result.Error) waserror = true;
				}
				catch (Exception e) {
					Console.Write("error:"+e.Message);
					waserror = true;
				}
				Console.Write("--"+s.Statistics.BatchTime);
				Console.WriteLine();
			}
			if (waserror) {
				Assert.Fail();
			}
		}
		[Test]
		public void SingleSessionSerial()
		{
			var allcontrolpoints = RowCache.Byid.Values.Where(_ => _.MarkCache.Contains("/CONTROLPOINT/")).ToArray();
			var waserror = false;
			var s = new Session(true);
			var ser = s.AsSerial();
			foreach (var c in allcontrolpoints)
			{
				Console.Write(c.Code + ":");
				
				var q = new Query
				{
					Row = { Native = c },
					Col = { Native = ColumnCache.Bycode["CONTROLPD1"] },
					Time = { Year = 2013, Period = 251 },
					Obj = { Native = ObjCache.ObjById[357] }
				};
				try
				{
					var result = ser.Eval(q, 10000);
					Console.Write("{0}:{1}:{2}", result.IsComplete, result.NumericResult, result.Error);
					if (null != result.Error) waserror = true;
				}
				catch (Exception e)
				{
					Console.Write("error:" + e.Message);
					waserror = true;
				}
				Console.Write("--" + s.Statistics.BatchTime);
				Console.WriteLine();
			}
			if (waserror)
			{
				Assert.Fail();
			}
		}

		[Test]
		public void SingleSession()
		{
			var allcontrolpoints = RowCache.Byid.Values.Where(_ => _.MarkCache.Contains("/CONTROLPOINT/")).ToArray();
			var s = new Session(true);
			foreach (var c in allcontrolpoints)
			{
				Console.Write(c.Code + ":");

				var q = new Query
				{
					Row = { Native = c },
					Col = { Native = ColumnCache.Bycode["CONTROLPD1"] },
					Time = { Year = 2013, Period = 251 },
					Obj = { Native = ObjCache.ObjById[357] }
				};
				s.RegisterAsync(q);
			}
			((ISession) s).Execute();
		}

		[Test]
		public void m260900_Must_Not_Have_Errors() {
			var q = new Query
			{
				Row = { Native = RowCache.Bycode["M260900"] },
				Col = { Native = ColumnCache.Bycode["CONTROLPD1"] },
				Time = { Year = 2013, Period = 251 },
				Obj = { Native = ObjCache.ObjById[357] }
			};
			var s = new Session(true);
			var ser = s.AsSerial();
			var result = ser.Eval(q);
			Assert.Null(result.Error);
		}

		[Test]
		[Timeout(3000)]
		public void f110610_HangUp() {
			var q = new Query
			{
				Row = { Native = RowCache.Bycode["F110610"] },
				Col = { Native = ColumnCache.Bycode["CONTROLPD1"] },
				Time = { Year = 2013, Period = 251 },
				Obj = { Native = ObjCache.ObjById[357] }
			};
			var s = new Session(true);
			var ser = s.AsSerial();
			var result = ser.Eval(q);
			Assert.Null(result.Error);
		}


		[Test]
		public void f110610_Analyze()
		{
			var q = new Query
			{
				Row = { Native = RowCache.Bycode["F110610"] },
				Col = { Native = ColumnCache.Bycode["CONTROLPD1"] },
				Time = { Year = 2013, Period = 251 },
				Obj = { Native = ObjCache.ObjById[357] }
			};
			var s = new Session(true);
			var ser = s.AsSerial();
			var sw = Stopwatch.StartNew();
			var result = ser.Eval(q);
			Console.WriteLine(s.Statistics.BatchTime);
			Console.WriteLine(s.Statistics.RegistryPreprocessed);
			sw.Stop();
			Console.WriteLine(sw.Elapsed);
		}

		[Test]
		[Timeout(3000)]
		
		public void f110610_HangUp_MustBeProcessed_WithInternalTimeoutCheck()
		{
			var q = new Query
			{
				Row = { Native = RowCache.Bycode["F110610"] },
				Col = { Native = ColumnCache.Bycode["CONTROLPD1"] },
				Time = { Year = 2013, Period = 251 },
				Obj = { Native = ObjCache.ObjById[357] }
			};
			var s = new Session(true);
			var ser = s.AsSerial();
			try {
				var result = ser.Eval(q, 2000);
			}
			catch {
				
			}

		}
	}
}

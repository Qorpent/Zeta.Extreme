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
// PROJECT ORIGIN: Zeta.Extreme.Core.Tests/SerialAcessTest.cs
#endregion
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using NUnit.Framework;
using Zeta.Extreme.Core.Tests.CoreTests;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.MetaCaches;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.Core.Tests {
	[TestFixture]
	public class SerialAcessTest : SessionTestBase {
		[SetUp]
		public override void setup() {
			base.setup();
			_row = RowCache.get("m112");
			_col = ColumnCache.get("PLAN");
			_obj = MetaCache.Default.Get<IZetaMainObject>(352);//myapp.storage.Get<IZetaMainObject>().Load(352);
			_serial = session.AsSerial();
		}

		private IZetaRow _row;
		private IZetaColumn _col;
		private IZetaMainObject _obj;
		private ISerialSession _serial;

		private IEnumerable<Query> _getQueries() {
			foreach (var r in _row.AllChildren) {
				yield return
					new Query {Row = {Native = r}, Col = {Native = _col}, Obj = {Native = _obj}, Time = {Year = 2012, Period = 301}}
					;
			}
		}

		private decimal RunSync(int lag = 0) {
			session = new Session(true);
			var controlsum = 0m;
			foreach (var q in _getQueries()) {
				var result = _serial.Eval(q).NumericResult;
				Thread.Sleep(lag);
				controlsum += result;
				//Console.WriteLine(q.Row.Code+" "+result);
			}
			return controlsum;
		}

		private decimal RunAsync(int lag = 0) {
			var controlsum = 0m;
			foreach (var q in _getQueries()) {
				var t = _serial.EvalAsync(q);
				//Console.Write(q.Row.Code+" ");
				Thread.Sleep(lag);
				t.Wait();
				var result = t.Result ?? new QueryResult();
				//Console.WriteLine(result.NumericResult);
				controlsum += result.NumericResult;
			}


			return controlsum;
		}

		[Test]
		public void AsynchronousModeWorks() {
			var controlsum = RunAsync();
			Console.WriteLine(controlsum);
		}

		[Test]
		public void SynchronousModeWorks() {
			var controlsum = RunSync();
			Console.WriteLine(controlsum);
		}

		[Test]
		[Explicit]
		public void TestBenchmark() {
			RunSync(); //remove cache issues
			var sw = Stopwatch.StartNew();
			for (var i = 0; i < 50; i++) {
				Console.Write(".");
				RunSync(20);
			}
			sw.Stop();
			Console.WriteLine();
			Console.WriteLine("====================");
			Console.WriteLine();
			var synctime = sw.Elapsed;
			Console.WriteLine("synctime: " + synctime);

			sw = Stopwatch.StartNew();
			for (var i = 0; i < 50; i++) {
				Console.Write(".");
				RunAsync(20);
			}
			sw.Stop();
			var asynctime = sw.Elapsed;
			Console.WriteLine();
			Console.WriteLine("====================");
			Console.WriteLine();
			Console.WriteLine("asynctime: " + asynctime);
		}
	}
}
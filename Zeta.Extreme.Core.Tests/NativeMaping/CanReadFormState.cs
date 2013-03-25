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
// PROJECT ORIGIN: Zeta.Extreme.Core.Tests/CanReadFormState.cs
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Zeta.Extreme.Core.Tests.CoreTests;
using Zeta.Extreme.Model.SqlSupport;

namespace Zeta.Extreme.Core.Tests.NativeMaping
{

	[TestFixture]
	public class ZC438NativeCellReading:SessionTestBase
	{
		[Test]
		public void CanReadExistedCell() {
			var reader = new NativeZetaReader();
			var cells = reader.GetCells("Id = " + 6204357).ToArray();
			Assert.AreEqual(1,cells.Length);
			var cell = cells[0];
			Assert.AreEqual(6204357,cell.Id);
			Assert.AreEqual("2013-03-25", cell.Version.ToString("yyyy-MM-dd"));
			Assert.AreEqual(2013,cell.Year);
			Assert.AreEqual(11, cell.Period);
			Assert.AreEqual("z2501712", cell.Row.Code);
			Assert.AreEqual("SUMMA", cell.Column.Code);
			Assert.AreEqual(536,cell.Object.Id);
			Assert.AreEqual("RUB", cell.Currency);
			Assert.AreEqual(7703m, cell.NumericValue);
			Assert.AreEqual("7703", cell.StringValue);
			Assert.AreEqual("ugmk\\intro.elzink24",cell.User);
		}
	}

	[TestFixture]
	public class ZC439NativeCellHistoryReading:SessionTestBase {
		[Test]
		public void CanReadHistoryOfCell() {
			var reader = new NativeZetaReader();
			var history = reader.GetCellHistory(6204487).ToArray();
			Assert.Greater(history.Length,2);
			Assert.True(history.All(_ => _.CellId == 6204487));
			Assert.True(history.Any(_ => _.Value == "0,464"));
			
		}
	}

	[TestFixture]
	public class CanReadFormState:SessionTestBase
	{
		[Test]
		public void CanReadStateHistory() {
			var states =
				new NativeZetaReader().ReadFormStates(
					"Year = 2012 and Period = 3 and LockCode='sm111' and Object = 352 order by Version").ToArray();
			Assert.AreEqual(2,states.Length);
			Assert.AreEqual("0ISBLOCK",states[0].State);
			Assert.AreEqual("0ISCHECKED", states[1].State);
		}
	}
}

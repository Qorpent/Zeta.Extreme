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
// PROJECT ORIGIN: Zeta.Extreme.Core.Tests/CanCalculateValuta.cs
#endregion
using System;
using NUnit.Framework;
using Zeta.Extreme.Core.Tests.CoreTests;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.Core.Tests {
	[TestFixture]
	public class CanCalculateValuta : SessionTestBase
	{
		[Test]
		public void SimpleUsdCalculation()
		{
			var query = new Query
				{
					Row = { Code = "m260113" },
					Col = { Code = "�1" },
					Obj = { Id = 352, Type = ZoneType.Obj },
					Time = { Year = 2012, Period = 1 }
				}
				;
			var result = _serial.Eval(query);
			var rubresult = result.NumericResult;
			query = new Query
				{
					Row = {Code = "m260113"},
					Col = {Code = "�1"},
					Obj = {Id = 352, Type = ZoneType.Obj},
					Time = {Year = 2012, Period = 1},
					Currency = "USD",
				};
			result = _serial.Eval(query);
			const decimal usdrate = 29.32820m;
			Assert.AreEqual(Math.Round(rubresult /usdrate, 6), result.NumericResult);
		}
	}
}
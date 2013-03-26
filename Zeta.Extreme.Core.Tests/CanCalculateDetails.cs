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
// PROJECT ORIGIN: Zeta.Extreme.Core.Tests/CanCalculateDetails.cs
#endregion
using NUnit.Framework;
using Zeta.Extreme.Core.Tests.CoreTests;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.Core.Tests {
	[TestFixture]
	public class CanCalculateDetails:SessionTestBase {
		[Test]
		public void SimpleDetailCount() {
			var query = new Query
				{
					Row = {Code="m1303111"},
					Col = {Code="Á1"},
					Obj = {Id=236, Type = ZoneType.Detail},
					Time= {Year = 2012,Period = 1}
				}
				;
			var result = _serial.Eval(query);
			Assert.AreEqual(93.89m, result.NumericResult);
		}
	}
}
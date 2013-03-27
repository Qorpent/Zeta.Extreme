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

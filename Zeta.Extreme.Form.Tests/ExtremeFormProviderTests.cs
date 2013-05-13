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
// PROJECT ORIGIN: Zeta.Extreme.Form.Tests/ExtremeFormProviderTests.cs
#endregion
using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using Zeta.Extreme.Form.Themas;

namespace Zeta.Extreme.Form.Tests
{
	[TestFixture]
	[Explicit]
	public class ExtremeFormProviderTests
	{
		private ExtremeFormProvider _efp;

		[TestFixtureSetUp]
		public void FixtureSetup() {
			_efp = new ExtremeFormProvider(@"c:\apps\eco\tmp\compiled_themas","");

		}
		[TestCase("prib2011A.in",true)]
		[TestCase("balans2011A.in",true)]
		[TestCase("zatrA.in",true)]
		[TestCase("free_activeA.in",true)]
		public void CanAccessThemas(string code, bool existed) {
			var form = _efp.Get(code);
			if(existed) {
				Assert.NotNull(form);
			}else {
				Assert.Null(form);
			}
		}
		[Test]
		[Ignore("Больше не лимиитируем")]
		public void LimitThemaSet()
		{
			Assert.Greater(125,_efp.Factory.GetAll().Count());
		}

		[Test]
		[Explicit]
		public void TimeToReloadMustBeMinimal() {
			var sw = Stopwatch.StartNew();
			for(var i=0;i<20;i++) {
				_efp.Reload();
				var f = _efp.Factory;
			}
			sw.Stop();
			Console.WriteLine(sw.Elapsed);
			Assert.Less(sw.ElapsedMilliseconds,20000);
		} 
	}
}

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
// PROJECT ORIGIN: Zeta.Extreme.Form.Tests/InternalZC371Tests.cs
#endregion
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Zeta.Extreme.Form.Tests.ZC371TestBench
{
	[TestFixture]
	public class InternalZC371Tests
	{
		[Test]
		[Explicit]
		public void ConditionSetGeneratorTest() {
			//проверяем, что действительно возвращаются все варианты массивов
			var conditions = new[] {"A", "B", "C"};
			IList<string> results = new List<string>();
			var generator = new ConditionSetGenerator(conditions);
			foreach (var subresult in generator.GenerateConditionSets()) {
				results.Add(string.Join(",",subresult));
			}
			Assert.AreEqual(8,results.Count);
			CollectionAssert.Contains(results,"");
			CollectionAssert.Contains(results, "A");
			CollectionAssert.Contains(results, "B");
			CollectionAssert.Contains(results, "C");
			CollectionAssert.Contains(results, "A,B");
			CollectionAssert.Contains(results, "B,C");
			CollectionAssert.Contains(results, "A,C");
			CollectionAssert.Contains(results, "A,B,C");
		}
		[Test]
		[Explicit]
		public void ConditionSetGeneratorTestFromConditionString() {
			//проверяем, что действительно возвращаются все варианты массивов
			IList<string> results = new List<string>();
			var generator = new ConditionSetGenerator("A and B or C and not A and not B");
			foreach (var subresult in generator.GenerateConditionSets()) {
				results.Add(string.Join(",",subresult));
			}
			Assert.AreEqual(8,results.Count);
			CollectionAssert.Contains(results,"");
			CollectionAssert.Contains(results, "A");
			CollectionAssert.Contains(results, "B");
			CollectionAssert.Contains(results, "C");
			CollectionAssert.Contains(results, "A,B");
			CollectionAssert.Contains(results, "B,C");
			CollectionAssert.Contains(results, "A,C");
			CollectionAssert.Contains(results, "A,B,C");
		}
	}
}

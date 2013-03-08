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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Zeta.Extreme.Developer.Tests
{
	[TestFixture]
	public class CodeIndexTest : CodeIndexBasedTest {
		[TestCase("a/b",9)]
		[TestCase("c/b", 1)]
		[TestCase("a", 2)]
		public void FindAttributes_Method_BasicTest(string root,int count) {
			var result = index.FindAttributes(simplesource, root).ToArray();
			Assert.AreEqual(count,result.Length);
			Assert.True(result.All(_=>_.LexInfo.File=="simple"));
			Assert.True(result.All(_=>_.LexInfo.Line!=0));
			Assert.True(result.All(_ => !string.IsNullOrWhiteSpace(_.Value)));
			Assert.True(result.All(_=>!string.IsNullOrWhiteSpace(_.LexInfo.Context)));
		}

		[Test]
		public void FindAttributes_Array_Method_BasicTest()
		{
			var result = index.FindAttributes(simplesource, new[]{"a/b","c/b"}).ToArray();
			Assert.AreEqual(10, result.Length);
			
		}

		[Test]
		public void GetAttributes_Works() {
			var result = index.GetAttributes(new[] {"a/b"}).ToArray();
			Assert.AreEqual(4,result.Length);
			Assert.True(result.All(_=>_.ValueVariants.Count>0));
		}

		[Test]
		public void GetAttributes_Works_WithCache()
		{
			var result = index.GetAttributes(new[] { "a/b","c/b" });
			
			var  result2 = index.GetAttributes(new[] { "a/b" ,"c/b"});
			Assert.AreSame(result,result2);
			var result3 = index.GetAttributes(new[] {"c/b", "a/b" });
			Assert.AreSame(result,result3);
			var result4 = index.GetAttributes(new[] { "c/b", "a/b","a/b" });
			Assert.AreSame(result, result4);
			var result5 = index.GetAttributes(new[] { "c/b", "a/b", "a" });
			Assert.AreNotSame(result, result5);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Qorpent.Serialization;
using Zeta.Extreme.Developer.Analyzers;
using Zeta.Extreme.Developer.Model;

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
		public void BugWithReferenceGroupping() {
			var group =
				CodeIndex.GroupReferences(new[] {
					new ItemReference {File = "A", MainContext = "A", SubContext = "B", Line = 1},
					new ItemReference {File = "A", MainContext = "A", SubContext = "C", Line = 2}
				}).ToArray();
			Assert.AreEqual(1,group.Length);
			Assert.AreEqual("A",group[0].File);
			Assert.AreEqual("A", group[0].MainContext);
			Assert.AreEqual(null, group[0].SubContext);
			Assert.AreEqual(0, group[0].Line);

			Assert.AreEqual(2,group[0].Children.Count());

			Assert.AreEqual(null,group[0].Children.ElementAt(0).File);
			Assert.AreEqual(null,group[0].Children.ElementAt(0).MainContext);
			Assert.AreEqual("B", group[0].Children.ElementAt(0).SubContext);
			Assert.AreEqual(1, group[0].Children.ElementAt(0).Line);

			Assert.AreEqual(null, group[0].Children.ElementAt(1).File);
			Assert.AreEqual(null, group[0].Children.ElementAt(1).MainContext);
			Assert.AreEqual("C", group[0].Children.ElementAt(1).SubContext);
			Assert.AreEqual(2, group[0].Children.ElementAt(1).Line);
		}


		[Test]
		public void BugWithReferenceGrouppingInvalidXml()
		{
			var group =
				CodeIndex.GroupReferences(new[] {
					new ItemReference {File = "A", MainContext = "A", SubContext = "B", Line = 1},
					new ItemReference {File = "A", MainContext = "A", SubContext = "C", Line = 2}
				}).ToArray();
			var variant = new AttributeValueVariant {Value = "X"};
			foreach (var g in group) {
				variant.References.Add(g);
			}
			var attr = new AttributeDescriptor {Name = "x"};
			attr.ValueVariants.Add(variant);
			var result = new XmlSerializer().Serialize("test",attr);
			Console.WriteLine(result);
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

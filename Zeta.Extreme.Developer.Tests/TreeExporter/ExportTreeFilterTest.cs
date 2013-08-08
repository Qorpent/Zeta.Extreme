using System.Linq;
using NUnit.Framework;
using Qorpent;
using Zeta.Extreme.Developer.MetaStorage;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Developer.Tests.TreeExporter
{
	/// <summary>
	/// Проверяем фильтрацию дерева
	/// </summary>
	[TestFixture]
	public class ExportTreeFilterTest {
		private ExportTreeFilter filter = new ExportTreeFilter();
		[SetUp]
		public void SetUp() {
			filter = new ExportTreeFilter();
		}
		[Test]
		public void DoesNotApplyToSource() {
			var root = new Row();
			var root2 = filter.Execute(root);
			Assert.AreNotSame(root,root2);
		}
		[Test]
		public void CanRewriteCode() {
			filter.CodeReplacer = new ReplaceDescriptor {Pattern = "a", Replacer = "b"};
			var root = new Row {Code = "a1a"};
			root.Children.Add(new Row {Code = "a2a"});
			root.Children.Add(new Row {Code = "a3a"});
			var result = filter.Execute(root);
			CollectionAssert.AreEquivalent(
				new[]{"a1a","a2a","a3a"}, 
				new[]{root,root.Children.First(),root.Children.ElementAt(1)}.Select(_=>_.Code).ToArray()  );
			CollectionAssert.AreEquivalent(
				new[] { "b1b", "b2b", "b3b" },
				new[] { result, result.Children.First(), result.Children.ElementAt(1) }.Select(_ => _.Code).ToArray());

		}

		[Test]
		public void CanRemoveSubTreeByMark() {
			filter.ExcludeRegex = "X";
			var root = new Row { Code = "a" };
			IZetaRow c1 = null;
			IZetaRow c2 = null;
			root.Children.Add(c1 = new Row { Code = "b" });
			root.Children.Add(c2 = new Row { Code = "c", MarkCache = "/X/"});
			c1.Children.Add(new Row {Code = "d"});
			c2.Children.Add(new Row {Code = "e"});
			root.ResetAllChildren();
			Assert.AreEqual(4,root.AllChildren.Count());

			var result = (Row)filter.Execute(root);
			result.ResetAllChildren();
			Assert.AreEqual(2,result.AllChildren.Count());
			Assert.True(result.AllChildren.Any(_=>_.Code=="d"));
		}
		[Test]
		public void CanChangeExtensibleRowsToPrimary()
		{
			filter.ConvertExtToPrimary = true;
			var root = new Row { Code = "a" };
			IZetaRow c = null;
			root.Children.Add(c = new Row { Code = "b", MarkCache = "/0SA/0EXT/" });
			c.Children.Add(new Row { Code = "e" });
			root.ResetAllChildren();
			Assert.AreEqual(2, root.AllChildren.Count());

			var result = (Row)filter.Execute(root);
			result.ResetAllChildren();
			
			Assert.AreEqual("",result.Children.First().MarkCache);
			Assert.AreEqual("b", result.Children.First().Code);
			Assert.AreEqual(1, result.AllChildren.Count());
		}

		[Test]
		public void CanRemoveIndexes()
		{
			
			var root = new Row { Code = "a",Index = 20};
			
			var result = (Row)filter.Execute(root);
			Assert.AreEqual(20, result.Index);
			filter.ResetAutoIndex = true;
			result = (Row)filter.Execute(root);
			Assert.AreEqual(0, result.Index);
		}

		[Test]
		public void CanRemoveTags()
		{

			var root = new Row { Code = "a", Tag="/x:0//y:1/" };
			var result = (Row)filter.Execute(root);
			Assert.AreEqual("/x:0//y:1/", result.Tag);
			filter.ParseExcludes("tag:noform");
			result = (Row)filter.Execute(root);
			Assert.AreEqual("/x:0/ /y:1/", result.Tag);
			filter.ParseExcludes("tag:x");
			result = (Row)filter.Execute(root);
			Assert.AreEqual("/y:1/", result.Tag);
			filter.ParseExcludes("tag:x tag:y");
			result = (Row)filter.Execute(root);
			Assert.AreEqual("", result.Tag);
		}

		[Test]
		public void CanRemoveGroups()
		{

			var root = new Row { Code = "a", GroupCache = "/X/Y/" };
			var result = (Row)filter.Execute(root);
			Assert.AreEqual("/X/Y/", result.GroupCache);
			filter.ParseExcludes("grp:X");
			result = (Row)filter.Execute(root);
			Assert.AreEqual("/Y/", result.GroupCache);
			filter.ParseExcludes("grp:X grp:Y");
			result = (Row)filter.Execute(root);
			Assert.AreEqual("", result.GroupCache);
		}

		[Test]
		public void CanRemoveMarks()
		{

			var root = new Row { Code = "a", MarkCache = "/X/Y/" };
			var result = (Row)filter.Execute(root);
			Assert.AreEqual("/X/Y/", result.MarkCache);
			filter.ParseExcludes("mark:X");
			result = (Row)filter.Execute(root);
			Assert.AreEqual("/Y/", result.MarkCache);
			filter.ParseExcludes("mark:X mark:Y");
			result = (Row)filter.Execute(root);
			Assert.AreEqual("", result.MarkCache);
		}
	}
}

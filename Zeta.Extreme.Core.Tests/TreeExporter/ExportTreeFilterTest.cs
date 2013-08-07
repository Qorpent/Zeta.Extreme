using System.Linq;
using NUnit.Framework;
using Qorpent;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.MetaStorage;

namespace Zeta.Extreme.Core.Tests.TreeExporter
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
	
	}
}

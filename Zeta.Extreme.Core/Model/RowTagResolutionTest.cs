using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Qorpent.Model;
using Zeta.Extreme.Model.Extensions;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model
{
	/// <summary>
	/// Tests of row tag resolution
	/// </summary>
	[TestFixture]
	public class RowTagResolutionTest
	{
		private IZetaRow _root;
		private IZetaRow _child;
		private IZetaRow _child2;

		/// <summary>
		/// </summary>
		[SetUp]
		public void Setup() {
			_root = new Row {Code = "r", Tag = "/x:1/"};
			_child = new Row {Code = "c1", ParentCode = "r", Tag = "/y:2/"};
			_child2 = new Row { Code = "c2", ParentCode = "c1", Tag = "/z:3/" };
			_root = new[] {_root, _child, _child2}.BuildHierarchy().First();
		}

		/// <summary>
		/// </summary>
		[Test]
		public void CanResolveTagAtSelfLevel() {
			Assert.AreEqual("1", _child2.ResolveTag("x"));
			Assert.AreEqual("2", _child.ResolveTag("y"));
			Assert.AreEqual("3",_child2.ResolveTag("z"));
		}

		/// <summary>
		/// </summary>
		[Test]
		public void CanResolveTagUp()
		{
			Assert.AreEqual("1", _root.ResolveTag("x"));
			Assert.AreEqual("1", _child.ResolveTag("x"));
			Assert.AreEqual("1", _child2.ResolveTag("x"));
			Assert.AreEqual("2", _child.ResolveTag("y"));
			Assert.AreEqual("2", _child2.ResolveTag("y"));
		}

		/// <summary>
		/// </summary>
		[Test]
		public void CanResolveTagWithTemporal()
		{
			Assert.AreEqual("1", _child2.ResolveTag("x"));
			Assert.AreEqual("1", _child.ResolveTag("x"));
			Assert.AreEqual("1", _child2.ResolveTag("x"));
			Assert.AreEqual("2", _child.ResolveTag("y"));
			Assert.AreEqual("2", _child2.ResolveTag("y"));
			_child.TemporalParent = new Row{Tag = "/x:4/"};
			Assert.AreEqual("1", _root.ResolveTag("x"));
			Assert.AreEqual("4", _child.ResolveTag("x"));
			Assert.AreEqual("4", _child2.ResolveTag("x"));
			Assert.AreEqual("2", _child.ResolveTag("y"));
			Assert.AreEqual("2", _child2.ResolveTag("y"));
		}	
	}
}

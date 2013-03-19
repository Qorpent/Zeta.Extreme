using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Zeta.Extreme.Model
{
	/// <summary>
	/// Tests of row tag resolution
	/// </summary>
	[TestFixture]
	public class RowTagResolutionTest
	{
		private Row _root;

		/// <summary>
		/// </summary>
		[SetUp]
		public void Setup() {
			_root = new Row {Code = "r", Tag = "/x:1/"};

		}

		/// <summary>
		/// </summary>
		[Test]
		public void CanResolveTagAtSelfLevel() {
			
		}
	}
}

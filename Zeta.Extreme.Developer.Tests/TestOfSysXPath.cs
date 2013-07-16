using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using NUnit.Framework;

namespace Zeta.Extreme.Developer.Tests
{
	[TestFixture]
	[Explicit]
	public class TestOfSysXPath
	{
		[Test]
		public void CanRetrieveAttributes() {
			var xml = XElement.Parse("<a x='1' y='2'/>");
			var result = (xml.XPathEvaluate("//@*") as IEnumerable).Cast<XAttribute>();
			Assert.AreEqual(2,result.Count());
		}
	}
}

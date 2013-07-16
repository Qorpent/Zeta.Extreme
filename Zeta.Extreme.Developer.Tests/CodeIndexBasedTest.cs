using System.Xml.Linq;
using NUnit.Framework;
using Qorpent.Bxl;
using Zeta.Extreme.Developer.Analyzers;
using Zeta.Extreme.Developer.Model;

namespace Zeta.Extreme.Developer.Tests {
	public abstract class CodeIndexBasedTest {
		protected CodeIndex index;
		protected XElement simplexml;
		protected Source simplesource;
		protected string simplebx =@"
a x=1
	b y=2 z=3
	b y=2 a=1 n=23
a x=2
	b y=3 a=1
	b y=2 z=4
c x=3
	b y=4
";

		[SetUp]
		public virtual void Setup() {
			index = new CodeIndex();
			simplexml = new BxlParser().Parse(simplebx,"simple");
			simplesource = new Source {FileName = "simple", SourceContent = simplebx, XmlContent = simplexml};
			index.AddSource(simplesource);
		}
	}
}
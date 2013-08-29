using System;
using System.Xml.Linq;
using NUnit.Framework;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Developer.MetaStorage;
using Zeta.Extreme.Developer.MetaStorage.Tree;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Developer.Tests.TreeExporter
{
	/// <summary>
	/// Тестируем выгон в BSharp для деревьев
	/// </summary>
	[TestFixture]
	public class BSharpExporterTest
	{
		XElement compile(IZetaRow row=null,string ns=null, string cls=null) {
			var root = row ?? getForm();
			ns = ns ?? "demo.ns";
			cls = cls ?? "balans";
			var result = new BSharpXmlExporter().Export(root,ns , cls);
			Console.WriteLine(result);
			var bsharp = new BSharpTreeExporter().ConvertXmlToBSharp(root,result,new TreeExporterOptions {Namespace = ns,ClassName=cls});
			Console.WriteLine(bsharp);
			return result;
		}
		Row getForm() {
			return new Row {
				Id=1718, Code = "m111", Name = "Баланс", MarkCache = "/0CAPTION/", Version=new DateTime(2013,5,12,13,44,34),
				Children = {
					new Row{Code="m111110",Name="Актив", MarkCache = "0SA"},
					new Row{Code="m111120",Name="Формула",IsFormula = true, Formula="$m113456? * $a214567@Pd?" },
				}
			};
		}
		[Test]
		public void HeaderTest() {
			var xml = compile();
			Assert.AreEqual("namespace",xml.Name.LocalName);
			Assert.AreEqual("demo.ns",xml.Attr("code"));
			var cls = xml.Element("class");
			Assert.NotNull(cls);
			Assert.AreEqual("balans",cls.Attr("code"));
			Assert.AreEqual("m111",cls.Attr("formcode"));
			var imp = cls.Element("import");
			Assert.AreEqual("tree",imp.Attr("code"));
		}

		[Test]
		public void StructureTest()
		{
			var xml = compile();
			var cls = xml.Element("class");
			var title = cls.Element("title");
			Assert.NotNull(title);
		}
	}
}

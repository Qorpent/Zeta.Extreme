using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Qorpent.Dsl;

namespace Zeta.Extreme.FrontEnd.Tests
{
	[TestFixture]
	public class JsonSaveParseTest
	{
		[Test]
		public void BasicParse() {
			var str = @"{""0"":{""id"":""6:3"",""value"":""32345""},""1"":{""id"":""7:3"",""value"":""23626""}}";
			Console.WriteLine(new JsonToXmlParser().Parse(str));
		}
	}
}

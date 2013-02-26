using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Qorpent.Dsl;
using Zeta.Extreme.Core.Tests.CoreTests;
using Zeta.Extreme.FrontEnd.Helpers;

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

	[TestFixture]
	public class AccessiblleObjectsTest:SessionTestBase {
		[Test]
		public void Bug_Innormal_List() {
			var objects = new AccessibleObjectsHelper().GetAccessibleObjects(new GenericPrincipal(new GenericIdentity("ugmk\\intro.hca12"),new string[]{}));
			Assert.AreEqual(1,objects.divs.Length);
			Assert.AreEqual(5, objects.objs.Length);
		}
	}
}

using System.Security.Principal;
using NUnit.Framework;
using Zeta.Extreme.Core.Tests.CoreTests;
using Zeta.Extreme.FrontEnd.Helpers;

namespace Zeta.Extreme.FrontEnd.Tests {
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
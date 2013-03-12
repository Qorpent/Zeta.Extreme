using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Zeta.Extreme.Core.Tests.CoreTests;
using Zeta.Extreme.Model.SqlSupport;

namespace Zeta.Extreme.Core.Tests.NativeMaping
{
	[TestFixture]
	public class CanReadFormState:SessionTestBase
	{
		[Test]
		public void CanReadStateHistory() {
			var states =
				new NativeZetaReader().ReadFormStates(
					"Year = 2012 and Period = 3 and LockCode='sm111' and Object = 352 order by Version").ToArray();
			Assert.AreEqual(2,states.Length);
			Assert.AreEqual("0ISBLOCK",states[0].State);
			Assert.AreEqual("0ISCHECKED", states[1].State);
		}
	}
}

using NUnit.Framework;

namespace Zeta.Extreme.Core.Tests.CoreTests {
	public class SessionTestBase {
		protected ZexSession session;

		[SetUp]
		public void setup() {
			this.session = new ZexSession();
		}
	}
}
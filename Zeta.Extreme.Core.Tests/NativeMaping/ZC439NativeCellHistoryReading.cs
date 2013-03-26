using System.Linq;
using NUnit.Framework;
using Zeta.Extreme.Core.Tests.CoreTests;
using Zeta.Extreme.Model.SqlSupport;

namespace Zeta.Extreme.Core.Tests.NativeMaping {
	[TestFixture]
	public class ZC439NativeCellHistoryReading:SessionTestBase {
		[Test]
		public void CanReadHistoryOfCell() {
			var reader = new NativeZetaReader();
			var history = reader.GetCellHistory(6204487).ToArray();
			Assert.Greater(history.Length,2);
			Assert.True(history.All(_ => _.CellId == 6204487));
			Assert.True(history.Any(_ => _.Value == "0,464"));
			
		}
	}
}
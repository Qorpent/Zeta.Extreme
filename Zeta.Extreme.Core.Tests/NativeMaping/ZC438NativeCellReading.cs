using System.Linq;
using NUnit.Framework;
using Zeta.Extreme.Core.Tests.CoreTests;
using Zeta.Extreme.Model.SqlSupport;

namespace Zeta.Extreme.Core.Tests.NativeMaping {
	[TestFixture]
	public class ZC438NativeCellReading:SessionTestBase
	{
		[Test]
		public void CanReadExistedCell() {
			var reader = new NativeZetaReader();
			var cells = reader.GetCells("Id = " + 6204357).ToArray();
			Assert.AreEqual(1,cells.Length);
			var cell = cells[0];
			Assert.AreEqual(6204357,cell.Id);
			Assert.AreEqual("2013-03-25", cell.Version.ToString("yyyy-MM-dd"));
			Assert.AreEqual(2013,cell.Year);
			Assert.AreEqual(11, cell.Period);
			Assert.AreEqual("z2501712", cell.Row.Code);
			Assert.AreEqual("SUMMA", cell.Column.Code);
			Assert.AreEqual(536,cell.Object.Id);
			Assert.AreEqual("RUB", cell.Currency);
			Assert.AreEqual(7703m, cell.NumericValue);
			Assert.AreEqual("7703", cell.StringValue);
			Assert.AreEqual("ugmk\\intro.elzink24",cell.User);
		}
	}
}
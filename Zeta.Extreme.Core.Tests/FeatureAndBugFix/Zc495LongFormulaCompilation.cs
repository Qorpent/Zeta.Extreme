using NUnit.Framework;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.Core.Tests {
	[TestFixture]
	public class Zc495LongFormulaCompilation {
		[Test]
		public void Zc495TryCompileFromScratch() {
			var formulastorage = new FormulaStorage();
			var formulareq = new FormulaRequest
				{
					Language = "boo",
					Key = "x",
					Formula =
						@"f.If ( periodin (251, 301) and year > 2012, { ( $t110400@PLAN? / $t110400@OZHIDPREDGOD.Y-1? - 1.0 ) * 100.0 } , {
f.If ( periodin (401) , { ( $t110400@OZHIDPREDGOD? / $t110400@Á1.Y-1.P4? - 1.0 ) * 100.0 } , {
f.If ( periodin (303,306,309,301,251) , { ( $t110400@PLAN? / $t110400@Á1.Y-1.P4? - 1.0 ) * 100.0 } , {
f.If ( periodin (42,43,44,444) , { ( $t110400@Á1? / $t110400@Á1.P-209? - 1.0 ) * 100.0 } , {
( $t110400@Á1? / $t110400@Á1.Y-1? - 1.0 ) * 100.0 } ) } ) } ) } )"
				};
			formulastorage.Register(formulareq);
			formulastorage.CompileAll(null);
			Assert.NotNull(formulareq.PreparedType);
			Assert.AreNotEqual(typeof(CompileErrorFormulaStub),formulareq.PreparedType);
		}
	}
}
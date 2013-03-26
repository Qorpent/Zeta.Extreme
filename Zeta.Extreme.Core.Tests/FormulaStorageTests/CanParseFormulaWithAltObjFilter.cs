using System;
using NUnit.Framework;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.Core.Tests.FormulaStorageTests
{
	[TestFixture]
	public class CanParseFormulaWithAltObjFilter
	{
		[Test]
		public void FormulaWithAltObjfilterCanBePreprocessed() {
			var formula = @"f.If ( colin(""Б1"", ""PLAN""), { $r510110@Pd.altobjfilter(""1787"")? } )";
			var testformula = "f.If ( colin(\"Б1\", \"PLAN\"), ()=>( Eval( new QueryDelta{ RowCode = \"r510110\", ColCode = \"Pd\", Contragents = \"1787\", }) ) )";
			var request = new FormulaRequest {Key = "test", Language = "boo", Formula = formula};
			new FormulaStorage().Preprocess(request);
			Console.WriteLine(request.PreprocessedFormula);
			Assert.AreEqual(testformula,request.PreprocessedFormula);
		}

		[Test]
		public void CanCompileFormula()
		{
			var formula = @"f.If ( colin(""Б1"", ""PLAN""), { $r510110@Pd.altobjfilter(""1787"")? } )";
			var request = new FormulaRequest { Key = "test", Language = "boo", Formula = formula };
			var storage = new FormulaStorage();
			var key = storage.Register(request);
			storage.CompileAll(null);
			Assert.NotNull(request.PreparedType);
			Assert.AreNotEqual(typeof(CompileErrorFormulaStub),request.PreparedType);

		}
	}
}

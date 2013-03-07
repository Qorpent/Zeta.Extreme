using NUnit.Framework;

namespace Zeta.Extreme.Analyzer.Tests
{
    [TestFixture]
    public class FormulaTests
    {
        public const string DEFAULT_SIMPLE_FORMULA = "$CODE1@COL1.P3? + $CODE2@COL3? - $code1@COl2?";
        [Test]
        public void CanCreateFormula()
        {
            var formula = new Formula(DEFAULT_SIMPLE_FORMULA);
        }
    }
}

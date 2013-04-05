using System;
using System.Collections.Generic;
using NUnit.Framework;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.Core.Tests {
	[TestFixture]
	public class Zc491CanUseTagAndThemaResolutionInFormulas {
		private MetaCache _mc;
		private Session _session;

		[SetUp]
		public void Setup() {
			_mc = new MetaCache();
			IZetaRow r = null;
			IZetaRow r2 =null;
			//FormulaStorage.Default.Clear();
			
			_mc.Set(new Row {Code = "rc1", IsFormula = true, Formula = "100", FormulaType = "boo"});
			_mc.Set(new Row { Code = "rc2", IsFormula = true, Formula = "20", FormulaType = "boo" });
			_mc.Set(r = new Row { Code = "r1byTag", IsFormula = true, Formula = "$__MYTAG? * $__MYTAG?",Tag = "/MYTAG:rc1/",FormulaType = "boo"});
			_mc.Set(r = new Row { Code = "r1SbyTag", IsFormula = true, Formula = "$__MYTAG? + $__MYTAG?", Tag = "/MYTAG:rc1/", FormulaType = "boo" });
			_mc.Set(r = new Row { Code = "r1RbyTag", IsFormula = true, Formula = "$__MYTAG?", Tag = "/MYTAG:rc1/", FormulaType = "boo" });
			_mc.Set(r2 = new Row { Code = "r2byTag", IsFormula = true, Formula = "$__MYTAG? / $__MYTAG?", Parent = r, FormulaType = "boo" });
			_mc.Set(new Row { Code = "r2byTagAndSource", IsFormula = true, Formula = "$__MYTAG? * $__SRC?", Parent = r, FormulaType = "boo" });
			_mc.Set(new Row { Code = "r1STagAndSource", IsFormula = true, Formula = "$__MYTAG? + $__SRC?", Parent = r, FormulaType = "boo" });
			_mc.Set(new Row { Code = "r1RefSource", IsFormula = true, Formula = "$__SRC?", Parent = r, FormulaType = "boo" });
			r.Children.Add(r2);
			_session = new Session {MetaCache = _mc,FormulaStorage = new FormulaStorage(), PropertySource = new DictionarySessionPropertySource(new Dictionary<string, object>{{"SRC","rc2"}})};
		}

		[TestCase("r1byTag",10000)]
		[TestCase("r1SbyTag", 200)]
		[TestCase("r1RbyTag", 100)]
		[TestCase("r2byTag", 1)]
		[TestCase("r2byTagAndSource", 2000)]
		[TestCase("r1STagAndSource", 120)]
		[TestCase("r1RefSource", 20)]
		public void CanEvaluate(string code, int value) {
			var q = new Query {Row = {Code = code}};
			q = (Query) _session.Register(q);
			_session.WaitPreparation();
			var result = _session.AsSerial().Eval(q);
			if (null != result.Error) {
				Console.WriteLine(result.Error);

			}
			Assert.True(result.IsComplete);
			Assert.AreEqual(value,result.NumericResult);
		}
	
	}
}
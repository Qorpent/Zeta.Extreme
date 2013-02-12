#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : ExistedFormulasParsingTest.cs
// Project: Zeta.Extreme.Core.Tests
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Comdiv.Zeta.Data.Minimal;
using Comdiv.Zeta.Model;
using NUnit.Framework;
using Zeta.Extreme.Core.Tests.CoreTests;

namespace Zeta.Extreme.Core.Tests.SubQuery {
	[TestFixture]
	public class ExistedFormulasParsingTest : SessionTestBase {
		private bool CheckFormula(IZetaRow _row, out string result) {
			var p = new DefaultDeltaPreprocessor();
			result = p.Preprocess(_row.Formula, new FormulaRequest {Language = _row.FormulaEvaluator});
			return !(result.Contains("$") || result.Contains("@"));
		}

		[Test]
		public void CheckAllFormulas() {
			IDictionary<string, string> Errors = new Dictionary<string, string>();
			var sumh = new StrongSumProvider();
			var formulas = RowCache.byid.Values.Where(
				_ => _.IsFormula && _.FormulaEvaluator == "boo" && !sumh.IsSum(_)
				);
			foreach (var f in formulas.ToArray()) {
				string result;
				if (!CheckFormula(f, out result)) {
					Errors[f.Code] = f.Formula + "\r\n!=\r\n" + result;
				}
			}
			if (Errors.Count > 0) {
				foreach (var e in Errors) {
					Console.WriteLine(e.Key + " : " + e.Value);
				}
				Assert.Fail("�� ��� �������");
			}
		}
	}
}
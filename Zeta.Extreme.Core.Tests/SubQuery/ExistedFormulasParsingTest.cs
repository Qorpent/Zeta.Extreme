#region LICENSE
// Copyright 2007-2013 Qorpent Team - http://github.com/Qorpent
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// PROJECT ORIGIN: Zeta.Extreme.Core.Tests/ExistedFormulasParsingTest.cs
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Zeta.Extreme.Core.Tests.CoreTests;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.MetaCaches;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.Core.Tests.SubQuery {
	[TestFixture]
	public class ExistedFormulasParsingTest : SessionTestBase {
		private bool CheckFormula(IZetaRow _row, out string result) {
			var p = new DefaultDeltaPreprocessor();
			result = p.Preprocess(_row.Formula, new FormulaRequest {Language = _row.FormulaType});
			return !(result.Contains("$") || result.Contains("@"));
		}

		[Test]
		[Explicit]
		public void CheckAllFormulas() {
			IDictionary<string, string> Errors = new Dictionary<string, string>();
			var sumh = new StrongSumProvider();
			var formulas = RowCache.Byid.Values.Where(
				_ => _.IsFormula && _.FormulaType == "boo" && !sumh.IsSum(_)
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
				Assert.Fail("не все радужно");
			}
		}
	}
}
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
// PROJECT ORIGIN: Zeta.Extreme.Core.Tests/SimpleFormula.cs
#endregion
using System.Collections.Generic;
using NUnit.Framework;

namespace Zeta.Extreme.Core.Tests.DatabaseIgnorancePureZeta {
	[TestFixture]
	public class SimpleFormula : PureZetaTestFixtureBase {
		/// <summary>
		/// 	строит модель и возвращает нужные запросы
		/// </summary>
		/// <returns> </returns>
		protected override IEnumerable<Query> BuildModel() {
			Add(new Query {Row = {Code = "x"}}, 5);
			Add(new Query {Row = {Code = "y"}}, 5);
			Add(new Query {Row = {Code = "y"}, Col = {Code = "u"}}, 6);
			yield return new Query { Row = { Code = "PureZetaTestFixtureBase", Formula = "$x? * $y@u?", FormulaType = "boo" } };
		}

		protected override void Examinate(Query query) {
			Assert.AreEqual(30, query.Result.NumericResult);
		}
	}
}
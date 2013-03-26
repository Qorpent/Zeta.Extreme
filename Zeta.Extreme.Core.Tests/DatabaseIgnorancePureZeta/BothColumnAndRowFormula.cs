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
// PROJECT ORIGIN: Zeta.Extreme.Core.Tests/BothColumnAndRowFormula.cs
#endregion
using System.Collections.Generic;
using NUnit.Framework;

namespace Zeta.Extreme.Core.Tests.DatabaseIgnorancePureZeta {
	[TestFixture]
	public class BothColumnAndRowFormula : PureZetaTestFixtureBase {
		/// <summary>
		/// 	строит модель и возвращает нужные запросы
		/// </summary>
		/// <returns> </returns>
		protected override IEnumerable<Query> BuildModel() {
			Add(new Query {Row = {Code = "x"}, Col = {Code = "a"}}, 1);
			Add(new Query {Row = {Code = "y"}, Col = {Code = "a"}}, 2);
			Add(new Query {Row = {Code = "z"}, Col = {Code = "a"}}, 3);
			Add(new Query {Row = {Code = "x"}, Col = {Code = "b"}}, 4);
			Add(new Query {Row = {Code = "y"}, Col = {Code = "b"}}, 5);
			Add(new Query {Row = {Code = "z"}, Col = {Code = "b"}}, 6);
			yield return new Query
				{
					Row = {Code = "myf", Formula = "$x? * $y?", FormulaType = "boo"},
					Col = {Code = "sum", Formula = "@a? * @b? + $z@b? / $z@a? ", FormulaType = "boo"}
					/*
					 (xa*ya) * (xb*yb) + zb / za == (1*2)*(4*5) + 6 / 3 = 2 * 20 + 2 = 42
					 */

					 /*
					  * инверсный был бы вариант
					  *  (xa * xb + zb / za ) * (ya * yb + zb / za ) = (4 + 2 ) * (10 + 2) = 6 * 12 = 72
					  *  
					  */

					 // ОТСЮДА - ПОРЯДОК ПРИМЕНЕНИЯ КРАЙНЕ ВАЖЕН
				};
		}

		protected override void Examinate(Query query) {
			Assert.AreEqual(42, query.Result.NumericResult);
		}
	}
}
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
// PROJECT ORIGIN: Zeta.Extreme.Core.Tests/RowColAndObjFormula.cs
#endregion
using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Zeta.Extreme.Core.Tests.DatabaseIgnorancePureZeta {
	[TestFixture]
	public class RowColAndObjFormula : PureZetaTestFixtureBase
	{
		/// <summary>
		/// 	строит модель и возвращает нужные запросы
		/// </summary>
		/// <returns> </returns>
		protected override IEnumerable<Query> BuildModel()
		{
			Add(new Query { Row = { Code = "x" }, Col = { Code = "a" }, Obj = {Id=1}}, 1);
			Add(new Query { Row = { Code = "y" }, Col = { Code = "a" }, Obj = { Id = 1 } }, 2);
			Add(new Query { Row = { Code = "z" }, Col = { Code = "a" }, Obj = { Id = 1 } }, 3);
			Add(new Query { Row = { Code = "x" }, Col = { Code = "b" }, Obj = { Id = 1 } }, 4);
			Add(new Query { Row = { Code = "y" }, Col = { Code = "b" }, Obj = { Id = 1 } }, 5);
			Add(new Query { Row = { Code = "z" }, Col = { Code = "b" }, Obj = { Id = 1 } }, 6);
			Add(new Query { Row = { Code = "x" }, Col = { Code = "a" }, Obj = { Id = 2 } }, 7);
			Add(new Query { Row = { Code = "y" }, Col = { Code = "a" }, Obj = { Id = 2} }, 8);
			Add(new Query { Row = { Code = "z" }, Col = { Code = "a" }, Obj = { Id = 2 } }, 9);
			Add(new Query { Row = { Code = "x" }, Col = { Code = "b" }, Obj = { Id = 2 } }, 10);
			Add(new Query { Row = { Code = "y" }, Col = { Code = "b" }, Obj = { Id = 2 } }, 11);
			Add(new Query { Row = { Code = "z" }, Col = { Code = "b" }, Obj = { Id = 2 } }, 12);
			yield return new Query
				{
					Obj = {Code="fobj",Formula = "$_.toobj(1)? / $_.toobj(2)? ",FormulaType = "boo"},
					Row = { Code = "myf", Formula = "$x? * $y?", FormulaType = "boo" },
					Col = { Code = "sum", Formula = "@a? * @b? + $z@b? / $z@a? ", FormulaType = "boo" }
					/*
					 *	для первого объекта
				       (xa*ya) * (xb*yb) + zb / za == (1*2)*(4*5) + 6 / 3 = 2 * 20 + 2 = 42
					 * для второго
					 * (xa*ya) * (xb*yb) + zb / za == (7*8)*(11*10) + 12 / 9 = 56 * 110 + 1.3333 = 6161,33333
					 * итого
					 * 42 / 6161.33333 = 0.00833
				 
				 */


					// ОТСЮДА - ПОРЯДОК ПРИМЕНЕНИЯ КРАЙНЕ ВАЖЕН
				};
		}

		protected override void Examinate(Query query)
		{
			Assert.AreEqual(0.00682m, Math.Round(query.Result.NumericResult,5));
		}
	}
}
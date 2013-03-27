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
// PROJECT ORIGIN: Zeta.Extreme.Core.Tests/CustomCodeSupport.cs
#endregion
using System.Collections.Generic;
using NUnit.Framework;
using Zeta.Extreme.Model;

namespace Zeta.Extreme.Core.Tests.DatabaseIgnorancePureZeta {
	[TestFixture]
	public class CustomCodeSupport : PureZetaTestFixtureBase
	{
		/// <summary>
		/// С точки зрения ядра CustomCode не более чем дополнительная дельта 
		/// </summary>
		/// <returns> </returns>
		protected override IEnumerable<Query> BuildModel() {
			//описываем некую CustomCols
			_session.MetaCache.Set(new Column {Code = "CUSTOM", ForeignCode = "x", Year = 2013, Period = 13});
			//описыаваем реальные данные
			Add(new Query { Row = { Code = "x" } , Col={Code="x"},Time={Year=2013,Period = 13}}, 5);	
					
			yield return new Query { Row = { Code = "x"}, Time = {Year = 2014,Period = 1}, Col = {Formula = "@CUSTOM? * @CUSTOM?", FormulaType = "boo"}};
		}

		protected override void Examinate(Query query)
		{
			Assert.AreEqual(25, query.Result.NumericResult);
		}
	}
}
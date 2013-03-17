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
// PROJECT ORIGIN: Zeta.Extreme.Core.Tests/SimpleHardLink.cs
#endregion
using System.Collections.Generic;
using NUnit.Framework;
using Zeta.Extreme.Model;

namespace Zeta.Extreme.Core.Tests.DatabaseIgnorancePureZeta {
	[TestFixture]
	public class SimpleHardLink : PureZetaTestFixtureBase {
		/// <summary>
		/// 	строит модель и возвращает нужные запросы
		/// </summary>
		/// <returns> </returns>
		protected override IEnumerable<Query> BuildModel() {
			var q1 = new Query {Row = {Code = "x"}};
			Add(q1, 100);
			var q2 = new Query
				{
					Row =
						{
							Native = new Row
								{
									Code = "y",
									RefTo = new Row
										{
											Code = "z",
											RefTo = new Row {Code = "x"}
										}
								}
						}
				};
			yield return q2;
		}

		protected override void Examinate(Query query) {
			Assert.AreEqual(100, query.Result.NumericResult);
			Assert.AreEqual(1, _session.Registry.Count); //тут не должно быть никаких подзапросов
		}
	}
}
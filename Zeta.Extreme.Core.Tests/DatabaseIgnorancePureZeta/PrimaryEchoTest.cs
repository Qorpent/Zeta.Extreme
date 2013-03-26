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
// PROJECT ORIGIN: Zeta.Extreme.Core.Tests/PrimaryEchoTest.cs
#endregion
using System.Collections.Generic;
using NUnit.Framework;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.Core.Tests.DatabaseIgnorancePureZeta {
	/// <summary>
	/// Проверяем саму работу виртуализации
	/// на первичных данных
	/// </summary>
	[TestFixture]
	public class PrimaryEchoTest : PureZetaTestFixtureBase {
		/// <summary>
		/// строит модель и возвращает нужные запросы
		/// </summary>
		/// <returns></returns>
		protected override IEnumerable<Query> BuildModel() {
			IQuery q;
			yield return (Query) (q = new Query());
			Add(q,10);
			yield return (Query) (q = new Query{Row={Code="x"}});
			Add(q,20);
		}

		protected override void Examinate(Query query) {
			if(query.Row.Code=="x") {
				Assert.AreEqual(20, query.Result.NumericResult);
			}else {
				Assert.AreEqual(10, query.Result.NumericResult);
			}
		}

	}
}
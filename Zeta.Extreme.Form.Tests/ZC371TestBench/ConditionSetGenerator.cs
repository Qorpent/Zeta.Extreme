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
// PROJECT ORIGIN: Zeta.Extreme.Form.Tests/ConditionSetGenerator.cs
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Zeta.Extreme.Form.Tests.ZC371TestBench
{
	
	
	/// <summary>
	/// Получая на входе набор кондиций, генерирует все их возможные сочетания
	/// </summary>
	public class ConditionSetGenerator
	{
		private string[] _conditionList;

		public ConditionSetGenerator(IEnumerable<string> conditionList) {
			_conditionList = conditionList.Distinct().ToArray();
		}

		public ConditionSetGenerator(string condition) {
			_conditionList = Regex.Matches(condition, @"\p{Lu}+").OfType<Match>().Select(_ => _.Value).Distinct().ToArray();
		}

		public IEnumerable<IEnumerable<string>> GenerateConditionSets() {
			var iternumber = Math.Pow(2 , (_conditionList.Length) );
			for(var i = 0;i<iternumber;i++) {
				IList<string> subresult =new List<string>();
				for(var ai = 0;ai<_conditionList.Length;ai++) {
					var flag = (int)Math.Pow(2 , ai);
					if(0!=(flag&i)) {
						subresult.Add(_conditionList[ai]);
					}
				}
				yield return subresult;
			}
		}
		

		
	}
}

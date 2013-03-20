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
// PROJECT ORIGIN: Zeta.Extreme.Form.Tests/NewConditionMatcherImplementation.cs
#endregion
using System.Collections.Generic;
using System.Linq;
using Qorpent.Dsl.LogicalExpressions;
using Qorpent.Utils.Extensions;

namespace Zeta.Extreme.Form.Tests.ZC371TestBench {
	public class NewConditionMatcherImplementation : ConditionMatcherBase {
		readonly LogicalExpressionEvaluator _evaluator = new LogicalExpressionEvaluator();
		protected override bool EvaluateByScript(string condition, IEnumerable<string> conds) {
			var normalizedCondition = (" " + condition + " ").Replace("(", " ( ").Replace(")", " ) ")
				//fix not processable formulas
				.Replace(" and ", " & ").Replace(" or ", " | ").Replace(" not ", " ! "); 
			//fix operators
				
			var soruce = LogicTermSource.Create(conds);
			return _evaluator.Eval(normalizedCondition, soruce);
		}

		protected override bool EvaluateByListLikeCondition(string condition, IEnumerable<string> conds) {
			var condsets = condition.SmartSplit(false, true, '|');
			return condsets.Select(condset => conds.ContainsAll(condset.SmartSplit().ToArray())).Any(match => match);
		}

		protected override bool IsConditionListLike(string condition) {
			return condition.Contains(",") || condition.Contains("|");
		}
	}
}
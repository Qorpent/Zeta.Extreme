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
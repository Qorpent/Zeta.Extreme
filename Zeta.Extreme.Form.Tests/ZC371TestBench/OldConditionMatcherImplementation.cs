using System;
using System.Collections.Generic;
using System.Linq;
using Comdiv.Extensibility;
using Comdiv.Extensions;

namespace Zeta.Extreme.Form.Tests.ZC371TestBench
{
	/// <summary>
	/// Класс, полностью воспроизводящий старый алгоритм расчета условий, чтобы сверять исходный алгоритм
	/// </summary>
	public class OldConditionMatcherImplementation : ConditionMatcherBase {
		protected override bool EvaluateByScript(string condition, IEnumerable<string> conds)
		{
			bool result;
			var e = PythonPool.Get();
			var cond = condition;
			try
			{
				cond = cond.replace(@"[\w\d_]+", m =>
				{
					if (m.Value.isIn("or", "and", "not"))
					{
						return m.Value;
					}
					var c = m.Value;
					if (conds.Contains(c))
					{
						return " True ";
					}
					return " False ";
				}).Trim();
				result = e.Execute<bool>(cond);
				return result;
			}
			catch (Exception)
			{
				throw new Exception("Ошибка в " + cond);
			}
			finally
			{
				PythonPool.Release(e);
			}
		}

		protected override bool EvaluateByListLikeCondition(string condition, IEnumerable<string> conds)
		{
			var condsets = condition.split(false, true, '|');
			foreach (var condset in condsets)
			{
				var match = conds.containsAll(condset.split().ToArray());
				if (match)
				{
					return true;
				}
			}
			return false;
		}

		protected override bool IsConditionListLike(string condition)
		{
			return condition.Contains(",") || condition.Contains("|");
		}
	}
}

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
// PROJECT ORIGIN: Zeta.Extreme.Form.Tests/OldConditionMatcherImplementation.cs
#endregion
using System.Collections.Generic;
using System.Linq;
using Qorpent.Utils.Extensions;

namespace Zeta.Extreme.Form.Tests.ZC371TestBench
{
	/// <summary>
	/// Класс, полностью воспроизводящий старый алгоритм расчета условий, чтобы сверять исходный алгоритм
	/// </summary>
	public class OldConditionMatcherImplementation : ConditionMatcherBase {
		protected override bool EvaluateByScript(string condition, IEnumerable<string> conds)
		{
			#if TESWITHPYTHON
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
			#endif
			return false;
		}

		protected override bool EvaluateByListLikeCondition(string condition, IEnumerable<string> conds)
		{
			var condsets = condition.SmartSplit(false, true, '|');
			return condsets.Select(condset => conds.ContainsAll(condset.SmartSplit().ToArray())).Any(match => match);
		}

		protected override bool IsConditionListLike(string condition)
		{
			return condition.Contains(",") || condition.Contains("|");
		}
	}
}

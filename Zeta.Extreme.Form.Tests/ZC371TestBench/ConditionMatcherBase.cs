using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Zeta.Extreme.Form.Tests.ZC371TestBench {
	/// <summary>
	/// Общий описатель для метода мэтчинга
	/// </summary>
	public abstract class ConditionMatcherBase {
		/// <summary>
		/// 	Проверка соответствия строки условиям
		/// </summary>
		/// <param name="condition"> </param>
		/// <returns> </returns>
		/// <exception cref="Exception"></exception>
		public bool Match(string condition, IEnumerable<string> conditions) {
			var sw = Stopwatch.StartNew();
			if (IsConditionListLike(condition))
			{
				return EvaluateByListLikeCondition(condition, conditions);
			}
			sw.Stop();
			EvaluationTime += sw.Elapsed;
			return EvaluateByScript(condition, conditions);
			
		}

		protected abstract bool EvaluateByScript(string condition, IEnumerable<string> conds);
		protected abstract bool EvaluateByListLikeCondition(string condition, IEnumerable<string> conds);
		protected abstract bool IsConditionListLike(string condition);

		/// <summary>
		/// Число обработанных при помощи списков
		/// </summary>
		public int EvaluatedByListCount { get; set; }

		/// <summary>
		/// Число обработанных при помощи формул
		/// </summary>
		public int EvaluatedByScriptCount { get; set; }

		/// <summary>
		/// Время, затраченное на выполнение
		/// </summary>
		public TimeSpan EvaluationTime { get; set; }
	}
}
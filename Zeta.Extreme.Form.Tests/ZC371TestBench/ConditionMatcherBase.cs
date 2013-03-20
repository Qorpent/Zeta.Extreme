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
// PROJECT ORIGIN: Zeta.Extreme.Form.Tests/ConditionMatcherBase.cs
#endregion
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
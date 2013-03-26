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
// PROJECT ORIGIN: Zeta.Extreme.Core/DeltaFormulaBase.cs
#endregion
using System;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme {
	/// <summary>
	/// 	Базовая основа для формул, оснванных на переходах исходного запроса
	/// </summary>
	public abstract class DeltaFormulaBase : FormulaBase {
		/// <summary>
		/// 	Команда вычисления результата - игнорирует деление на ноль, возвращает в этом случае строковый результат
		/// </summary>
		/// <returns> </returns>
		/// <remarks>
		/// 	В принципе кроме вычисления результата формула не должна ничего уметь
		/// </remarks>
		protected override QueryResult InternalEval() {
			if (IsInPlaybackMode) {
				EvaluateExpression();
				return null;
			}
			try {
				playbackCounter = 0;
				var result = EvaluateExpression();
				if (result == null) {
					return new QueryResult();
				}
				if (result is decimal || result is int) {
					return new QueryResult((decimal) result);
				}
				return new QueryResult {IsComplete = true, StringResult = result.ToString()};
			}
			catch (DivideByZeroException) {
				return new QueryResult {IsComplete = true, StringResult = "-"};
			}
			catch (Exception e) {
				return new QueryResult {IsComplete = false, Error = e};
			}
		}

		/// <summary>
		/// 	Основной промежуточный метод , все приводит к числу
		/// </summary>
		/// <param name="delta"> </param>
		protected internal decimal Eval(QueryDelta delta) {
			var query = delta.Apply(Query);
			if (IsInPlaybackMode) {
				Query.FormulaDependency.Add(Session.Register(query));
				return 1;
			}
			var realq = Query.FormulaDependency[playbackCounter];
			playbackCounter++;
			return GetNumericResult(realq);
		}

		private  decimal GetNumericResult(IQuery realq) {
			if (null == realq) {
				return 0m;
			}
			var processable = realq as IQueryWithProcessing;
			if (null == processable) {
				var result = realq.Result;
				if (null != result) {
					return result.NumericResult;
				}
			}
			else {
				var result = processable.GetResult();
				if (null != result) {
					return result.NumericResult;
				}
			}
			return 0m;
		}

		/// <summary>
		/// 	Метод для перекрытия при формировании динамической формулы
		/// </summary>
		/// <returns> </returns>
		protected abstract object EvaluateExpression();

		/// <summary>
		/// 	Счетчик смещений на плейбэках
		/// </summary>
		protected int playbackCounter;
	}
}
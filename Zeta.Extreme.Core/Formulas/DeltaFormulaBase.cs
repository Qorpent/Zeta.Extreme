#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : DeltaFormulaBase.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;

namespace Zeta.Extreme {
	/// <summary>
	/// 	Ѕазова€ основа дл€ формул, оснванных на переходах исходного запроса
	/// </summary>
	public abstract class DeltaFormulaBase : SessionAttachedFormulaBase {
		/// <summary>
		/// 	 оманда вычислени€ результата - игнорирует деление на ноль, возвращает в этом случае строковый результат
		/// </summary>
		/// <returns> </returns>
		/// <remarks>
		/// 	¬ принципе кроме вычислени€ результата формула не должна ничего уметь
		/// </remarks>
		public override QueryResult Eval() {
			try {
				var result = EvaluateNumber();
				return new QueryResult(result);
			}
			catch (DivideByZeroException) {
				return new QueryResult {IsComplete = true, StringResult = "-"};
			}
			catch (Exception e) {
				return new QueryResult {IsComplete = false, Error = e};
			}
		}

		/// <summary>
		/// 	ќсновной промежуточный метод , все приводит к числу
		/// </summary>
		/// <param name="delta"> </param>
		protected decimal EvalDelta(ZexQueryDelta delta) {
			var query = delta.Apply(Query);
			var result = Session.Eval(query);
			if (null == result) {
				return 0;
			}
			return result.NumericResult;
		}

		/// <summary>
		/// 	ћетод дл€ перекрыти€ при формировании динамической формулы
		/// </summary>
		/// <returns> </returns>
		protected abstract decimal EvaluateNumber();
	}
}
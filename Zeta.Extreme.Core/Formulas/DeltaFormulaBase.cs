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
	public abstract class DeltaFormulaBase : FormulaBase {
		/// <summary>
		/// 	 оманда вычислени€ результата - игнорирует деление на ноль, возвращает в этом случае строковый результат
		/// </summary>
		/// <returns> </returns>
		/// <remarks>
		/// 	¬ принципе кроме вычислени€ результата формула не должна ничего уметь
		/// </remarks>
		protected override QueryResult InternalEval() {
			if(IsInPlaybackMode) {
				EvaluateExpression();
				return null;
			}
			try {
				var result = EvaluateExpression();
				if(result==null) return new QueryResult();
				if(result is decimal || result is int) {
					return new QueryResult((decimal)result);
				}else {
					return new QueryResult {IsComplete = true, StringResult = result.ToString()};

				}
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
		protected internal decimal Eval(QueryDelta delta) {
			var query = delta.Apply(Query);
			if(IsInPlaybackMode) {
				Session.Register(query);
				return 1;
			}
			var realq = Session.Register(query);
			if(null==realq) return 0m;
			var result = realq.GetResult();
			if(null!=result) {
				return result.NumericResult;
			}
			return 0m;
		}

		/// <summary>
		/// 	ћетод дл€ перекрыти€ при формировании динамической формулы
		/// </summary>
		/// <returns> </returns>
		protected abstract object EvaluateExpression();
	}
}
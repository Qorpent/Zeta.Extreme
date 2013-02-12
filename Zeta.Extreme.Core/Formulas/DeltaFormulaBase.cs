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
	/// 	Базовая основа для формул, оснванных на переходах исходного запроса
	/// </summary>
	public abstract class DeltaFormulaBase : SessionAttachedFormulaBase {
		/// <summary>
		/// 	Команда вычисления результата - игнорирует деление на ноль, возвращает в этом случае строковый результат
		/// </summary>
		/// <returns> </returns>
		/// <remarks>
		/// 	В принципе кроме вычисления результата формула не должна ничего уметь
		/// </remarks>
		public override QueryResult Eval() {
			try {
				IsInPlayBack = false;
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
		/// Флаг нахождения в режиме проигрывания формулы
		/// </summary>
		protected bool IsInPlayBack;

		/// <summary>
		/// Вызывается в фазе подготовки, имитирует вызов функции, но без вычисления значений
		/// </summary>
		/// <param name="query"> </param>
		public override void Playback(Query query) {
			Mastersession = query.Session;
			Query = query;
			IsInPlayBack = true;
			EvaluateExpression();
			Query = null;
			Mastersession = null;
		}


		/// <summary>
		/// 	Основной промежуточный метод , все приводит к числу
		/// </summary>
		/// <param name="delta"> </param>
		protected internal decimal EvalDelta(QueryDelta delta) {
			var query = delta.Apply(Query);
			if(IsInPlayBack) {
				Mastersession.Register(query);
				return 1; //fail-free value
			}
			var realq = Mastersession.Register(query);
			if(null==realq) return 0m;
			var result = realq.GetResult();
			if(null!=result) {
				return result.NumericResult;
			}
			return 0m;
		}

		/// <summary>
		/// 	Метод для перекрытия при формировании динамической формулы
		/// </summary>
		/// <returns> </returns>
		protected abstract object EvaluateExpression();
	}
}
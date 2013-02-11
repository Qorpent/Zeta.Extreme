#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : IFormula.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme {
	/// <summary>
	/// 	Базовый интерфейс формулы
	/// </summary>
	public interface IFormula {
		/// <summary>
		/// 	Настраивает формулу на конкретный переданный запрос
		/// </summary>
		/// <param name="query"> </param>
		void Init(ZexQuery query);

		/// <summary>
		/// 	Команда вычисления результата
		/// </summary>
		/// <returns> </returns>
		/// <remarks>
		/// 	В принципе кроме вычисления результата формула не должна ничего уметь
		/// </remarks>
		QueryResult Eval();

		/// <summary>
		/// 	Выполняет очистку ресурсов формулы после использования
		/// </summary>
		void CleanUp();
	}
}
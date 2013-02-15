using System;

namespace Zeta.Extreme {
	/// <summary>
	/// Тип вычисления запроса
	/// </summary>
	[Flags]
	public enum QueryEvaluationType {
		/// <summary>
		/// Неизвестный
		/// </summary>
		Unknown,
		/// <summary>
		/// Первичный
		/// </summary>
		Primary,
		/// <summary>
		/// Суммовой
		/// </summary>
		Summa,
		/// <summary>
		/// Формула
		/// </summary>
		Formula,
	}
}
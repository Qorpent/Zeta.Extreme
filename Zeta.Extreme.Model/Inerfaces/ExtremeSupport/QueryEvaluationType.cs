#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : QueryEvaluationType.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;

namespace Zeta.Extreme {
	/// <summary>
	/// 	Тип вычисления запроса
	/// </summary>
	[Flags]
	public enum QueryEvaluationType {
		/// <summary>
		/// 	Неизвестный
		/// </summary>
		Unknown,

		/// <summary>
		/// 	Первичный
		/// </summary>
		Primary,

		/// <summary>
		/// 	Суммовой
		/// </summary>
		Summa,

		/// <summary>
		/// 	Формула
		/// </summary>
		Formula,
	}
}
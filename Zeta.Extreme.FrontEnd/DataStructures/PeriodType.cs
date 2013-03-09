#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : PeriodType.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// 	Тип периода
	/// </summary>
	[Flags]
	public enum PeriodType {
		/// <summary>
		/// 	Неопределенный
		/// </summary>
		None,

		/// <summary>
		/// 	Месяц
		/// </summary>
		Month,

		/// <summary>
		/// 	Основные периоды с начала года
		/// </summary>
		FromYearStartMain,

		/// <summary>
		/// 	Дополнительные периоды с начала года
		/// </summary>
		FromYearStartExt,

		/// <summary>
		/// 	Плановые периоды
		/// </summary>
		Plan,

		/// <summary>
		/// 	Плановые периоды (месячные)
		/// </summary>
		MonthPlan,

		/// <summary>
		/// 	Коррективы
		/// </summary>
		Corrective,

		/// <summary>
		/// 	Ожидаемые периоды
		/// </summary>
		Awaited,

		/// <summary>
		/// 	Период в середине года
		/// </summary>
		InYear,

		/// <summary>
		/// 	Дополнительные периоды
		/// </summary>
		Ext,
	}
}
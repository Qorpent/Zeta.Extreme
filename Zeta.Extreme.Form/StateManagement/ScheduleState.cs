#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : ScheduleState.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;

namespace Zeta.Extreme.Form.StateManagement {
	/// <summary>
	/// 	Статус относительно расписания формы
	/// </summary>
	public class ScheduleState {
		/// <summary>
		/// 	Целевая дата
		/// </summary>
		public DateTime Date { get; set; }

		/// <summary>
		/// 	Тип соответствия расписанию
		/// </summary>
		public ScheduleOvertime Overtime { get; set; }
	}
}
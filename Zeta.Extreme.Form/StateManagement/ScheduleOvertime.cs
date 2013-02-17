#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : ScheduleOvertime.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.Form.StateManagement {
	/// <summary>
	/// 	Уровень несоответствия расписанию
	/// </summary>
	public enum ScheduleOvertime {
		/// <summary>
		/// 	Не просрочено
		/// </summary>
		None, //нет, не просрочено
		/// <summary>
		/// 	Наступил срок подачи
		/// </summary>
		Critical, //наступил срок подачи
		/// <summary>
		/// 	Просрочено
		/// </summary>
		Fail, // просрочено
	}
}
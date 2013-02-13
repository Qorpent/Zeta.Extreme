#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : IScheduleChecker.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.Form.InputTemplates;

namespace Zeta.Extreme.Form.StateManagement {
	/// <summary>
	/// 	Интерфейс контроля расписаний
	/// </summary>
	public interface IScheduleChecker {
		/// <summary>
		/// 	вычисление соответствия расписанию
		/// </summary>
		/// <param name="template"> </param>
		/// <returns> </returns>
		ScheduleState Evaluate(IInputTemplate template);
	}
}
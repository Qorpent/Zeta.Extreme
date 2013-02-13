#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : StateRule.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.Form.StateManagement {
	/// <summary>
	/// 	Правило на статус
	/// </summary>
	public class StateRule {
		/// <summary>
		/// 	Текущая форма
		/// </summary>
		public string Current { get; set; }

		/// <summary>
		/// 	Целевая форма (?)
		/// </summary>
		public string Target { get; set; }

		/// <summary>
		/// 	Тип
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// 	Действие
		/// </summary>
		public string Action { get; set; }

		/// <summary>
		/// 	Текущий статус
		/// </summary>
		public string CurrentState { get; set; }

		/// <summary>
		/// 	Результирующий статус
		/// </summary>
		public string ResultState { get; set; }
	}
}
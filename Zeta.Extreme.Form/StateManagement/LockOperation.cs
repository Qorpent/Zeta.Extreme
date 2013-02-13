﻿#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : LockOperation.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.Form.StateManagement {
	/// <summary>
	/// 	Тип операции блокировки
	/// </summary>
	public enum LockOperation {
		/// <summary>
		/// 	Нет
		/// </summary>
		None,

		/// <summary>
		/// 	Открыть
		/// </summary>
		Open,

		/// <summary>
		/// 	Блокировать
		/// </summary>
		Block,

		/// <summary>
		/// 	Утвердить
		/// </summary>
		Underwrite
	}
}
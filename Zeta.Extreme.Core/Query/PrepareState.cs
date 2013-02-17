#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : PrepareState.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme {
	/// <summary>
	/// 	Статус подготовки запроса
	/// </summary>
	public enum PrepareState {
		/// <summary>
		/// 	Не начат
		/// </summary>
		None,

		/// <summary>
		/// 	Задача начата
		/// </summary>
		TaskStarted,

		/// <summary>
		/// 	Идет процесс подготовки
		/// </summary>
		InPrepare,

		/// <summary>
		/// 	Готово
		/// </summary>
		Prepared
	}
}
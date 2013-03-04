#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : LockStateInfo.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.Form.StateManagement {
	/// <summary>
	/// 	Информация о статусе закрытия формы
	/// </summary>
	public class LockStateInfo {
		/// <summary>
		/// 	Возможность блокировки формы
		/// </summary>
		public bool canblock;

		/// <summary>
		/// 	Признак возможности сохранения
		/// </summary>
		public bool cansave;

		/// <summary>
		/// 	Зарезервированное понятия открытой формы
		/// </summary>
		public bool isopen;

		/// <summary>
		/// 	Сообщение об ошибке сохранения
		/// </summary>
		public string message;

		/// <summary>
		/// 	Текущий статус
		/// </summary>
		public string state;
	}
}
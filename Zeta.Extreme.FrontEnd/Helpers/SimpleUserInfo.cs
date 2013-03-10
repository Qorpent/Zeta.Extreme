#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : SimpleUserInfo.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.FrontEnd.Helpers {
	/// <summary>
	/// 	Упрощенный описатель пользователя
	/// </summary>
	public class SimpleUserInfo {
		/// <summary>
		/// 	Логин
		/// </summary>
		public string Login { get; set; }

		/// <summary>
		/// 	Имя
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 	Должность
		/// </summary>
		public string Dolzh { get; set; }

		/// <summary>
		/// 	Контактные данные
		/// </summary>
		public string Contact { get; set; }

		/// <summary>
		/// 	Электронная почта
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// 	Идентификатор предприятия
		/// </summary>
		public int ObjId { get; set; }

		/// <summary>
		/// 	Имя предприятия
		/// </summary>
		public string ObjName { get; set; }

		/// <summary>
		/// 	Признак администратора предприятия
		/// </summary>
		public bool IsObjAdmin { get; set; }

		/// <summary>
		/// 	Признак активности пользователя
		/// </summary>
		public bool Active { get; set; }
	}
}
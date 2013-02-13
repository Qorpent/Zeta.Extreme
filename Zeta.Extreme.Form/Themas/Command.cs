#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : Command.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.Form.Themas {
	/// <summary>
	/// 	Команда темы
	/// </summary>
	public class Command : ICommand {
		/// <summary>
		/// 	Код
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		/// 	Название
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 	Роль доступа
		/// </summary>
		public string Role { get; set; }

		/// <summary>
		/// 	Выполняемый URL
		/// </summary>
		public string Url { get; set; }
	}
}
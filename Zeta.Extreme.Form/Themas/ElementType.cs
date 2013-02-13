#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : ElementType.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;

namespace Zeta.Extreme.Form.Themas {
	/// <summary>
	/// 	Тип элемента темы
	/// </summary>
	[Flags]
	public enum ElementType {
		/// <summary>
		/// 	Неопределенный
		/// </summary>
		None = 0,

		/// <summary>
		/// 	Формы
		/// </summary>
		Form = 1,

		/// <summary>
		/// 	Отчеты
		/// </summary>
		Report = 2,

		/// <summary>
		/// 	Документы
		/// </summary>
		Document = 4,

		/// <summary>
		/// 	Команды
		/// </summary>
		Command = 8,

		/// <summary>
		/// 	Пользовательские нестандартные элементы
		/// </summary>
		Custom = 16,

		/// <summary>
		/// 	Шоткат для описания всех элементов
		/// </summary>
		All = Form | Report | Document | Command | Custom,
	}
}
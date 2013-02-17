#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : AttachedFileType.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;

namespace Zeta.Extreme.Form.InputTemplates {
	/// <summary>
	/// 	Тип присоединенного файла
	/// </summary>
	[Flags]
	public enum AttachedFileType {
		/// <summary>
		/// 	Никакой
		/// </summary>
		None = 0,

		/// <summary>
		/// 	По умолчанию
		/// </summary>
		Default = 1,

		/// <summary>
		/// 	Дополнительный
		/// </summary>
		Advanced = 2,

		/// <summary>
		/// 	Ссылочный
		/// </summary>
		Correlated = 4,

		/// <summary>
		/// 	Все
		/// </summary>
		All = 8,

		/// <summary>
		/// 	Связанный
		/// </summary>
		Related = 16,
	}
}
using System;

namespace Comdiv.Zeta.Web.InputTemplates {
	/// <summary>
	/// Тип присоединенного файла
	/// </summary>
	[Flags]
	public enum AttachedFileType {
		/// <summary>
		/// Никакой
		/// </summary>
		None = 0,
		/// <summary>
		/// По умолчанию
		/// </summary>
		Default = 1,
		/// <summary>
		/// Дополнительный
		/// </summary>
		Advanced = 2,
		/// <summary>
		/// Ссылочный
		/// </summary>
		Correlated = 4,
		/// <summary>
		/// Все
		/// </summary>
		All = 8,
		/// <summary>
		/// Связанный
		/// </summary>
		Related = 16,
	}
}
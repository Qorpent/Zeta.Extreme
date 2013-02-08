using System;

namespace Zeta.Extreme {
	/// <summary>
	/// Перечисление типов зон
	/// </summary>
	[Flags]
	public enum ZoneType {
		/// <summary>
		/// Не определено
		/// </summary>
		None = 0,
		/// <summary>
		/// Старший объект
		/// </summary>
		Obj = 1,
		/// <summary>
		/// Младший объект
		/// </summary>
		Detail = 2,
		/// <summary>
		/// Синоним Detail
		/// </summary>
		Det = Detail,
		/// <summary>
		/// Синоним Detail
		/// </summary>
		Sp = Detail,
		/// <summary>
		/// Группа объектов
		/// </summary>
		Grp = 4,
		/// <summary>
		/// Синони Grp
		/// </summary>
		Og = Grp,
		/// <summary>
		/// Дивизион
		/// </summary>
		Div = 8,
		/// <summary>
		/// Синоним DIV
		/// </summary>
		H = Div,
		/// <summary>
		/// Неизвестный тип
		/// </summary>
		Unknown = 128
	}
}
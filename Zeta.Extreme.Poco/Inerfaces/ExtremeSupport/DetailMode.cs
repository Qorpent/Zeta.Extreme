using System;

namespace Comdiv.Zeta.Model.ExtremeSupport {
	/// <summary>
	/// Тип работы с деталями при входном объекте типа "объект" или агрегатный объект,
	/// при работе с деталями напрямую - не имеет сущностного смысла
	/// </summary>
	[Flags]
	public enum DetailMode {
		/// <summary>
		/// Неопределенный - система сама выбирает способ
		/// </summary>
		None, 
		/// <summary>
		/// Максимальная защита объекта, технически - четкая проверка, что DETAIL IS NULL при запросах на объект
		/// </summary>
		SafeObject,
		/// <summary>
		/// Объект - как сумма, используется оператор SUM(VALUE), не может использоваться как первичная ячейка для ввода
		/// </summary>
		SumObject,
		/// <summary>
		/// То же самое что и SumObject, однако производится жесткий контроль, что DETAIL IS NOT NULL
		/// </summary>
		SafeSumObject,
		/// <summary>
		/// Означает что дополнительно используется четкий селектор DETAILTYPE (получаемый из других настроек объекта)
		/// </summary>
		TypedSumObject,
	}
}
using System;
using System.Collections.Concurrent;

namespace Zeta.Extreme {
	/// <summary>
	/// Запрос на компиляцию формулы
	/// </summary>
	public class FormulaRequest {
		/// <summary>
		/// Уникальный ключ
		/// </summary>
		public string Key;
		/// <summary>
		/// Текст формулы
		/// </summary>
		public string Formula;
		/// <summary>
		/// Тип формулы
		/// </summary>
		public string FormulaType;
		/// <summary>
		/// Теги
		/// </summary>
		public string Tags;
		/// <summary>
		/// Метки
		/// </summary>
		public string Marks;
		/// <summary>
		/// Опциональный базовый класс
		/// </summary>
		public Type AssertedBaseType;

		/// <summary>
		/// Прямая регистрация типа в коллекции хранилища
		/// </summary>
		public Type PreparedType;

		/// <summary>
		/// Кэш формул - может использовтаься ххранилищем для организации пула
		/// </summary>
		public readonly ConcurrentStack<IFormula> Cache = new ConcurrentStack<IFormula>();
	}
}
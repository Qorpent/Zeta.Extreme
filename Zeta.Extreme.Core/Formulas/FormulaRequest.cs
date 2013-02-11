#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : FormulaRequest.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Zeta.Extreme {
	/// <summary>
	/// 	Запрос на компиляцию формулы
	/// </summary>
	public class FormulaRequest {
		/// <summary>
		/// 	Кэш формул - может использовтаься ххранилищем для организации пула
		/// </summary>
		public readonly ConcurrentStack<IFormula> Cache = new ConcurrentStack<IFormula>();

		/// <summary>
		/// 	Опциональный базовый класс
		/// </summary>
		public Type AssertedBaseType;

		/// <summary>
		/// 	Текст формулы
		/// </summary>
		public string Formula;

		/// <summary>
		/// 	Текущая выполняемая задача компиляции формулы
		/// </summary>
		public Task FormulaCompilationTask;

		/// <summary>
		/// 	Уникальный ключ
		/// </summary>
		public string Key;

		/// <summary>
		/// 	Тип формулы
		/// </summary>
		public string Language;

		/// <summary>
		/// 	Метки
		/// </summary>
		public string Marks;

		/// <summary>
		/// 	Прямая регистрация типа в коллекции хранилища
		/// </summary>
		public Type PreparedType;

		/// <summary>
		/// 	Формула, преобразованная в валидный C#
		/// </summary>
		public string PreprocessedFormula;

		/// <summary>
		/// 	Теги
		/// </summary>
		public string Tags;
	}
}
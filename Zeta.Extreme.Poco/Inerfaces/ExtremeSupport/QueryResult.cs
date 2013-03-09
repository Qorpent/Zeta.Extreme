#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : QueryResult.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;

namespace Comdiv.Zeta.Model.ExtremeSupport {
	/// <summary>
	/// 	Инкапсуляция результата
	/// </summary>
	public class QueryResult {
		/// <summary>
		/// 	Конструктор по умолчанию
		/// </summary>
		public QueryResult() {}

		/// <summary>
		/// 	Конструктор простого численного результата
		/// </summary>
		/// <param name="result"> </param>
		public QueryResult(decimal result) {
			IsComplete = true;
			NumericResult = result;
		}

		/// <summary>
		/// 	Для форм - идентификатор ячейки
		/// </summary>
		public int CellId;

		/// <summary>
		/// 	Ошибка
		/// </summary>
		public Exception Error;

		/// <summary>
		/// 	Признак завершенности
		/// </summary>
		public bool IsComplete;

		/// <summary>
		/// 	Запрос обработан но отклик не получен
		/// </summary>
		public bool IsNull;

		/// <summary>
		/// 	Численное значение
		/// </summary>
		public decimal NumericResult;

		/// <summary>
		/// 	Строчный результат
		/// </summary>
		public string StringResult;
	}
}
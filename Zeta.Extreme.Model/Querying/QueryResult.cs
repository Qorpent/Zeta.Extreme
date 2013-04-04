#region LICENSE
// Copyright 2007-2013 Qorpent Team - http://github.com/Qorpent
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// PROJECT ORIGIN: Zeta.Extreme.Model/QueryResult.cs
#endregion
using System;

namespace Zeta.Extreme.Model.Querying {
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
		/// 	Строчный результат
		/// </summary>
		public string StringResult;

		/// <summary>
		/// 	Численное значение
		/// </summary>
		public decimal NumericResult;
		/// <summary>
		/// Не дает получить численное значение если была ошибка
		/// </summary>
		/// <returns></returns>
		public decimal GetNumericResultSafe() {
			if (null != Error) throw Error;
			return NumericResult;
		}
	}
}
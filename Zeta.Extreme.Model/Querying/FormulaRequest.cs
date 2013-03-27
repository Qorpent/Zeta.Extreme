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
// PROJECT ORIGIN: Zeta.Extreme.Model/FormulaRequest.cs
#endregion
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// 	Запрос на компиляцию формулы
	/// </summary>
	public class FormulaRequest {
		/// <summary>
		/// 	Версия
		/// </summary>
		public string Version { get; set; }

		/// <summary>
		/// 	Кэш формул - может использовтаься ххранилищем для организации пула
		/// </summary>
		public readonly ConcurrentStack<IFormula> Cache = new ConcurrentStack<IFormula>();

		/// <summary>
		/// 	Опциональный базовый класс
		/// </summary>
		public Type AssertedBaseType;

		/// <summary>
		/// 	Ошибка компиляции
		/// </summary>
		public Exception ErrorInCompilation;

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
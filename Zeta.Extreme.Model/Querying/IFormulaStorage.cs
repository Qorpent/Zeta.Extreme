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
// PROJECT ORIGIN: Zeta.Extreme.Model/IFormulaStorage.cs
#endregion
using System;

namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// 	Интерфейс коллекции формул
	/// </summary>
	public interface IFormulaStorage {
		/// <summary>
		/// 	True - включен режим автоматического батча
		/// </summary>
		bool AutoBatchCompile { get; set; }

		/// <summary>
		/// 	Последняя ошибка компиляции
		/// </summary>
		Exception LastCompileError { get; set; }

		/// <summary>
		/// 	Количество формул
		/// </summary>
		int Count { get; }


		///<summary>
		///</summary>
		///<param name="request"> </param>
		///<returns> Возвращает полученный ключ формулы </returns>
		string Register(FormulaRequest request);

		/// <summary>
		/// 	Регистрирует препроцессор в хранилище
		/// </summary>
		/// <param name="preprocessor"> </param>
		/// <returns> </returns>
		void AddPreprocessor(IFormulaPreprocessor preprocessor);

		/// <summary>
		/// 	Возвращает экземпляр формулы по ключу
		/// </summary>
		/// <param name="key"> </param>
		/// <param name="throwErrorOnNotFound"> false если надо возвращать NULL при отсутствии формулы </param>
		/// <returns> </returns>
		IFormula GetFormula(string key, bool throwErrorOnNotFound = true);

		/// <summary>
		/// 	Возвращает формулу обратно хранилищу, может использовать для реализации кэша формул
		/// </summary>
		/// <param name="key"> </param>
		/// <param name="formula"> </param>
		void Return(string key, IFormula formula);

		/// <summary>
		/// 	Асинхронно выполняет полную компиляцию формул
		/// </summary>
		void StartAsyncCompilation();

		/// <summary>
		/// 	Компилирует все формы в стеке
		/// </summary>
		void CompileAll(string savepath = null);

		/// <summary>
		/// 	Очистка кэша формул
		/// </summary>
		void Clear();

		/// <summary>
		/// 	Проверяет наличие формулы в хранилище
		/// </summary>
		/// <param name="key"> </param>
		/// <returns> </returns>
		bool Exists(string key);

		/// <summary>
		/// Загружает формулы по умолчанию из кжша, с использованием указанной папки готовых DLL
		/// </summary>
		/// <param name="rootDirectory"></param>
		void LoadDefaultFormulas(string rootDirectory);

		/// <summary>
		/// Возвращает запрос по коду
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		FormulaRequest GetRequest(string code);
	}
}
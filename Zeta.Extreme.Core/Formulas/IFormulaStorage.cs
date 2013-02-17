#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IFormulaStorage.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;

namespace Zeta.Extreme {
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
		void CompileAll();

		/// <summary>
		/// 	Очистка кэша формул
		/// </summary>
		void Clear();
	}
}
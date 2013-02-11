#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : IFormulaPreprocessor.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme {
	/// <summary>
	/// 	Препроцессор формул - конвертирует исходный текст в компилируемый код на C# с учетом шаблона
	/// </summary>
	public interface IFormulaPreprocessor {
		/// <summary>
		/// 	Индекс препроцессора
		/// </summary>
		int Idx { get; }

		/// <summary>
		/// 	Препроцессит текст формулы указанного реквеста с учетом текущего статуса препроцессора
		/// </summary>
		/// <param name="currentResult"> </param>
		/// <param name="request"> </param>
		/// <returns> </returns>
		string Preprocess(string currentResult, FormulaRequest request);
	}
}
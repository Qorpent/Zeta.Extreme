#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : DefaultDeltaPreprocessor.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Text.RegularExpressions;

namespace Zeta.Extreme {
	/// <summary>
	/// 	Конвертирует стандартные пути в boo, cs
	/// </summary>
	public class DefaultDeltaPreprocessor : IFormulaPreprocessor {
		/// <summary>
		/// 	Индекс препроцессора
		/// </summary>
		public int Idx {
			get { return 10; }
		}


		/// <summary>
		/// 	Препроцессит текст формулы указанного реквеста с учетом текущего статуса препроцессора
		/// </summary>
		/// <param name="currentResult"> </param>
		/// <param name="request"> </param>
		/// <returns> </returns>
		public string Preprocess(string currentResult, FormulaRequest request) {
			if (request.Language == "boo" || request.Language == "cs") {
				//ограничитель на язык
				return Regex.Replace(currentResult, FormulaParserConstants.FormulaVector,
				                     m => ZexQueryDelta.CreateFromMatch(m).ToCSharpString("EvalDelta"));
			}
			else {
				return currentResult;
			}
		}
	}
}
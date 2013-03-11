#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : DefaultDeltaPreprocessor.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Text.RegularExpressions;
using Zeta.Extreme.Model.Inerfaces;

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
				var result = Regex.Replace(currentResult, FormulaParserConstants.CallDeltaPattern,
				                           m => QueryDelta.CreateFromMatch(m).ToCSharpString(true, "Eval"));
				result = Regex.Replace(result, FormulaParserConstants.DeltaPattern,
				                       m => QueryDelta.CreateFromMatch(m).ToCSharpString());
				// совместимость с формулами типа f.choose
				result = Regex.Replace(result, @"(\d+\.\d+)", "$1m", RegexOptions.Compiled);
				result = Regex.Replace(result, @"""___(\w+)""", "getn(\"$1\")", RegexOptions.Compiled);
				result = Regex.Replace(result, @"___(\w+)", "getn(\"$1\")", RegexOptions.Compiled);
				result = Regex.Replace(result, @"""__(\w+)""", "gets(\"$1\")", RegexOptions.Compiled);
				result = Regex.Replace(result, @"__(\w+)", "gets(\"$1\")", RegexOptions.Compiled);
				if (result.Contains("@") || result.Contains("$") || result.Contains("?")) {
					request.PreparedType = typeof (IllegalSyntaxFormulaStub);
				}
				return result;
			}
			return currentResult;
		}
	}
}
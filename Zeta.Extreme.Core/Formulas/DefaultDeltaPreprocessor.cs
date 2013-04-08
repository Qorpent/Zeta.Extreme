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
// PROJECT ORIGIN: Zeta.Extreme.Core/DefaultDeltaPreprocessor.cs
#endregion
using System.Text.RegularExpressions;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.Querying;

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
				if (result.Contains("f.If")) {
					result = Regex.Replace(result, @"f\.If\s*\(", "f.If(()=>", RegexOptions.Compiled);
				}
				return result;
			}
			return currentResult;
		}
	}
}
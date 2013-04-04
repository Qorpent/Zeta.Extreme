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
// PROJECT ORIGIN: Zeta.Extreme.Core/BooConverter.cs
#endregion
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme {
	/// <summary>
	/// 	Конвертирует фигурные скобки с ламбдами BOO в С# ламбды
	/// </summary>
	public class BooConverter : IFormulaPreprocessor {
		/// <summary>
		/// 	Индекс препроцессора
		/// </summary>
		public int Idx {
			get { return 5; }
		}

		/// <summary>
		/// 	Препроцессит текст формулы указанного реквеста с учетом текущего статуса препроцессора
		/// </summary>
		/// <param name="currentResult"> </param>
		/// <param name="request"> </param>
		/// <returns> </returns>
		public string Preprocess(string currentResult, FormulaRequest request) {
			if (request.Language == "boo") {
				//ограничитель на язык - только BOO
				var result =
					currentResult
						.Replace("{", "()=>(")
						.Replace("}", ")")
						.Replace(" and ", " && ")
						.Replace(" or ", " || ")
						.Replace(" not ", " ! ")
						.Replace("'", "\"")
						.Replace(".Column.Period", ".Time.Period")
						.Replace("q.Column", "q.Col")
						.Replace("query.Column", "q.Col")
						.Replace("MCOUNT", " f.monthCount(q) ")
						.Replace(".altdiv(\"", ".altobjfilter(\"div_")
						.Replace(".torootobj()", ".toobj(-1)")
					;

				return result;
			}
			return currentResult;
		}
	}
}
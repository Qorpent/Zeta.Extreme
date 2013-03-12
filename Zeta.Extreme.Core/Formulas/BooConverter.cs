#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : BooConverter.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

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
					;

				return result;
			}
			return currentResult;
		}
	}
}
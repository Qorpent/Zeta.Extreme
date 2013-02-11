namespace Zeta.Extreme {
	/// <summary>
	/// Конвертирует фигурные скобки с ламбдами BOO в С# ламбды
	/// </summary>
	public class BooConverter : IFormulaPreprocessor {
		/// <summary>
		/// 	Индекс препроцессора
		/// </summary>
		public int Idx
		{
			get { return 10; }
		}

		/// <summary>
		/// 	Препроцессит текст формулы указанного реквеста с учетом текущего статуса препроцессора
		/// </summary>
		/// <param name="currentResult"> </param>
		/// <param name="request"> </param>
		/// <returns> </returns>
		public string Preprocess(string currentResult, FormulaRequest request) {
			if (request.Language == "boo" )
			{
				//ограничитель на язык - только BOO
				return 
					currentResult
						.Replace("{", "()=>(")
						.Replace("}", ")")
						.Replace(" and "," && ")
						.Replace(" or "," || ")
						.Replace(" not "," ! ")
						.Replace("'","\"")
						;

			}
			else
			{
				return currentResult;
			}
		}
	}
}
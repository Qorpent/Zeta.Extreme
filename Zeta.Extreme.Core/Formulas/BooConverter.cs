namespace Zeta.Extreme {
	/// <summary>
	/// ������������ �������� ������ � �������� BOO � �# ������
	/// </summary>
	public class BooConverter : IFormulaPreprocessor {
		/// <summary>
		/// 	������ �������������
		/// </summary>
		public int Idx
		{
			get { return 10; }
		}

		/// <summary>
		/// 	������������ ����� ������� ���������� �������� � ������ �������� ������� �������������
		/// </summary>
		/// <param name="currentResult"> </param>
		/// <param name="request"> </param>
		/// <returns> </returns>
		public string Preprocess(string currentResult, FormulaRequest request) {
			if (request.Language == "boo" )
			{
				//������������ �� ���� - ������ BOO
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
using System.Text.RegularExpressions;
using Comdiv.Extensions;

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
			get { return 5; }
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
				var result = 
					currentResult
						.Replace("{", "()=>(")
						.Replace("}", ")")
						.Replace(" and "," && ")
						.Replace(" or "," || ")
						.Replace(" not "," ! ")
						.Replace("'","\"")
						.Replace(".Column.Period",".Time.Period")
						.Replace("q.Column", "q.Col")
						.Replace("query.Column","q.Col")
						;
				
				return result;
			}
			else
			{
				return currentResult;
			}
		}
	}
}
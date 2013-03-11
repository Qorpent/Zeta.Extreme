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
	/// 	������������ �������� ������ � �������� BOO � �# ������
	/// </summary>
	public class BooConverter : IFormulaPreprocessor {
		/// <summary>
		/// 	������ �������������
		/// </summary>
		public int Idx {
			get { return 5; }
		}

		/// <summary>
		/// 	������������ ����� ������� ���������� �������� � ������ �������� ������� �������������
		/// </summary>
		/// <param name="currentResult"> </param>
		/// <param name="request"> </param>
		/// <returns> </returns>
		public string Preprocess(string currentResult, FormulaRequest request) {
			if (request.Language == "boo") {
				//������������ �� ���� - ������ BOO
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
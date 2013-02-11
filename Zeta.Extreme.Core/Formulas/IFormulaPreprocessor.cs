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
	/// 	������������ ������ - ������������ �������� ����� � ������������� ��� �� C# � ������ �������
	/// </summary>
	public interface IFormulaPreprocessor {
		/// <summary>
		/// 	������ �������������
		/// </summary>
		int Idx { get; }

		/// <summary>
		/// 	������������ ����� ������� ���������� �������� � ������ �������� ������� �������������
		/// </summary>
		/// <param name="currentResult"> </param>
		/// <param name="request"> </param>
		/// <returns> </returns>
		string Preprocess(string currentResult, FormulaRequest request);
	}
}
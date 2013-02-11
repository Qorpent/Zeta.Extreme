#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : IFormulaStorage.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme {
	/// <summary>
	/// 	��������� ��������� ������
	/// </summary>
	public interface IFormulaStorage {
		///<summary>
		///</summary>
		///<param name="request"> </param>
		///<returns> ���������� ���������� ���� ������� </returns>
		string Register(FormulaRequest request);

		/// <summary>
		/// 	������������ ������������ � ���������
		/// </summary>
		/// <param name="preprocessor"> </param>
		/// <returns> </returns>
		void AddPreprocessor(IFormulaPreprocessor preprocessor);

		/// <summary>
		/// 	���������� ��������� ������� �� �����
		/// </summary>
		/// <param name="key"> </param>
		/// <param name="throwErrorOnNotFound"> false ���� ���� ���������� NULL ��� ���������� ������� </param>
		/// <returns> </returns>
		IFormula GetFormula(string key, bool throwErrorOnNotFound = true);

		/// <summary>
		/// 	���������� ������� ������� ���������, ����� ������������ ��� ���������� ���� ������
		/// </summary>
		/// <param name="key"> </param>
		/// <param name="formula"> </param>
		void Return(string key, IFormula formula);
	}
}
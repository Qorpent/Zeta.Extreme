#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IFormulaStorage.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;

namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// 	��������� ��������� ������
	/// </summary>
	public interface IFormulaStorage {
		/// <summary>
		/// 	True - ������� ����� ��������������� �����
		/// </summary>
		bool AutoBatchCompile { get; set; }

		/// <summary>
		/// 	��������� ������ ����������
		/// </summary>
		Exception LastCompileError { get; set; }

		/// <summary>
		/// 	���������� ������
		/// </summary>
		int Count { get; }


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

		/// <summary>
		/// 	���������� ��������� ������ ���������� ������
		/// </summary>
		void StartAsyncCompilation();

		/// <summary>
		/// 	����������� ��� ����� � �����
		/// </summary>
		void CompileAll(string savepath = null);

		/// <summary>
		/// 	������� ���� ������
		/// </summary>
		void Clear();

		/// <summary>
		/// 	��������� ������� ������� � ���������
		/// </summary>
		/// <param name="key"> </param>
		/// <returns> </returns>
		bool Exists(string key);


		/// <summary>
		/// 	������ ��� �� ��������� ����������
		/// </summary>
		/// <param name="root"> </param>
		void BuildCache(string root);

		/// <summary>
		/// ��������� ������� �� ��������� �� ����, � �������������� ��������� ����� ������� DLL
		/// </summary>
		/// <param name="rootDirectory"></param>
		void LoadDefaultFormulas(string rootDirectory);
	}
}
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
// PROJECT ORIGIN: Zeta.Extreme.Model/IFormulaStorage.cs
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
		/// ��������� ������� �� ��������� �� ����, � �������������� ��������� ����� ������� DLL
		/// </summary>
		/// <param name="rootDirectory"></param>
		void LoadDefaultFormulas(string rootDirectory);

		/// <summary>
		/// ���������� ������ �� ����
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		FormulaRequest GetRequest(string code);
	}
}
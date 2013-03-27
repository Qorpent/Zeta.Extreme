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
// PROJECT ORIGIN: Zeta.Extreme.Model/FormulaRequest.cs
#endregion
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// 	������ �� ���������� �������
	/// </summary>
	public class FormulaRequest {
		/// <summary>
		/// 	������
		/// </summary>
		public string Version { get; set; }

		/// <summary>
		/// 	��� ������ - ����� �������������� ����������� ��� ����������� ����
		/// </summary>
		public readonly ConcurrentStack<IFormula> Cache = new ConcurrentStack<IFormula>();

		/// <summary>
		/// 	������������ ������� �����
		/// </summary>
		public Type AssertedBaseType;

		/// <summary>
		/// 	������ ����������
		/// </summary>
		public Exception ErrorInCompilation;

		/// <summary>
		/// 	����� �������
		/// </summary>
		public string Formula;

		/// <summary>
		/// 	������� ����������� ������ ���������� �������
		/// </summary>
		public Task FormulaCompilationTask;

		/// <summary>
		/// 	���������� ����
		/// </summary>
		public string Key;

		/// <summary>
		/// 	��� �������
		/// </summary>
		public string Language;

		/// <summary>
		/// 	�����
		/// </summary>
		public string Marks;

		/// <summary>
		/// 	������ ����������� ���� � ��������� ���������
		/// </summary>
		public Type PreparedType;

		/// <summary>
		/// 	�������, ��������������� � �������� C#
		/// </summary>
		public string PreprocessedFormula;

		/// <summary>
		/// 	����
		/// </summary>
		public string Tags;
	}
}
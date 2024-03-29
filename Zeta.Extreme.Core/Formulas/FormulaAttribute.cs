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
// PROJECT ORIGIN: Zeta.Extreme.Core/FormulaAttribute.cs
#endregion
using System;

namespace Zeta.Extreme {
	/// <summary>
	/// 	������� ��� ������������� ���������������� ������
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class FormulaAttribute : Attribute {
		/// <summary>
		/// </summary>
		/// <param name="key"> </param>
		/// <param name="version"> </param>
		public FormulaAttribute(string key, string version) {
			Key = key;
			Version = version;
		}

		/// <summary>
		/// </summary>
		/// <param name="key"> </param>
		/// <param name="version"> </param>
		/// <param name="srcformula"></param>
		/// <param name="generatedCode"></param>
		public FormulaAttribute(string key, string version, string srcformula,string generatedCode)
		{
			Key = key;
			Version = version;
			SourceFormula = srcformula;
			GeneratedCode = generatedCode;
		}

		/// <summary>
		/// ��������������� ���
		/// </summary>
		public string GeneratedCode { get; set; }

		/// <summary>
		/// �������� �������
		/// </summary>
		public string SourceFormula { get; set; }


		/// <summary>
		/// 	���� ������� ��� ��������� ����������� � ���������
		/// </summary>
		public string Key { get; private set; }

		/// <summary>
		/// 	������ � ������� �������
		/// </summary>
		public string Version { get; set; }

		
	}
}
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
// PROJECT ORIGIN: Zeta.Extreme.Core/PrimaryQueryGroup.cs
#endregion
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.Primary {
	/// <summary>
	/// 	������ ��������� ��������
	/// </summary>
	public class PrimaryQueryGroup {
		/// <summary>
		/// 	������� � ������
		/// </summary>
		public IQuery[] Queries { get; set; }

		/// <summary>
		/// 	�������� ���������� �������
		/// </summary>
		public PrimaryQueryPrototype Prototype { get; set; }

		/// <summary>
		/// 	��������� ��������
		/// </summary>
		public IScriptGenerator ScriptGenerator { get; set; }

		/// <summary>
		/// 	������ SQL ������
		/// </summary>
		/// <returns> </returns>
		public string GenerateSqlScript() {
			return ScriptGenerator.Generate(Queries, Prototype);
		}
	}
}
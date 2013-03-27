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
// PROJECT ORIGIN: Zeta.Extreme.FrontEnd/StructureItem.cs
#endregion
using Qorpent.Serialization;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// 	��������� ������� ���������
	/// </summary>
	[Serialize]
	public class StructureItem {
		/// <summary>
		/// 	��� ������/�������
		/// </summary>
		public string code;

		/// <summary>
		/// 	������� ����������� �����
		/// </summary>
		[SerializeNotNullOnly] public bool controlpoint;

		/// <summary>
		/// 	������ � �������
		/// </summary>
		public int idx;

		/// <summary>
		/// 	������� �����
		/// </summary>
		[SerializeNotNullOnly] public bool iscaption;

		/// <summary>
		/// 	������� �����������
		/// </summary>
		[SerializeNotNullOnly] public bool isprimary;

		/// <summary>
		/// 	�������
		/// </summary>
		[SerializeNotNullOnly] public int level;

		///<summary>
		///	������� ���������
		///</summary>
		[SerializeNotNullOnly] public string measure;

		/// <summary>
		/// 	�������� ������/�������
		/// </summary>
		public string name;

		/// <summary>
		/// 	����� ������
		/// </summary>
		[SerializeNotNullOnly] public string number;

		/// <summary>
		/// 	������ ��� �������
		/// </summary>
		[SerializeNotNullOnly] public int period;

		/// <summary>
		/// 	r or c
		/// </summary>
		public string type;

		/// <summary>
		/// 	��� ��� �������
		/// </summary>
		[SerializeNotNullOnly] public int year;

		/// <summary>
		/// ������� ������������ ���������
		/// </summary>
		[SerializeNotNullOnly]public bool exref;
		/// <summary>
		/// ������ �����
		/// </summary>
		public string format;
	}
}
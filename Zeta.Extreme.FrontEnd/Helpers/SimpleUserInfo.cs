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
// PROJECT ORIGIN: Zeta.Extreme.FrontEnd/SimpleUserInfo.cs
#endregion

using Qorpent.Serialization;

namespace Zeta.Extreme.FrontEnd.Helpers {
	/// <summary>
	/// 	���������� ��������� ������������
	/// </summary>
	public class SimpleUserInfo {
		/// <summary>
		/// 	�����
		/// </summary>
		public string Login { get; set; }

		/// <summary>
		/// 	���
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 	���������
		/// </summary>
		public string Dolzh { get; set; }

		/// <summary>
		/// 	���������� ������
		/// </summary>
		public string Contact { get; set; }

		/// <summary>
		/// 	����������� �����
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// 	������������� �����������
		/// </summary>
		public int ObjId { get; set; }

		/// <summary>
		/// 	��� �����������
		/// </summary>
		public string ObjName { get; set; }

		/// <summary>
		/// 	������� �������������� �����������
		/// </summary>
		public bool IsObjAdmin { get; set; }

		/// <summary>
		/// 	������� ���������� ������������
		/// </summary>
		public bool Active { get; set; }
		/// <summary>
		/// ������ ������
		/// </summary>
		[SerializeNotNullOnly]
		public string[] Slots { get; set; }

		/// <summary>
		/// �������� ����� �������
		/// </summary>
		public string[] ObjRoles { get; set; }
		/// <summary>
		/// �������� ��������� �����
		/// </summary>
		public string[] SysRoles { get; set; }
	}
}
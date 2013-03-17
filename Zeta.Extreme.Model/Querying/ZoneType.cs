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
// PROJECT ORIGIN: Zeta.Extreme.Model/ObjType.cs
#endregion
using System;

namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// 	������������ ����� ���
	/// </summary>
	[Flags]
	public enum ZoneType {
		/// <summary>
		/// 	�� ����������
		/// </summary>
		None = 0,

		/// <summary>
		/// 	������� ������
		/// </summary>
		Obj = 1,

		/// <summary>
		/// 	������� ������
		/// </summary>
		Detail = 2,

		/// <summary>
		/// 	������� Detail
		/// </summary>
		Det = Detail,

		/// <summary>
		/// 	������� Detail
		/// </summary>
		Sp = Detail,

		/// <summary>
		/// 	������ ��������
		/// </summary>
		Grp = 4,

		/// <summary>
		/// 	������ Grp
		/// </summary>
		Og = Grp,

		/// <summary>
		/// 	��������
		/// </summary>
		Div = 8,

		/// <summary>
		/// 	������� DIV
		/// </summary>
		H = Div,

		/// <summary>
		/// 	����������� ���
		/// </summary>
		Unknown = 128
	}
}
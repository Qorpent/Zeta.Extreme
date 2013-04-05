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
// PROJECT ORIGIN: Zeta.Extreme.Model/IObjHandler.cs
#endregion
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// 	����������� ��������� ��������� ������� Obj
	/// </summary>
	public interface IObjHandler : IQueryDimension<IZetaObject> {
		/// <summary>
		/// 	��� ����
		/// </summary>
		ZoneType Type { get; set; }

		/// <summary>
		/// 	����� ������ � �������� �� ������ ��������� ��������, �� ��������� NONE - ����� �������� �� ��������
		/// </summary>
		DetailMode DetailMode { get; set; }

		/// <summary>
		/// 	������ ��� ������� �������� ��� ���� ���� � �����������
		/// </summary>
		bool IsForObj { get; }

		/// <summary>
		/// 	������ ��� ������� �������� ��� ���� ���� �� � �����������
		/// </summary>
		bool IsNotForObj { get; }

		/// <summary>
		/// 	������ �� �������� ��������� �������� �������
		/// </summary>
		IZetaMainObject ObjRef { get; }


		/// <summary>
		/// 	������� ����� ����
		/// </summary>
		/// <returns> </returns>
		IObjHandler Copy();
	}
}
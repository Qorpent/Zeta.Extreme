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
// PROJECT ORIGIN: Zeta.Extreme.BizProcess/TypedParameter.cs
#endregion
using System;
using System.Reflection;
using Qorpent.Utils.Extensions;

namespace Zeta.Extreme.BizProcess.Themas {
	/// <summary>
	/// 	�������������� �������� ����
	/// </summary>
	public class TypedParameter {
		/// <summary>
		/// 	����������� �� ���������
		/// </summary>
		public TypedParameter() {
			Type = typeof (Missing);
		}

		/// <summary>
		/// 	��������� �������� ��������
		/// </summary>
		/// <returns> </returns>
		public object GetValue() {
			if (Value != null && Type == typeof (Missing)) {
				Type = typeof (string);
			}
			return Value.ToTargetType(Type);
		}

		/// <summary>
		/// 	���������� ����� ���������
		/// </summary>
		public int Idx;

		/// <summary>
		/// 	����� ���������
		/// </summary>
		public string Mode; //NOTE: ��� ��� �� �����?

		/// <summary>
		/// 	��� ���������
		/// </summary>
		public string Name;

		/// <summary>
		/// 	��� ���������
		/// </summary>
		public Type Type;

		/// <summary>
		/// 	�������� ���������
		/// </summary>
		public string Value;
	}
}
#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : TypedParameter.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Reflection;
using Comdiv.Extensions;

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
			return Value.to(Type);
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
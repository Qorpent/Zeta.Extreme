#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : ElementType.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;

namespace Zeta.Extreme.Form.Themas {
	/// <summary>
	/// 	��� �������� ����
	/// </summary>
	[Flags]
	public enum ElementType {
		/// <summary>
		/// 	��������������
		/// </summary>
		None = 0,

		/// <summary>
		/// 	�����
		/// </summary>
		Form = 1,

		/// <summary>
		/// 	������
		/// </summary>
		Report = 2,

		/// <summary>
		/// 	���������
		/// </summary>
		Document = 4,

		/// <summary>
		/// 	�������
		/// </summary>
		Command = 8,

		/// <summary>
		/// 	���������������� ������������� ��������
		/// </summary>
		Custom = 16,

		/// <summary>
		/// 	������ ��� �������� ���� ���������
		/// </summary>
		All = Form | Report | Document | Command | Custom,
	}
}
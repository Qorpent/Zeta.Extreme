#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : RowTreeUsage.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;

namespace Zeta.Extreme {
	/// <summary>
	/// 	������������� ������ ��� ������
	/// </summary>
	[Flags]
	public enum RowTreeUsage {
		/// <summary>
		/// 	�� ������������
		/// </summary>
		None = 0,

		/// <summary>
		/// 	��� ���������
		/// </summary>
		AllWithSelf = 1,

		/// <summary>
		/// 	��� ��������
		/// </summary>
		AllDescendants = 2,

		/// <summary>
		/// 	��������� ��������
		/// </summary>
		Primaries = 4,
	}
}
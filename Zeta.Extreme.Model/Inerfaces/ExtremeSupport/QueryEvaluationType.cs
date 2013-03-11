#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : QueryEvaluationType.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;

namespace Zeta.Extreme {
	/// <summary>
	/// 	��� ���������� �������
	/// </summary>
	[Flags]
	public enum QueryEvaluationType {
		/// <summary>
		/// 	�����������
		/// </summary>
		Unknown,

		/// <summary>
		/// 	���������
		/// </summary>
		Primary,

		/// <summary>
		/// 	��������
		/// </summary>
		Summa,

		/// <summary>
		/// 	�������
		/// </summary>
		Formula,
	}
}
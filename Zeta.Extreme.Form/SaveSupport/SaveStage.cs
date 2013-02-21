#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : SaveStage.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;

namespace Zeta.Extreme.Form.SaveSupport {
	/// <summary>
	/// 	������ ����������
	/// </summary>
	[Flags]
	public enum SaveStage {
		/// <summary>
		/// 	����������� ���������
		/// </summary>
		None,

		/// <summary>
		/// 	�������� ������ ����������
		/// </summary>
		Load,

		/// <summary>
		/// 	�������� ����������� ���������� �� �������� ������������
		/// </summary>
		Authorize,

		/// <summary>
		/// 	���������� ������� ������ - ����������� ������������
		/// </summary>
		Prepare,

		/// <summary>
		/// 	�������� ����������� ������������ ����������
		/// </summary>
		Validate,

		/// <summary>
		/// 	�������� ����������� ����������, �������� �������� � ����
		/// </summary>
		Test,

		/// <summary>
		/// 	���������� ���������� �����
		/// </summary>
		Save,

		/// <summary>
		/// 	���������� ����������� �������� ����� ���������� ����������, ������-�������
		/// </summary>
		AfterSave,

		/// <summary>
		/// 	�������� ����������
		/// </summary>
		Finished,
	}
}
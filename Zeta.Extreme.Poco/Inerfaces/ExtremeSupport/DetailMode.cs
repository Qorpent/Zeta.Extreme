#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : DetailMode.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;

namespace Comdiv.Zeta.Model.ExtremeSupport {
	/// <summary>
	/// 	��� ������ � �������� ��� ������� ������� ���� "������" ��� ���������� ������,
	/// 	��� ������ � �������� �������� - �� ����� ����������� ������
	/// </summary>
	[Flags]
	public enum DetailMode {
		/// <summary>
		/// 	�������������� - ������� ���� �������� ������
		/// </summary>
		None,

		/// <summary>
		/// 	������������ ������ �������, ���������� - ������ ��������, ��� DETAIL IS NULL ��� �������� �� ������
		/// </summary>
		SafeObject,

		/// <summary>
		/// 	������ - ��� �����, ������������ �������� SUM(VALUE), �� ����� �������������� ��� ��������� ������ ��� �����
		/// </summary>
		SumObject,

		/// <summary>
		/// 	�� �� ����� ��� � SumObject, ������ ������������ ������� ��������, ��� DETAIL IS NOT NULL
		/// </summary>
		SafeSumObject,

		/// <summary>
		/// 	�������� ��� ������������� ������������ ������ �������� DETAILTYPE (���������� �� ������ �������� �������)
		/// </summary>
		TypedSumObject,
	}
}
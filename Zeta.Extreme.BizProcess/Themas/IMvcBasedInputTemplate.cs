#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IMvcBasedInputTemplate.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;

namespace Zeta.Extreme.BizProcess.Themas {
	/// <summary>
	/// 	��������� ����� ���������� ��� ����, ��������������� �� ��������� ��������� � MVC
	/// </summary>
	//[Obsolete("�� ����� ������ ��������� ��������� � ����� ����")] 
	public interface IMvcBasedInputTemplate {
		/// <summary>
		/// 	��������� ��� ��� MVC
		/// </summary>
		[Obsolete("�� ����� ������ ��������� ��������� � ����� ����")] string Controller { get; set; }

		/// <summary>
		/// 	������� ������������� �������������� ����
		/// </summary>
		[Obsolete("�� ����� ������ ��������� ��������� � ����� ����")] bool IsCustomView { get; }

		/// <summary>
		/// 	������������� ���������������� ���
		/// </summary>
		[Obsolete("�� ����� ������ ��������� ��������� � ����� ����")] string CustomView { get; set; }

		/// <summary>
		/// 	������������� ��� �����������
		/// </summary>
		[Obsolete("�� ����� ������ ��������� ��������� � ����� ����")] string CustomControllerType { get; set; }

		/// <summary>
		/// 	��� �������
		/// </summary>
		[Obsolete("�� ����� ������ ��������� ��������� � ����� ����")] string TableView { get; set; }
	}
}
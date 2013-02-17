#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IThemaFactory.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using System.Security.Principal;
using Comdiv.Reporting;
using Zeta.Extreme.Form.InputTemplates;

namespace Zeta.Extreme.Form.Themas {
	/// <summary>
	/// 	������� ���
	/// </summary>
	public interface IThemaFactory : IDisposable {
		/// <summary>
		/// 	��� ���
		/// </summary>
		IDictionary<string, object> Cache { get; }

		/// <summary>
		/// 	�������� XML
		/// </summary>
		string SrcXml { get; set; }

		/// <summary>
		/// 	������
		/// </summary>
		DateTime Version { get; set; }

		/// <summary>
		/// 	�������� ���� �� ����
		/// </summary>
		/// <param name="code"> </param>
		/// <returns> </returns>
		IThema Get(string code);

		/// <summary>
		/// 	�������� ��� ����
		/// </summary>
		/// <returns> </returns>
		IEnumerable<IThema> GetAll();

		/// <summary>
		/// 	�������� ��������� ������
		/// </summary>
		/// <param name="code"> </param>
		/// <returns> </returns>
		IReportDefinition GetReport(string code);

		/// <summary>
		/// 	�������� ���� � ��������� �� �������� ������������
		/// </summary>
		/// <returns> </returns>
		IEnumerable<IThema> GetForUser();

		/// <summary>
		/// 	�������� ���� � ��������� �� ����������� ������������
		/// </summary>
		/// <param name="usr"> </param>
		/// <returns> </returns>
		IEnumerable<IThema> GetForUser(IPrincipal usr);

		/// <summary>
		/// 	�������� ������ �����
		/// </summary>
		/// <param name="code"> </param>
		/// <param name="throwerror"> </param>
		/// <returns> </returns>
		IInputTemplate GetForm(string code, bool throwerror = false);

		/// <summary>
		/// 	�������� ��� ������������
		/// </summary>
		/// <param name="usrname"> </param>
		void CleanUser(string usrname);
	}
}
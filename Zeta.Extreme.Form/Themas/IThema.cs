// // Copyright 2007-2010 Comdiv (F. Sadykov) - http://code.google.com/u/fagim.sadykov/
// // Supported by Media Technology LTD 
// //  
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// //  
// //      http://www.apache.org/licenses/LICENSE-2.0
// //  
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
// // 
// // MODIFICATIONS HAVE BEEN MADE TO THIS FILE

using System;
using System.Collections.Generic;
using System.Security.Principal;
using Comdiv.Model.Interfaces;
using Comdiv.Reporting;
using Comdiv.Zeta.Model;
using Comdiv.Zeta.Web.InputTemplates;

namespace Comdiv.Zeta.Web.Themas{
	/// <summary>
	/// ������� ��������� ����
	/// </summary>
    public interface IThema : IPseudoThema{
		/// <summary>
		/// ������ �� �������, ��������� ����
		/// </summary>
        IThemaFactory Factory { get; set; }
		/// <summary>
		/// ���� ������� � ����
		/// </summary>
        string Role { get; set; }
		/// <summary>
		/// ������� ����, ��� ���� - ������
		/// </summary>
        bool IsGroup { get; set; }
		/// <summary>
		/// ��� ������
		/// </summary>
        string Group { get; set; }

		/// <summary>
		/// ��������� ����
		/// </summary>
        IDictionary<string, object> Parameters { get; }
		/// <summary>
		/// ������� ���������
		/// </summary>
        bool Visible { get; set; }
		/// <summary>
		/// ������� � �������
		/// </summary>
        int Idx { get; set; }
		/// <summary>
		/// ������� ���� - ��� ���� - ������
		/// </summary>
        bool IsTemplate { get; set; }
		/// <summary>
		/// ������������ ����
		/// </summary>
        IThema ParentThema { get; set; }
		/// <summary>
		/// ��� ������������ ����
		/// </summary>
        string Parent { get; set; }
		/// <summary>
		/// �������� ����
		/// </summary>
        IList<IThema> Children { get; }
		/// <summary>
		/// ������� ��������� ���� (��� ��������� �����)
		/// </summary>
        bool IsFavorite { get; set; }
		/// <summary>
		/// ��������� ������, ��������� ��� ��������� ����
		/// </summary>
    	Exception Error { get; set; }
		/// <summary>
		/// �������� ��� �����
		/// </summary>
		/// <returns></returns>
    	IEnumerable<IInputTemplate> GetAllForms();
		/// <summary>
		/// �������� ��� ������
		/// </summary>
		/// <returns></returns>
        IEnumerable<IReportDefinition> GetAllReports();
		/// <summary>
		/// �������� ���������� �����
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
        IInputTemplate GetForm(string code);
		/// <summary>
		/// �������� ���������� �����
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
        IReportDefinition GetReport(string code);
		/// <summary>
		/// ������� ����� ���� ��� ����������� ������������
		/// </summary>
		/// <param name="usr"></param>
		/// <returns></returns>
        IThema Personalize(IPrincipal usr);
		/// <summary>
		/// �������� ��� ���������� ���������
		/// </summary>
		/// <returns></returns>
        IEnumerable<IDocument> GetAllDocuments();
		/// <summary>
		/// �������� ��� ���������� �������
		/// </summary>
		/// <returns></returns>
        IEnumerable<ICommand> GetAllCommands();
		/// <summary>
		/// ������������ ���� ��� ���������� ������
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="year"></param>
		/// <param name="period"></param>
		/// <returns></returns>
        IThema Accomodate(IZetaMainObject obj, int year, int period);
		/// <summary>
		/// ����������� ����� ���������� ���� � ������� � ������ ���� ���������
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="year"></param>
		/// <param name="period"></param>
		/// <param name="statecache"></param>
		/// <returns></returns>
        IThema Accomodate(IZetaMainObject obj, int year, int period,IDictionary<string ,object >statecache);
		/// <summary>
		/// �������� ���������� ��������
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
        IDocument GetDocument(string code);
		/// <summary>
		/// �������� ���������� �������
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
        ICommand GetCommand(string code);
		/// <summary>
		/// �������� ������ ������
		/// </summary>
		/// <returns></returns>
        IEnumerable<IThema> GetGroup();
		/// <summary>
		/// ������� ���������� ���� ��� ����������� ������������
		/// </summary>
		/// <param name="usr"></param>
		/// <returns></returns>
        bool IsActive(IPrincipal usr);
		/// <summary>
		/// ������ ��������� ���� ��� �������� ������������
		/// </summary>
		/// <returns></returns>
        bool IsVisible();
		/// <summary>
		/// ������ ��������� ���� ��� ���������� ������������
		/// </summary>
		/// <param name="usr"></param>
		/// <returns></returns>
        bool IsVisible(IPrincipal usr);
		/// <summary>
		/// �������������� �������� ������� ���������
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="code"></param>
		/// <param name="def"></param>
		/// <returns></returns>
        T GetParameter<T>(string code, T def);
    }
}
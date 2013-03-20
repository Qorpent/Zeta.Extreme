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
// PROJECT ORIGIN: Zeta.Extreme.BizProcess/IThemaFactory.cs
#endregion
using System;
using System.Collections.Generic;
using System.Security.Principal;
using Zeta.Extreme.BizProcess.Reports;

namespace Zeta.Extreme.BizProcess.Themas {
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
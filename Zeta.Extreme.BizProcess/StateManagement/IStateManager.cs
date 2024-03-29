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
// PROJECT ORIGIN: IStateManager.cs

#endregion

using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.BizProcess.StateManagement {
	/// <summary>
	///     ��������� ������ ���������� ���������
	/// </summary>
	public interface IStateManager {
		/// <summary>
		///     ������� ����������� ���������� ������
		/// </summary>
		/// <param name="template"></param>
		/// <param name="obj"></param>
		/// <param name="detail"></param>
		/// <param name="state"></param>
		/// <returns>
		/// </returns>
		bool CanSet(IInputTemplate template, IZetaMainObject obj, IZetaDetailObject detail, string state);

		/// <summary>
		///     ��������� ��������� �������
		/// </summary>
		/// <param name="template"></param>
		/// <param name="obj"></param>
		/// <param name="detail"></param>
		/// <param name="state"></param>
		/// <param name="parent"></param>
		void Process(IInputTemplate template, IZetaMainObject obj, IZetaDetailObject detail, string state, int parent);

		/// <summary>
		///     ����� ��������� �����
		/// </summary>
		/// <param name="source"></param>
		/// <returns>
		/// </returns>
		IInputTemplate[] GetDependentTemplates(IInputTemplate source);

		/// <summary>
		///     ����� ����� �� ������� ������� �������
		/// </summary>
		/// <param name="target"></param>
		/// <param name="obj"></param>
		/// <returns>
		/// </returns>
		IInputTemplate[] GetSourceTemplates(IInputTemplate target, IZetaMainObject obj);

		/// <summary>
		///     ���������� ������� �����
		/// </summary>
		/// <param name="safer"></param>
		/// <returns>
		/// </returns>
		IInputTemplate GetMainTemplate(IInputTemplate safer);

		/// <summary>
		///     ����������� �����-������
		/// </summary>
		/// <param name="main"></param>
		/// <returns>
		/// </returns>
		IInputTemplate GetSaferTemplate(IInputTemplate main);

		/// <summary>
		///     ���������� ����������� ���������� ������
		/// </summary>
		/// <param name="template"></param>
		/// <param name="obj"></param>
		/// <param name="detail"></param>
		/// <param name="state"></param>
		/// <param name="cause"></param>
		/// <returns>
		/// </returns>
		bool CanSet(IInputTemplate template, IZetaMainObject obj, IZetaDetailObject detail, string state,
		            out string cause);

		/// <summary>
		///     ��������� ��������� �������
		/// </summary>
		/// <param name="objid"></param>
		/// <param name="year"></param>
		/// <param name="period"></param>
		/// <param name="template"></param>
		/// <param name="templatecode"></param>
		/// <param name="usr"></param>
		/// <param name="state"></param>
		/// <param name="comment"></param>
		/// <param name="parent"></param>
		/// <returns>
		/// </returns>
		int DoSet(int objid, int year, int period, string template, string templatecode, string usr, string state,
		          string comment, int parent);

		/// <summary>
		///     ��������� ��������� �������
		/// </summary>
		/// <param name="objid"></param>
		/// <param name="year"></param>
		/// <param name="period"></param>
		/// <param name="template"></param>
		/// <returns>
		/// </returns>
		string DoGet(int objid, int year, int period, string template);

		/// <summary>
		///     �������� ������ �������
		/// </summary>
		/// <param name="year"></param>
		/// <param name="period"></param>
		/// <param name="getParameter"></param>
		/// <returns>
		/// </returns>
		int GetPeriodState(int year, int period, string getParameter);

		/// <summary>
		///     ��� ���� ������� �������� ����������� ��������� �������
		/// </summary>
		/// <param name="template"></param>
		/// <param name="obj"></param>
		/// <param name="detail"></param>
		/// <param name="state"></param>
		/// <param name="cause"></param>
		/// <param name="parent"></param>
		/// <returns>
		/// </returns>
		bool CanSet(IInputTemplate template, IZetaMainObject obj, IZetaDetailObject detail, string state,
		            out string cause, int parent);
	}
}
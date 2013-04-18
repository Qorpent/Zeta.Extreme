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
// PROJECT ORIGIN: IFormStateRepository.cs

#endregion

using System;
using System.Security.Principal;
using Zeta.Extreme.BizProcess.Forms;
using Zeta.Extreme.Model;

namespace Zeta.Extreme.BizProcess.StateManagement {
	/// <summary>
	///     ��������� ������������� ��������� � ������ ������� �����
	/// </summary>
	public interface IFormStateRepository {
		/// <summary>
		///     ���������� �������� ������ �����
		/// </summary>
		/// <param name="form"></param>
		/// <returns>
		/// </returns>
		Form GetFormRecord(IFormSession form);

		/// <summary>
		///     ���������� ������� �������� ��� �����
		/// </summary>
		/// <param name="form"></param>
		/// <returns>
		/// </returns>
		FormState[] GetFormStateHistory(IFormSession form);


		/// <summary>
		///     ���������� ��������� ������ � ����� �������
		/// </summary>
		/// <param name="form"></param>
		/// <returns>
		/// </returns>
		FormState GetLastFormState(IFormSession form);

		/// <summary>
		///     ������������� ����� ������ ��� �����
		/// </summary>
		/// <param name="form"></param>
		/// <param name="stateType"></param>
		/// <param name="parentId"></param>
		/// <param name="principal">������� ������ ������������</param>
		/// <param name="comment"></param>
		FormState SetState(IFormSession form, FormStateType stateType, string comment, int parentId, IPrincipal principal = null);
	}
}
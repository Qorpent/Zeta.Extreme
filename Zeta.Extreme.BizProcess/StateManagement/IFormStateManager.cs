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
// PROJECT ORIGIN: IFormStateManager.cs

#endregion

using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.BizProcess.StateManagement {
	/// <summary>
	///     ����� ��������� ��������� ��������
	/// </summary>
	public interface IFormStateManager {
		/// <summary>
		///     ���������� ���������� �������� ���������� �� ��������
		/// </summary>
		/// <param name="repository"></param>
		void SetStateRepository(IFormStateRepository repository);

		/// <summary>
		///     ��������� ���������� ��� �������� ����������� ��������� �������
		/// </summary>
		/// <param name="checker"></param>
		void RegisterCanSetChecker(IFormStateAvailabilityChecker checker);


		/// <summary>
		///     �������� ����������� ��������� �������
		/// </summary>
		/// <param name="form"></param>
		/// <param name="newStateType"></param>
		/// <returns>
		/// </returns>
		FormStateOperationResult GetCanSet(IFormSession form, FormStateType newStateType);

		/// <summary>
		///     ������������� ����� ������ ��� �����
		/// </summary>
		/// <param name="form"></param>
		/// <param name="newStateType"></param>
		/// <param name="comment"></param>
		/// <param name="parentId"></param>
		/// <returns>
		/// </returns>
		FormStateOperationResult SetState(IFormSession form, FormStateType newStateType, string comment="", int parentId = 0);

		/// <summary>
		/// ������� ��������� �������� ���� ������� �����
		/// </summary>
		/// <param name="session"></param>
		/// <returns></returns>
		FormStateType GetCurrentState(IFormSession session);
	}
}
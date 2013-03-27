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
// PROJECT ORIGIN: Zeta.Extreme.Core/ISessionWithExtendedServices.cs
#endregion
using System;
using System.Threading.Tasks;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// ��������� ��� ����������� ���������� ����� ������ � ����������
	/// </summary>
	public interface ISessionWithExtendedServices:ISession {


		/// <summary>
		/// 	���������� ����������� ���������� ������� � ����������
		/// 	���������� �� �� ������, ��� � �����������
		/// </summary>
		/// <param name="query"> </param>
		/// <returns> </returns>
		Task PrepareAsync(IQuery query);

		/// <summary>
		/// 	������� ��������� ���� ��������� ����������� �����������
		/// </summary>
		/// <param name="timeout"> </param>
		void WaitPreparation(int timeout = -1);

		/// <summary>
		/// 	������� ��������� ���� ��������� ����������� �����������
		/// </summary>
		/// <param name="timeout"> </param>
		void WaitEvaluation(int timeout = -1);

		/// <summary>
		/// 	���������� ������ ���������������� ������ �����������
		/// </summary>
		/// <returns> </returns>
		/// <exception cref="NotImplementedException"></exception>
		IQueryPreparator GetPreparator();

		/// <summary>
		/// 	���������� ������������ � ���
		/// </summary>
		/// <param name="preparator"> </param>
		void Return(IQueryPreparator preparator);

		/// <summary>
		/// 	���������� ������ ���������������� ������ �����������
		/// </summary>
		/// <returns> </returns>
		/// <exception cref="NotImplementedException"></exception>
		IRegistryHelper GetRegistryHelper();

		/// <summary>
		/// 	���������� ������������ � ���
		/// </summary>
		/// <param name="helper"> </param>
		void ReturnRegistryHelper(IRegistryHelper helper);

		/// <summary>
		/// 	���������� ������ �������������
		/// </summary>
		/// <returns> </returns>
		/// <exception cref="NotImplementedException"></exception>
		IPreloadProcessor GetPreloadProcessor();

		/// <summary>
		/// 	���������� ������������ � ���
		/// </summary>
		/// <param name="processor"> </param>
		void Return(IPreloadProcessor processor);


		/// <summary>
		/// 	������ �������������� ���������� ����� � �������� �������� ����������
		/// </summary>
		/// <param name="timeout"> </param>
		void SyncPreEval(int timeout);
	}
}
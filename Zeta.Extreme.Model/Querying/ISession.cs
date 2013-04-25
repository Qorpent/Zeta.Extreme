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
// PROJECT ORIGIN: Zeta.Extreme.Model/ISession.cs
#endregion
using System;
using System.Threading.Tasks;

namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// 	�����������, Zeta.Extrem cec���, ������� ���������
	/// </summary>
	public interface ISession {
		

		/// <summary>
		/// </summary>
		/// <param name="key"> </param>
		/// <param name="timeout"> </param>
		/// <returns> </returns>
		QueryResult Get(string key, int timeout = -1);

		/// <summary>
		/// 	���������� ����������� ������� � ������
		/// </summary>
		/// <param name="query"> �������� ������ </param>
		/// <param name="uid"> ��������� ���� ������� ��������� ��� ��� ����������� ���������������� ��������� �������� </param>
		/// <returns> ������ �� ������ ����������� � ������ </returns>
		/// <remarks>
		/// 	��� ����������� �������, �� �������� �������������� ����������� � �������� �� ������,
		/// 	������������ ������ �������� ������
		/// </remarks>
		/// <exception cref="NotImplementedException"></exception>
		IQuery Register(IQuery query, string uid = null);

		/// <summary>
		/// 	����������� ����������� ������� � ������
		/// </summary>
		/// <param name="query"> �������� ������ </param>
		/// <param name="uid"> ��������� ���� ������� ��������� ��� ��� ����������� ���������������� ��������� �������� </param>
		/// <returns> ������, �� ����������� ������� ������������ ������ �� ������ ����������� � ������ </returns>
		/// <remarks>
		/// 	��� ����������� �������, �� �������� �������������� ����������� � �������� �� ������,
		/// 	������������ ������ �������� ������
		/// </remarks>
		Task<IQuery> RegisterAsync(IQuery query, string uid = null);

		/// <summary>
		/// 	��������� ������������� � ������ �������� � ������
		/// </summary>
		/// <param name="timeout"> </param>
		void Execute(int timeout = -1);

		/// <summary>
		/// �������� �������������� ����������
		/// </summary>
		ISessionPropertySource PropertySource { get; set; }

		/// <summary>
		/// ���� ������������� ������������������ ���������� ��������
		/// </summary>
		bool UseSyncPreparation { get; set; }
	}
}
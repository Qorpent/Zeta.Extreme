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
// PROJECT ORIGIN: Zeta.Extreme.Model/IQuery.cs
#endregion
using System;

namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// 	��������� ��������� �������
	/// </summary>
	public interface IQuery : IWithCacheKey {
		/// <summary>
		/// 	������� �� �����
		/// </summary>
		ITimeHandler Time { get; set; }

		/// <summary>
		/// 	������� �� ������
		/// </summary>
		IRowHandler Row { get; set; }

		/// <summary>
		/// 	������� �� �������
		/// </summary>
		IColumnHandler Col { get; set; }

		/// <summary>
		/// 	������� �� ������
		/// </summary>
		IObjHandler Obj { get; set; }

		/// <summary>
		/// 	�������� ������
		/// </summary>
		string Valuta { get; set; }

		/// <summary>
		/// ������� ������������ ��������
		/// </summary>
		QueryResult Result { get; set; }

		/// <summary>
		/// 	������������ ������� ���������� ������� � ���������
		/// </summary>
		/// <param name="timeout"> </param>
		/// <returns> </returns>
		/// <exception cref="Exception"></exception>
		QueryResult GetResult(int timeout = -1);

		/// <summary>
		/// 	�������������� ��� �������, ������������� ��������
		/// </summary>
		long Uid { get; set; }

		/// <summary>
		/// 	�������� ������ �� ������
		/// </summary>
		ISession Session { get; set; }

		/// <summary>
		/// 	������� ����� ������� �� �����
		/// </summary>
		/// <param name="deep"> ���� ��, �� ������ ����� ��������� ��������� </param>
		/// <returns> </returns>
		IQuery Copy(bool deep = false);

		/// <summary>
		/// 	����������� ��������� ������������
		/// </summary>
		void Normalize(ISession session = null);

	
	}
}
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
// PROJECT ORIGIN: Zeta.Extreme.Model/ITimeHandler.cs
#endregion
namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// 	����������� ��������� ��������� �������
	/// </summary>
	public interface ITimeHandler : IWithCacheKey {
		/// <summary>
		/// 	���
		/// </summary>
		int Year { get; set; }

		/// <summary>
		/// 	������
		/// </summary>
		int Period { get; set; }

		/// <summary>
		/// 	����� �� ���������� �����
		/// </summary>
		int[] Years { get; set; }

		/// <summary>
		/// 	����� ��������
		/// </summary>
		int[] Periods { get; set; }

		/// <summary>
		/// 	������� ��� (��� �������� ��������)
		/// </summary>
		int BaseYear { get; set; }

		/// <summary>
		/// 	������� ������ (��� �������� ��������)
		/// </summary>
		int BasePeriod { get; set; }

		/// <summary>
		/// 	True ���� ������ �������� � ���������
		/// </summary>
		/// <returns> </returns>
		bool IsPeriodDefined();

		/// <summary>
		/// 	True ���� ��� ��� �������� � ���������
		/// </summary>
		/// <returns> </returns>
		bool IsYearDefinied();

		/// <summary>
		/// 	������� ����� ������� �� �����
		/// </summary>
		/// <returns> </returns>
		ITimeHandler Copy();

		/// <summary>
		/// 	����������� ���������� ���� � �������
		/// </summary>
		/// <param name="session"> </param>
		void Normalize(ISession session = null);
	}
}
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
// PROJECT ORIGIN: Zeta.Extreme.Model/SessionStatistics.cs
#endregion
using System;
using Qorpent.Serialization;

namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// ������������� �������������� ������ �� ������
	/// </summary>
	[Serialize]
	public class SessionStatistics {
		
		/// <summary>
		/// 	���������� ������
		/// </summary>
		[SerializeNotNullOnly]
		public int BatchCount;

		/// <summary>
		/// 	���������� ������� ������
		/// </summary>
		[SerializeNotNullOnly]
		public TimeSpan BatchTime;

		/// <summary>
		/// 	���������� �������������� ��������
		/// </summary>
		[SerializeNotNullOnly]
		public int PrimaryAffected;

		/// <summary>
		/// 	���������� ����������� �����
		/// </summary>
		[SerializeNotNullOnly]
		public int PrimaryCatched;

		/// <summary>
		/// 	������� ������
		/// </summary>
		[SerializeNotNullOnly]
		public int QueryTypeFormula;

		/// <summary>
		/// 	������� ��������� ��������
		/// </summary>
		[SerializeNotNullOnly]
		public int QueryTypePrimary;

		/// <summary>
		/// 	������� ����
		/// </summary>
		[SerializeNotNullOnly]
		public int QueryTypeSum;

		/// <summary>
		/// 	������� ������������ ��������
		/// </summary>
		[SerializeNotNullOnly]
		public int RegistryIgnored;

		/// <summary>
		/// 	���������� ������������� ���������� �����������
		/// </summary>
		[SerializeNotNullOnly]
		public int RegistryNew;

		/// <summary>
		/// 	���������� ������� �������������
		/// </summary>
		[SerializeNotNullOnly]
		public int RegistryPreprocessed;

		/// <summary>
		/// 	���������� ����������� �� ����������� �����
		/// </summary>
		[SerializeNotNullOnly]
		public int RegistryResolvedByKey;

		/// <summary>
		/// 	���������� ���������� ������������� �������� ��� ��������������
		/// </summary>
		[SerializeNotNullOnly]
		public int RegistryResolvedByMapKey;

		/// <summary>
		/// 	���������� ����������� �� ������� � ����
		/// </summary>
		[SerializeNotNullOnly]
		public int RegistryResolvedByUid;

		/// <summary>
		/// 	���������� ���������� ������� �����������
		/// </summary>
		[SerializeNotNullOnly]
		public int RegistryStarted;

		/// <summary>
		/// 	���������� ��������������� �����������
		/// </summary>
		[SerializeNotNullOnly]
		public int RegistryStartedUser;

		/// <summary>
		/// 	������� �������������� ���������� ��������
		/// </summary>
		[SerializeNotNullOnly]
		public int RegistryUser;

		/// <summary>
		/// 	������� ��������� ������
		/// </summary>
		[SerializeNotNullOnly]
		public int RowRedirections;

	

		/// <summary>
		/// 	���������� ������ ������� ����������
		/// </summary>
		[SerializeNotNullOnly]
		public TimeSpan TimeTotal;
	}
}
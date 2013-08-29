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
// PROJECT ORIGIN: PeriodStateRecord.cs

#endregion

using System;
using Qorpent;
using Qorpent.Serialization;

namespace Zeta.Extreme.BizProcess.StateManagement {
	/// <summary>
	///     ������ ������� �������
	/// </summary>
	[Serialize]
	public sealed class PeriodStateRecord {
		/// <summary>
		///     ������� ����������� ������
		/// </summary>
		public PeriodStateRecord() {
			DeadLine = QorpentConst.Date.Begin;
		}

		/// <summary>
		///     �������
		/// </summary>
		public DateTime DeadLine;

		/// <summary>
		///     ������
		/// </summary>
		public int Period;

		/// <summary>
		///     ������
		/// </summary>
		public bool State;

		/// <summary>
		///     ����� �������
		/// </summary>
		public DateTime UDeadLine;

		/// <summary>
		///     ���
		/// </summary>
		public int Year;

		/// <summary>
		/// ������ ����������
		/// </summary>
		public string Grp;
	}
}
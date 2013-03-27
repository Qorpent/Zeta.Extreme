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
// PROJECT ORIGIN: Zeta.Extreme.BizProcess/IFormSession.cs
#endregion
using System.Collections.Generic;
using Qorpent.Log;
using Qorpent.Serialization;
using Zeta.Extreme.BizProcess.StateManagement;
using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.BizProcess.Forms {
	/// <summary>
	/// 	������� ��������� ������
	/// </summary>
	public interface IFormSession {
		/// <summary>
		/// 	������������� ������
		/// </summary>
		string Uid { get; }

		/// <summary>
		/// 	���
		/// </summary>
		int Year { get; }

		/// <summary>
		/// 	������
		/// </summary>
		int Period { get; }

		/// <summary>
		/// 	������
		/// </summary>
		[IgnoreSerialize] IZetaMainObject Object { get; }

		/// <summary>
		/// 	������
		/// </summary>
		[IgnoreSerialize] IInputTemplate Template { get; }

		/// <summary>
		/// 	������������
		/// </summary>
		string Usr { get; }

		/// <summary>
		/// 	������ ��� �������������� ������
		/// </summary>
		[IgnoreSerialize] List<OutCell> Data { get; }

		/// <summary>
		/// ������
		/// </summary>
		IUserLog Logger { get; set; }

		/// <summary>
		/// 	���������� ��������� ���������� �� ����� � ���������� �������� "�������" ����������
		/// </summary>
		/// <returns> </returns>
		LockStateInfo GetCurrentLockInfo();
	}
}
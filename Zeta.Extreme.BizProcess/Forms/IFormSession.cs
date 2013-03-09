#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IFormSession.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using Qorpent.Serialization;
using Zeta.Extreme.BizProcess.StateManagement;
using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.Poco.Inerfaces;

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
		/// 	���������� ��������� ���������� �� ����� � ���������� �������� "�������" ����������
		/// </summary>
		/// <returns> </returns>
		LockStateInfo GetCurrentLockInfo();
	}
}
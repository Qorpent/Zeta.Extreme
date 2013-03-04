#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : PeriodStateRecord.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Comdiv.Extensions;

namespace Zeta.Extreme.BizProcess.StateManagement {
	/// <summary>
	/// 	������ ������� �������
	/// </summary>
	public sealed class PeriodStateRecord {
		///<summary>
		///	������� ����������� ������
		///</summary>
		public PeriodStateRecord() {
			DeadLine = DateExtensions.Begin;
		}

		/// <summary>
		/// 	�������
		/// </summary>
		public DateTime DeadLine;

		/// <summary>
		/// 	������
		/// </summary>
		public int Period;

		/// <summary>
		/// 	������
		/// </summary>
		public bool State;

		/// <summary>
		/// 	����� �������
		/// </summary>
		public DateTime UDeadLine;

		/// <summary>
		/// 	���
		/// </summary>
		public int Year;
	}
}
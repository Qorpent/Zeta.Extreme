#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : LockStateInfo.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.Form.StateManagement {
	/// <summary>
	/// 	���������� � ������� �������� �����
	/// </summary>
	public class LockStateInfo {
		/// <summary>
		/// 	����������� ���������� �����
		/// </summary>
		public bool canblock;

		/// <summary>
		/// 	������� ����������� ����������
		/// </summary>
		public bool cansave;

		/// <summary>
		/// 	����������������� ������� �������� �����
		/// </summary>
		public bool isopen;

		/// <summary>
		/// 	��������� �� ������ ����������
		/// </summary>
		public string message;

		/// <summary>
		/// 	������� ������
		/// </summary>
		public string state;
	}
}
#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : PrepareState.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme {
	/// <summary>
	/// 	������ ���������� �������
	/// </summary>
	public enum PrepareState {
		/// <summary>
		/// 	�� �����
		/// </summary>
		None,

		/// <summary>
		/// 	������ ������
		/// </summary>
		TaskStarted,

		/// <summary>
		/// 	���� ������� ����������
		/// </summary>
		InPrepare,

		/// <summary>
		/// 	������
		/// </summary>
		Prepared
	}
}
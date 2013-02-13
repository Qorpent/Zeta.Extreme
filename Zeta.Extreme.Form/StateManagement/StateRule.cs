#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : StateRule.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.Form.StateManagement {
	/// <summary>
	/// 	������� �� ������
	/// </summary>
	public class StateRule {
		/// <summary>
		/// 	������� �����
		/// </summary>
		public string Current { get; set; }

		/// <summary>
		/// 	������� ����� (?)
		/// </summary>
		public string Target { get; set; }

		/// <summary>
		/// 	���
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// 	��������
		/// </summary>
		public string Action { get; set; }

		/// <summary>
		/// 	������� ������
		/// </summary>
		public string CurrentState { get; set; }

		/// <summary>
		/// 	�������������� ������
		/// </summary>
		public string ResultState { get; set; }
	}
}
#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : PrimaryQueryPrototype.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.Primary {
	/// <summary>
	/// 	��������� ��������� ���������� �������
	/// </summary>
	public struct PrimaryQueryPrototype {
		/// <summary>
		/// 	������� ������������� ���������
		/// </summary>
		public bool UseSum { get; set; }

		/// <summary>
		/// 	������ �� ������������� �������
		/// </summary>
		public bool PreserveDetails { get; set; }

		/// <summary>
		/// 	��������c�� � ������������� �������
		/// </summary>
		public bool RequireDetails { get; set; }

		/// <summary>
		/// 	������������� ������������ ������ ������� � ��������� ���������
		/// </summary>
		public bool RequreZetaEval { get; set; }
	}
}
#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : DivisionRecord.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Serialization;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// 	������ � ���������
	/// </summary>
	[Serialize]
	public class DivisionRecord {
		/// <summary>
		/// 	��� ���������
		/// </summary>
		public string code;

		/// <summary>
		/// 	������ ���������
		/// </summary>
		public int idx;

		/// <summary>
		/// 	�������� ���������
		/// </summary>
		public string name;
	}
}
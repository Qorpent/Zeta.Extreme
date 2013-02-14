#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : OutCell.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Serialization;

namespace Zeta.Extreme.FrontEnd.Session {
	/// <summary>
	/// 	��������� ������ ������
	/// </summary>
	[Serialize]
	public class OutCell {
		/// <summary>
		/// 	������ �� Id ������ � ��
		/// </summary>
		[SerializeNotNullOnly] public int c;

		/// <summary>
		/// 	���������� �� ������
		/// </summary>
		public string i;

		/// <summary>
		/// 	�������� ������
		/// </summary>
		[SerializeNotNullOnly] public string v;
	}
}
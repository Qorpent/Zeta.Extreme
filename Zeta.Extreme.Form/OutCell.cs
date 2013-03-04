#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : OutCell.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Serialization;

namespace Zeta.Extreme.Form {
	/// <summary>
	/// 	��������� ������ ������
	/// </summary>
	[Serialize]
	public class OutCell : OutCellBase {
		/// <summary>
		/// 	������ �� ������ ��� ����������� ��������
		/// </summary>
		[IgnoreSerialize] public Query query;
	}
}
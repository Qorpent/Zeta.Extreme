#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : AccessibleObjects.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Serialization;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// 	��������� ��������� ������� ��������
	/// </summary>
	[Serialize]
	public class AccessibleObjects {
		/// <summary>
		/// 	��������� ���������
		/// </summary>
		[Serialize] public DivisionRecord[] divs { get; set; }

		/// <summary>
		/// 	��������� �������
		/// </summary>
		[Serialize] public ObjectRecord[] objs { get; set; }
	}
}
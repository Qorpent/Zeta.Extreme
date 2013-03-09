#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : ObjectRecord.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Serialization;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// 	������ � ������� �������
	/// </summary>
	[Serialize]
	public class ObjectRecord {
		/// <summary>
		/// 	��������
		/// </summary>
		public string div;

		/// <summary>
		/// 	�� ������� (ClassicId)
		/// </summary>
		public int id;

		/// <summary>
		/// 	������ ������� � ������ ����
		/// </summary>
		public int idx;

		/// <summary>
		/// 	�������� �������
		/// </summary>
		public string name;

		/// <summary>
		/// 	�������� ���
		/// </summary>
		public string shortname;
	}
}
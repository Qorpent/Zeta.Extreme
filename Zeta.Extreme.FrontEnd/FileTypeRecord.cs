#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : FileTypeRecord.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Serialization;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// 	��������� ������������� �����
	/// </summary>
	[Serialize]
	public class FileTypeRecord {
		/// <summary>
		/// 	���
		/// </summary>
		public string code { get; set; }

		/// <summary>
		/// 	���
		/// </summary>
		public string name { get; set; }
	}
}
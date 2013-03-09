#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : SimpleUserInfo.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.FrontEnd.Helpers {
	/// <summary>
	/// 	���������� ��������� ������������
	/// </summary>
	public class SimpleUserInfo {
		/// <summary>
		/// 	�����
		/// </summary>
		public string Login { get; set; }

		/// <summary>
		/// 	���
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 	���������
		/// </summary>
		public string Dolzh { get; set; }

		/// <summary>
		/// 	���������� ������
		/// </summary>
		public string Contact { get; set; }

		/// <summary>
		/// 	����������� �����
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// 	������������� �����������
		/// </summary>
		public int ObjId { get; set; }

		/// <summary>
		/// 	��� �����������
		/// </summary>
		public string ObjName { get; set; }

		/// <summary>
		/// 	������� �������������� �����������
		/// </summary>
		public bool IsObjAdmin { get; set; }

		/// <summary>
		/// 	������� ���������� ������������
		/// </summary>
		public bool Active { get; set; }
	}
}
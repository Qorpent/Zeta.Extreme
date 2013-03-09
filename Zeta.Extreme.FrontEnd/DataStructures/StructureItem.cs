#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : StructureItem.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Serialization;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// 	��������� ������� ���������
	/// </summary>
	[Serialize]
	public class StructureItem {
		/// <summary>
		/// 	��� ������/�������
		/// </summary>
		public string code;

		/// <summary>
		/// 	������� ����������� �����
		/// </summary>
		[SerializeNotNullOnly] public bool controlpoint;

		/// <summary>
		/// 	������ � �������
		/// </summary>
		public int idx;

		/// <summary>
		/// 	������� �����
		/// </summary>
		[SerializeNotNullOnly] public bool iscaption;

		/// <summary>
		/// 	������� �����������
		/// </summary>
		[SerializeNotNullOnly] public bool isprimary;

		/// <summary>
		/// 	�������
		/// </summary>
		[SerializeNotNullOnly] public int level;

		///<summary>
		///	������� ���������
		///</summary>
		[SerializeNotNullOnly] public string measure;

		/// <summary>
		/// 	�������� ������/�������
		/// </summary>
		public string name;

		/// <summary>
		/// 	����� ������
		/// </summary>
		[SerializeNotNullOnly] public string number;

		/// <summary>
		/// 	������ ��� �������
		/// </summary>
		[SerializeNotNullOnly] public int period;

		/// <summary>
		/// 	r or c
		/// </summary>
		public string type;

		/// <summary>
		/// 	��� ��� �������
		/// </summary>
		[SerializeNotNullOnly] public int year;
	}
}
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
	public class OutCell {
		/// <summary>
		/// 	������ �� Id ������ � ��
		/// </summary>
		[SerializeNotNullOnly] public int c;

		/// <summary>
		/// 	������� ��������, ������� ����� ���� ����� ����������
		/// </summary>
		[IgnoreSerialize] public bool canbefilled;

		/// <summary>
		/// 	���������� �� ������
		/// </summary>
		public string i;

		/// <summary>
		/// 	��������� ������� ��� ������ � ������ �������
		/// </summary>
		[IgnoreSerialize] public OutCell linkedcell;

		/// <summary>
		/// 	������ �� ������ ��� ����������� ��������
		/// </summary>
		[IgnoreSerialize] public Query query;

		/// <summary>
		/// 	�������� ������
		/// </summary>
		[SerializeNotNullOnly] public string v;
	}
}
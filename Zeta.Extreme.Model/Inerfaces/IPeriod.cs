#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IPeriod.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Qorpent.Model;

namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// ��������� �������
	/// </summary>
	public interface IPeriod : IEntity, IWithFormula {
		/// <summary>
		/// ������ ��, ������������ � ����������
		/// </summary>
		int BizId { get; set; }
		/// <summary>
		/// ��������� �������
		/// </summary>
		string Category { get; set; }
		/// <summary>
		/// ����������� ���
		/// </summary>
		string ShortName { get; set; }
		/// <summary>
		/// ������� ������� ������ ���
		/// </summary>
		bool IsDayPeriod { get; set; }
		/// <summary>
		/// ��������� ���� ������� (�����������, 1899-01-01 ��� �������� 0)
		/// </summary>
		DateTime StartDate { get; set; }
		/// <summary>
		/// �������� ���� ������� (�����������, 1899-01-01 ��� �������� 0)
		/// </summary>
		DateTime EndDate { get; set; }
		/// <summary>
		/// ���������� ������� � �������
		/// </summary>
		int MonthCount { get; set; }
	}
}
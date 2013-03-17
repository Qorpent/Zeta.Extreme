#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : Period.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model {
	/// <summary>
	/// ������������� ���������� �������
	/// </summary>
	public class Period : IPeriod {
		/// <summary>
		/// 	������ �����
		/// </summary>
		public virtual string Tag { get; set; }

		/// <summary>
		/// ������ ��, ������������ � ����������
		/// </summary>
		public virtual int BizId { get; set; }

		/// <summary>
		/// ��������� �������
		/// </summary>
		public virtual string Category { get; set; }

		/// <summary>
		/// ����������� ���
		/// </summary>
		public virtual string ShortName { get; set; }

		/// <summary>
		/// ������� ������� ������ ���
		/// </summary>
		public virtual bool IsDayPeriod { get; set; }

		/// <summary>
		/// ��������� ���� ������� (�����������, 1899-01-01 ��� �������� 0)
		/// </summary>
		public virtual DateTime StartDate { get; set; }

		/// <summary>
		/// �������� ���� ������� (�����������, 1899-01-01 ��� �������� 0)
		/// </summary>
		public virtual DateTime EndDate { get; set; }

		/// <summary>
		/// ���������� ������� � �������
		/// </summary>
		public virtual int MonthCount { get; set; }

		/// <summary>
		/// 	��� �������
		/// </summary>
		public string FormulaType { get; set; }

		/// <summary>
		/// 	������� ���������� �������
		/// </summary>
		public virtual bool IsFormula { get; set; }

		/// <summary>
		/// 	������ �������
		/// </summary>
		public virtual string Formula { get; set; }


		/// <summary>
		/// 	��������� ���������� �������������
		/// </summary>
		public virtual string Code { get; set; }

		/// <summary>
		/// 	�����������
		/// </summary>
		public virtual string Comment { get; set; }

		/// <summary>
		/// 	������������� ���������� �������������
		/// </summary>
		public virtual int Id { get; set; }

		/// <summary>
		/// 	An index of object
		/// </summary>
		public virtual int Idx { get; set; }

		/// <summary>
		/// 	��������/���
		/// </summary>
		public virtual string Name { get; set; }

		/// <summary>
		/// 	��������
		/// </summary>
		public virtual DateTime Version { get; set; }
	}
}
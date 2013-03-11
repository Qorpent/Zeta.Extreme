#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : Period.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Zeta.Extreme.Poco.Deprecated;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Model {
	public class period : IPeriod {
		public virtual Guid Uid { get; set; }
		[Map] public virtual string FormulaEvaluator { get; set; }

		public virtual string ParsedFormula { get; set; }

		[Map] public virtual string Tag { get; set; }

		[Map(Title = "������ ��")] public virtual int ClassicId { get; set; }

		[Map(Title = "���������")] public virtual string Category { get; set; }

		[Map] public virtual string ShortName { get; set; }

		[Map(Title = "��� ����")] public virtual bool IsDayPeriod { get; set; }

		[Map(Title = "��������� ����")] public virtual DateTime StartDate { get; set; }

		[Map(Title = "�������� ����")] public virtual DateTime EndDate { get; set; }

		[Map(Title = "����� �������")] public virtual int MonthCount { get; set; }

		/// <summary>
		/// 	��� �������
		/// </summary>
		public string FormulaType { get; set; }

		[Map(Title = "���. ��������")] public virtual bool IsFormula { get; set; }

		[Map(Title = "�������")] public virtual string Formula { get; set; }


		public virtual string Code { get; set; }

		public virtual string Comment { get; set; }

		public virtual int Id { get; set; }

		[Map(Title = "������")] public virtual int Idx { get; set; }

		public virtual string Name { get; set; }

		public virtual DateTime Version { get; set; }
	}
}
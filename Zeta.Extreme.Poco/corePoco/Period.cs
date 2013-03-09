#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : Period.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Comdiv.Model;
using Comdiv.Zeta.Model;

namespace Zeta.Extreme.Poco {
	public class period : IPeriod {
		public virtual Guid Uid { get; set; }

		[Deprecated.Map] public virtual string Tag { get; set; }

		[Deprecated.Map(Title = "������ ��")] public virtual int ClassicId { get; set; }

		[Deprecated.Map(Title = "���������")] public virtual string Category { get; set; }

		[Deprecated.Map] public virtual string ShortName { get; set; }

		[Deprecated.Map(Title = "��� ����")] public virtual bool IsDayPeriod { get; set; }

		[Deprecated.Map(Title = "��������� ����")] public virtual DateTime StartDate { get; set; }

		[Deprecated.Map(Title = "�������� ����")] public virtual DateTime EndDate { get; set; }

		[Deprecated.Map(Title = "����� �������")] public virtual int MonthCount { get; set; }

		[Deprecated.Map(Title = "���. ��������")] public virtual bool IsFormula { get; set; }

		[Deprecated.Map(Title = "�������")] public virtual string Formula { get; set; }
		[Deprecated.Map] public virtual string FormulaEvaluator { get; set; }

		public virtual string ParsedFormula { get; set; }


		public virtual string Code { get; set; }

		public virtual string Comment { get; set; }

		public virtual int Id { get; set; }

		[Deprecated.Map(Title = "������")] public virtual int Idx { get; set; }

		public virtual string Name { get; set; }

		public virtual DateTime Version { get; set; }
	}
}
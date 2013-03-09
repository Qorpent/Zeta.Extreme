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

		[Map] public virtual string Tag { get; set; }

		[Map(Title = "Старый Ид")] public virtual int ClassicId { get; set; }

		[Map(Title = "Категория")] public virtual string Category { get; set; }

		[Map] public virtual string ShortName { get; set; }

		[Map(Title = "Для дней")] public virtual bool IsDayPeriod { get; set; }

		[Map(Title = "Начальная дата")] public virtual DateTime StartDate { get; set; }

		[Map(Title = "Конечная дата")] public virtual DateTime EndDate { get; set; }

		[Map(Title = "Число месяцев")] public virtual int MonthCount { get; set; }

		[Map(Title = "Явл. формулой")] public virtual bool IsFormula { get; set; }

		[Map(Title = "Формула")] public virtual string Formula { get; set; }
		[Map] public virtual string FormulaEvaluator { get; set; }

		public virtual string ParsedFormula { get; set; }


		public virtual string Code { get; set; }

		public virtual string Comment { get; set; }

		public virtual int Id { get; set; }

		[Map(Title = "Индекс")] public virtual int Idx { get; set; }

		public virtual string Name { get; set; }

		public virtual DateTime Version { get; set; }
	}
}
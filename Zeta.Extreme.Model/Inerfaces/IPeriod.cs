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
	/// Интерфейс периода
	/// </summary>
	public interface IPeriod : IEntity, IWithFormula {
		/// <summary>
		/// Бизнес ИД, используемый в приложении
		/// </summary>
		int BizId { get; set; }
		/// <summary>
		/// Категория периода
		/// </summary>
		string Category { get; set; }
		/// <summary>
		/// Сокращенное имя
		/// </summary>
		string ShortName { get; set; }
		/// <summary>
		/// Признак периода ВНУТРИ дня
		/// </summary>
		bool IsDayPeriod { get; set; }
		/// <summary>
		/// Начальная дата периода (приведенная, 1899-01-01 как условный 0)
		/// </summary>
		DateTime StartDate { get; set; }
		/// <summary>
		/// Конечная дата периода (приведенная, 1899-01-01 как условный 0)
		/// </summary>
		DateTime EndDate { get; set; }
		/// <summary>
		/// Количество месяцев в периоде
		/// </summary>
		int MonthCount { get; set; }
	}
}
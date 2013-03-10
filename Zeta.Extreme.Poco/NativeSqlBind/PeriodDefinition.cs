#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : PeriodDefinition.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

#region

using System;
using System.Linq;
using Qorpent;

#endregion

namespace Zeta.Extreme.Poco.NativeSqlBind {
	/// <summary>
	/// 	Определение периода (с учетом года и дат)
	/// </summary>
	public class PeriodDefinition {
		/// <summary>
		/// </summary>
		public PeriodDefinition() {}

		/// <summary>
		/// </summary>
		/// <param name="year"> </param>
		/// <param name="periods"> </param>
		public PeriodDefinition(int year, params int[] periods) {
			Year = year;
			Periods = periods.Where(p => p != -1).ToArray();
		}

		/// <summary>
		/// </summary>
		/// <param name="startDate"> </param>
		public PeriodDefinition(DateTime startDate) {
			StartDate = startDate;
		}

		/// <summary>
		/// </summary>
		/// <param name="startDate"> </param>
		/// <param name="endDate"> </param>
		public PeriodDefinition(DateTime startDate, DateTime endDate) {
			StartDate = startDate;
			EndDate = endDate;
		}

		/// <summary>
		/// 	Периоды
		/// </summary>
		public int[] Periods { get; set; }

		/// <summary>
		/// 	Год
		/// </summary>
		public int Year { get; set; }

		/// <summary>
		/// 	Начальная дата
		/// </summary>
		public DateTime StartDate { get; set; }

		/// <summary>
		/// 	Конечная дата
		/// </summary>
		public DateTime EndDate { get; set; }

		/// <summary>
		/// 	Имя периода
		/// </summary>
		public string PeriodName { get; set; }

		/// <summary>
		/// 	Признак определенности периода
		/// </summary>
		/// <returns> </returns>
		public bool IsPeriodDefined() {
			if (null == Periods) {
				return false;
			}
			if (0 == Periods.Length) {
				return false;
			}
			if (1 == Periods.Length && Periods[0] == 0) {
				return false;
			}
			return true;
		}

		/// <summary>
		/// 	Признак определенности года
		/// </summary>
		/// <returns> </returns>
		public bool IsYearDefined() {
			return 0 != Year;
		}

		/// <summary>
		/// 	Признак определенности начальной даты
		/// </summary>
		/// <returns> </returns>
		public bool IsStartDateDefined() {
			return StartDate > QorpentConst.Date.Begin;
		}

		/// <summary>
		/// 	Признак определенности конечной даты
		/// </summary>
		/// <returns> </returns>
		public bool IsEndDateDefined() {
			return EndDate > QorpentConst.Date.Begin;
		}

		/// <summary>
		/// 	Признак единичного периода
		/// </summary>
		/// <returns> </returns>
		public bool IsOnePeriod() {
			return 1 == Periods.Length;
		}


		/// <summary>
		/// 	Возвращает строку, которая представляет текущий объект.
		/// </summary>
		/// <returns> Строка, представляющая текущий объект. </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString() {
			return string.Format("{0} {1} {2}-{3}", NativeSqlBind.Periods.Get(Periods[0]).Name, Year,
			                     StartDate.ToString("dd.MM.yyyy"), EndDate.ToString("dd.MM.yyyy"));
		}
	}
}
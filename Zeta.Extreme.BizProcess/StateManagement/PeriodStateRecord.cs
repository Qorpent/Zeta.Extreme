#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : PeriodStateRecord.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Comdiv.Extensions;

namespace Zeta.Extreme.BizProcess.StateManagement {
	/// <summary>
	/// 	Запись статуса периода
	/// </summary>
	public sealed class PeriodStateRecord {
		///<summary>
		///	Создает стандартную запись
		///</summary>
		public PeriodStateRecord() {
			DeadLine = DateExtensions.Begin;
		}

		/// <summary>
		/// 	Дедлайн
		/// </summary>
		public DateTime DeadLine;

		/// <summary>
		/// 	Период
		/// </summary>
		public int Period;

		/// <summary>
		/// 	Статус
		/// </summary>
		public bool State;

		/// <summary>
		/// 	Общий дедлайн
		/// </summary>
		public DateTime UDeadLine;

		/// <summary>
		/// 	Год
		/// </summary>
		public int Year;
	}
}
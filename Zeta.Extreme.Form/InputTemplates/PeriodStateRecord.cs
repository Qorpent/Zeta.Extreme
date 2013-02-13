using System;
using Comdiv.Extensions;

namespace Comdiv.Zeta.Web.InputTemplates {
	/// <summary>
	/// Запись статуса периода
	/// </summary>
	public sealed class PeriodStateRecord
	{
		/// <summary>
		///Создает стандартную запись
		/// </summary>
		public PeriodStateRecord()
		{
			DeadLine = DateExtensions.Begin;
		}

		/// <summary>
		/// Год
		/// </summary>
		public int Year;
		/// <summary>
		/// Период
		/// </summary>
		public int Period;
		/// <summary>
		/// Статус
		/// </summary>
		public bool State;
		/// <summary>
		/// Дедлайн
		/// </summary>
		public DateTime DeadLine;
		/// <summary>
		/// Общий дедлайн
		/// </summary>
		public DateTime UDeadLine;
	}
}
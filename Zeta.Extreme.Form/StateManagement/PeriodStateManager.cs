#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : PeriodStateManager.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using System.Data;
using Comdiv.Application;
using Comdiv.Extensions;
using Comdiv.Persistence;
using Qorpent.Applications;
using Zeta.Extreme.BizProcess.StateManagement;

namespace Zeta.Extreme.Form.StateManagement {
	/// <summary>
	/// 	Простой менеджер статусов периодов
	/// </summary>
	public class PeriodStateManager : IPeriodStateManager {
		/// <summary>
		/// 	Система
		/// </summary>
		public string System { get; set; }

		/// <summary>
		/// 	БД
		/// </summary>
		public string Database { get; set; }

		/// <summary>
		/// 	Получить все записи
		/// </summary>
		/// <param name="year"> </param>
		/// <returns> </returns>
		public PeriodStateRecord[] All(int year) {
			PeriodStateRecord[] result = null;
			indatabase(c => result = c.ExecuteOrm<PeriodStateRecord>("select * from usm.periodstate where year = " + year,
			                                                         (IParametersProvider) null));
			return result;
		}

		/// <summary>
		/// 	Получить запись по году и периоду
		/// </summary>
		/// <param name="year"> </param>
		/// <param name="period"> </param>
		/// <returns> </returns>
		public PeriodStateRecord Get(int year, int period) {
			var result = new PeriodStateRecord();
			result.Year = year;
			result.Period = period;
			IDictionary<string, object> dict = null;
			indatabase(
				c =>
				dict =
				c.ExecuteDictionary(
					"select state,deadline,udeadline from usm.periodstate where year=" + year + " and period=" + period, null));
			result.State = dict.get("state", () => result.State);
			result.DeadLine = dict.get("deadline", () => result.DeadLine);
			result.UDeadLine = dict.get("udeadline", () => result.UDeadLine);
			return result;
		}

		/// <summary>
		/// 	Обновить статус
		/// </summary>
		/// <param name="record"> </param>
		public void UpdateState(PeriodStateRecord record) {
			indatabase(
				c =>
				c.ExecuteNonQuery(
					"exec usm.set_period_state @year=" + record.Year + ",@period=" + record.Period + ",@state=" +
					(record.State ? 1 : 0), (IParametersProvider) null));
		}

		/// <summary>
		/// 	Обновить дедлайн
		/// </summary>
		/// <param name="record"> </param>
		public void UpdateDeadline(PeriodStateRecord record) {
			indatabase(
				c =>
				c.ExecuteNonQuery(
					@"exec usm.set_period_deadline @year=" + record.Year + ",@period=" + record.Period + ",@deadline=@date",
					new Dictionary<string, object> {{"@date", record.DeadLine}}
					));
		}

		/// <summary>
		/// 	Обновить дедлайн по подписанию
		/// </summary>
		/// <param name="record"> </param>
		public void UpdateUDeadline(PeriodStateRecord record) {
			indatabase(
				c =>
				c.ExecuteNonQuery(
					@"exec usm.set_period_udeadline @year=" + record.Year + ",@period=" + record.Period + ",@deadline=@date",
					new Dictionary<string, object> {{"@date", record.UDeadLine}}
					));
		}

		private static IDbConnection GetConnection(string system) {
			system = string.IsNullOrWhiteSpace(system) ? "Default" : system;
			return Application.Current.DatabaseConnections.GetConnection(system);
		}

		private void indatabase(Action<IDbConnection> action) {
			using (var c = GetConnection(System)) {
				c.WellOpen();
				if (Database.hasContent()) {
					c.ChangeDatabase(Database);
				}
				action(c);
			}
		}
	}
}
#region LICENSE
// Copyright 2007-2013 Qorpent Team - http://github.com/Qorpent
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// PROJECT ORIGIN: Zeta.Extreme.Form/PeriodStateManager.cs
#endregion
using System;
using System.Collections.Generic;
using System.Data;

using Qorpent.Applications;
using Qorpent.Data;
using Qorpent.Utils.Extensions;
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
		/// <param name="grp"></param>
		/// <returns> </returns>
		public PeriodStateRecord Get(int year, int period, string grp ) {
			grp = grp ?? "";
			var result = new PeriodStateRecord();
			if (!string.IsNullOrWhiteSpace(grp)) {
				result = Get(year, period, "");
			}
			result.Year = year;
			result.Period = period;
			result.Grp = grp;
			IDictionary<string, object> dict = null;
			indatabase(
				c =>
				dict =
				c.ExecuteDictionary(
					"select state,deadline,udeadline from usm.periodstate where year=" + year + " and period=" + period+" and grp='"+grp+"'", null));
			

			result.State = dict.SafeGet("state",result.State);
			result.DeadLine = dict.SafeGet("deadline", result.DeadLine);
			result.UDeadLine = dict.SafeGet("udeadline", result.UDeadLine);
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
					"UNICALL usm.set_period_state | year=~,period=~,state=~",
					new {year=record.Year, period=record.Period,state=record.State?1:0}
					));
		}



		/// <summary>
		/// 	Обновить дедлайн
		/// </summary>
		/// <param name="record"> </param>
		public void UpdateDeadline(PeriodStateRecord record) {
			indatabase(
				c =>
				c.ExecuteNonQuery(
					@"UNICALL usm.set_period_deadline | year=~ , period=~, deadline=~",
					new {year = record.Year,period =record.Period,deadline = record.DeadLine}
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
				if (Database.IsNotEmpty()) {
					c.ChangeDatabase(Database);
				}
				action(c);
			}
		}
	}
}
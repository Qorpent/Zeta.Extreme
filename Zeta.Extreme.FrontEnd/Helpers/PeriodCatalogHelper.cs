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
// PROJECT ORIGIN: Zeta.Extreme.FrontEnd/PeriodCatalogHelper.cs
#endregion
using System.Linq;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.MetaCaches;

namespace Zeta.Extreme.FrontEnd.Helpers {
	/// <summary>
	/// 	Вспомогательный класс для работы с периодами
	/// </summary>
	public class PeriodCatalogHelper {
		/// <summary>
		/// 	Возвращает каталог периодов
		/// </summary>
		/// <returns> </returns>
		public PeriodRecord[] GetAllPeriods() {
			return
				(Periods.All.OfType<Period>().Where(_ => _.BizId > 0).ToArray())
					.Select(_ => new PeriodRecord {id = _.BizId, name = _.Name, type = GetPeriodType(_), idx = GetIdx(_)})
					.Where(_ => PeriodType.None != _.type)
					.OrderBy(_ => _.type)
					.ThenBy(_ => _.idx)
					.ToArray();
		}

		/// <summary>
		/// Возвращает сгруппированный каталог периодов
		/// </summary>
		/// <returns></returns>
		public PeriodTypeGroup[] GetPeriodGroups() {
			return GetAllPeriods().GroupBy(_ => _.type, _ => _)
				.Select(_ => new PeriodTypeGroup {type = _.Key, periods = _.ToArray()})
				.ToArray();
		}
		

		/// <summary>
		/// 	Возвращает тип периода по записи о нем
		/// </summary>
		/// <param name="p"> </param>
		/// <returns> </returns>
		public PeriodType GetPeriodType(Period p) {
			var id = p.BizId;
			if ((id >= 11 && id <= 19) || (id == 110 || id == 111 || id == 112)) {
				return PeriodType.Month;
			}
			if (id >= 1 && id <= 4) {
				return PeriodType.FromYearStartMain;
			}
			if (id >= 41 && id <= 49) {
				return PeriodType.InYear;
			}
			if (id >= 31 && id <= 34) {
				return PeriodType.Corrective;
			}
			if ((id >= 401 && id <= 419) || (id==444) || (id == 4110 || id == 4111 || id == 4112)) {
				return PeriodType.Awaited;
			}
			if ((id >= 22 && id <= 29) || (id == 210 || id == 211 || id == 212)) {
				return PeriodType.FromYearStartExt;
			}
			if ((id >= 251 && id <= 254) || (id >= 301 && id <= 309) || (id == 3512) || (id >= 641 && id <= 649)) {
				return PeriodType.Plan;
			}
			if ((id >= 611 && id <= 619) || (id == 6110 || id == 6111 || id == 6112)) {
				return PeriodType.MonthPlan;
			}
			return PeriodType.None;
		}

		/// <summary>
		/// 	Возвращает индекс периода по его типу и записи о нем
		/// </summary>
		/// <returns> </returns>
		public int GetIdx(Period p) {
			return GetIdx(GetPeriodType(p), p);
		}

		private int GetIdx(PeriodType type, Period period) {
			switch (type) {
				case PeriodType.None:
					return 0;
				case PeriodType.Awaited:
					switch (period.BizId) {
						case 403:
							return 10;
						case 406:
							return 20;
						case 409:
							return 30;
						case 401:
							return 40;
						default:
							return period.BizId;
					}
				case PeriodType.Plan:
					switch (period.BizId) {
						case 301:
							return 10;
						case 3512:
							return 20;
						default:
							return period.BizId;
					}
				default:
					return period.BizId;
			}
		}
	}
}
#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : PeriodCatalogHelper.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Linq;
using Comdiv.Application;
using Comdiv.Zeta.Model;
using Zeta.Extreme.Meta;
using Zeta.Extreme.Poco;

namespace Zeta.Extreme.FrontEnd.Helpers {
	/// <summary>
	/// 	Вспомогательный класс для работы с периодами
	/// </summary>
	public class PeriodCatalogHelper {

		/// <summary>
		/// Возвращает каталог периодов
		/// </summary>
		/// <returns></returns>
		public PeriodRecord[] GetAllPeriods() {
			return
				(Periods.All.OfType<period>().Where(_ => _.ClassicId > 0).ToArray())
					.Select(_ => new PeriodRecord { id = _.ClassicId, name = _.Name, type = GetPeriodType(_), idx = GetIdx(_) })
					.Where(_ => PeriodType.None != _.type)
					.OrderBy(_ => _.type)
					.ThenBy(_ => _.idx)
					.ToArray();

		}

		/// <summary>
		/// 	Возвращает тип периода по записи о нем
		/// </summary>
		/// <param name="p"> </param>
		/// <returns> </returns>
		public PeriodType GetPeriodType(period p) {
			var id = p.ClassicId;
			if((id>=11 && id<=19)||(id==110||id==111||id==112))return PeriodType.Month;
			if(id>=1 && id<=4)return PeriodType.FromYearStartMain;
			if (id >= 41 && id <= 49) return PeriodType.InYear;
			if (id >= 31 && id <= 34) return PeriodType.Corrective;
			if ((id >= 401 && id <= 419) || (id == 4110 || id == 4111 || id == 4112)) return PeriodType.Awaited;
			if((id>=22 && id<=29)||(id==210||id==211||id==212))return PeriodType.FromYearStartExt;
			if((id>=251 && id<=254)||(id>=301 && id<=309)||(id==3512)||(id>=641 && id<=649))return PeriodType.Plan;
			if((id>=611 && id<=619)||(id==6110||id==6111||id==6112))return PeriodType.MonthPlan;
			return PeriodType.None;
		}

		/// <summary>
		/// 	Возвращает индекс периода по его типу и записи о нем
		/// </summary>
		/// <returns> </returns>
		public int GetIdx(period p) {
			return GetIdx(GetPeriodType(p), p);
		}

		private int GetIdx(PeriodType type, period period) {
			switch (type) {
				case PeriodType.None:
					return 0;
				case PeriodType.Awaited:
					switch (period.ClassicId) {
						case 403 :
							return 10;
						case 406 :
							return 20;
						case 409 :
							return 30;
						case 401:
							return 40;
						default:
							return period.ClassicId;
					}
				case PeriodType.Plan:
					switch (period.ClassicId) {
						case 301 :
							return 10;
						case 3512:
							return 20;
						default:
							return period.ClassicId;
					}
				default:
					return period.ClassicId;

			}
		}
	}
}
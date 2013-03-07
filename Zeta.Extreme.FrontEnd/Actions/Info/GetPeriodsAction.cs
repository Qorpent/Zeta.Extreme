using System.Linq;
using Comdiv.Application;
using Comdiv.Zeta.Model;
using Qorpent.Mvc;
using Zeta.Extreme.FrontEnd.Helpers;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// ¬озвращает реестр используемых периодов
	/// </summary>
	[Action("zeta.getperiods")]
	public class GetPeriodsAction : FormServerActionBase
	{
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			MyFormServer.MetaCacheLoad.Wait();
			
			return new PeriodCatalogHelper().GetAllPeriods();
		}
	}
}
using System.Linq;
using Comdiv.Application;
using Comdiv.Zeta.Model;
using Qorpent.Mvc;
using Zeta.Extreme.FrontEnd.Helpers;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// ¬озвращает реестр используемых периодов
	/// </summary>
	[Action("zefs.getperiods")]
	public class GetPeriodsAction : FormServerActionBase
	{
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			MyFormServer.HibernateLoad.Wait();
			var helper = new PeriodCatalogHelper();
			return
				(myapp.storage.AsQueryable<period>().Where(_ => _.ClassicId > 0).ToArray())
					.Select(_ => new {id = _.ClassicId, name = _.Name, type = helper.GetPeriodType(_), idx = helper.GetIdx(_)})
					.Where(_ => PeriodType.None != _.type)
					.OrderBy(_=>_.type)
					.ThenBy(_=>_.idx)
					.ToArray();
		}
	}
}
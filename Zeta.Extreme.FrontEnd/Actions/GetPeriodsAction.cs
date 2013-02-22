using System.Linq;
using Comdiv.Application;
using Comdiv.Zeta.Model;
using Qorpent.Mvc;
using Zeta.Extreme.FrontEnd.Helpers;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// ���������� ������ ������������ ��������
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
			return new PeriodCatalogHelper().GetAllPeriods();
		}
	}
}
#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : GetPeriodsAction.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Mvc;
using Zeta.Extreme.FrontEnd.Helpers;

namespace Zeta.Extreme.FrontEnd.Actions.Info {
	/// <summary>
	/// 	¬озвращает реестр используемых периодов
	/// </summary>
	[Action("zeta.getperiods")]
	public class GetPeriodsAction : FormServerActionBase {
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
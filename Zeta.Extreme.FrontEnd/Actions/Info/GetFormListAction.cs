#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : GetFormListAction.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd.Actions.Info {
	/// <summary>
	/// 	Выдает список загруженных форм
	/// </summary>
	[Action("zefs.formlist")]
	public class GetFormListAction : FormServerActionBase {
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			return MyFormServer.GetFormList();
		}
	}
}
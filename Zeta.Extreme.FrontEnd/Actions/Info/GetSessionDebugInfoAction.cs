#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : GetSessionDebugInfoAction.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd.Actions.Info {
	/// <summary>
	/// 	Возвращает информацию о сессии
	/// </summary>
	[Action("zefs.debuginfo")]
	public class GetSessionDebugInfoAction : FormSessionActionBase {
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			return MySession.CollectDebugInfo();
		}
	}
}
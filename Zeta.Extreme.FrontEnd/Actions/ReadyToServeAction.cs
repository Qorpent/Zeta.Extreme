#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : ReadyToServeAction.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd.Actions {
	/// <summary>
	/// 	Выдает список загруженных форм
	/// </summary>
	[Action("zefs.ready")]
	public class ReadyToServeAction : ActionBase {
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			return FormServer.Default.IsOk;
		}
	}
}
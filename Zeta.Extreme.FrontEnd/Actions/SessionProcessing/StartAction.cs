#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : StartAction.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd.Actions.SessionProcessing {
	/// <summary>
	/// 	Инициирует сессию
	/// </summary>
	[Action("zefs.start")]
	public class StartAction : SessionStartBase {
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			return MyFormServer.Start(_realform, _realobj, year, period);
		}
	}
}
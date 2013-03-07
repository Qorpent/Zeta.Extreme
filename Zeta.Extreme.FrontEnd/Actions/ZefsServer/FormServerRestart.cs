#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : FormServerRestart.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd.Actions.ZefsServer {
	/// <summary>
	/// 	Действие, возвращающее статус загрузки приложения
	/// </summary>
	[Action("zefs.restart")]
	public class FormServerRestart : FormServerActionBase {
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			MyFormServer.Reload();
			return true;
		}
	}
}
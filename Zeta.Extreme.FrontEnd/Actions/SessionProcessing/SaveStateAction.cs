#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : SaveStateAction.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd.Actions.SessionProcessing {
	/// <summary>
	/// 	¬озвращает текущий статус сохранени€
	/// </summary>
	[Action("zefs.savestate")]
	public class SaveStateAction : FormSessionActionBase {
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			return MySession.GetSaveState();
		}
	}
}
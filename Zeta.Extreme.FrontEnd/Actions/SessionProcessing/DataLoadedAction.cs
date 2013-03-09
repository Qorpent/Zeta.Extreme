#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : DataLoadedAction.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd.Actions.SessionProcessing {
	/// <summary>
	/// 	Вызывает процедуру очистки сессии после загрузки
	/// </summary>
	[Action("zefs.dataloaded")]
	public class DataLoadedAction : FormSessionActionBase {
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			return MySession.CleanupAfterDataLoaded();
		}
	}
}
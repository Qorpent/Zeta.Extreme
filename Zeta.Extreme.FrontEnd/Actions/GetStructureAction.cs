#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : GetStructureAction.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd.Actions {
	/// <summary>
	/// 	Инициирует сессию
	/// </summary>
	[Action("zefs.struct")]
	public class GetStructureAction : SessionAttachedActionBase {
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			_session.PrepareStructureTask.Wait();
			return _session.Structure;
		}
	}
}
#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : CurrentLockStateAction.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd.Actions {
	/// <summary>
	/// 	Возвращает статус формы
	/// </summary>
	[Action("zefs.currentlockstate")]
	public class CurrentLockStateAction : SessionAttachedActionBase {
		/// <summary>
		/// 	Возвращает статус формы по блокировке
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			var isopen = _session.Template.IsOpen;
			var state = _session.Template.GetState(_session.Object, null);
			var editable = isopen && state == "0ISOPEN";
			return new
				{
					isopen,
					state,
					editable,
				};
		}
	}
}
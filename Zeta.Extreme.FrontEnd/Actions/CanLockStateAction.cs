#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : CanLockStateAction.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd.Actions {
	/// <summary>
	/// 	Возвращает статус формы
	/// </summary>
	[Action("zefs.canlockstate")]
	public class CanLockStateAction : SessionAttachedActionBase {
		/// <summary>
		/// 	Возвращает статус формы по блокировке
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			var isopen = _session.Template.IsOpen;
			var state = _session.Template.GetState(_session.Object, null);
			var editable = isopen && state == "0ISOPEN";
			var message = _session.Template.CanSetState(_session.Object, null, "0ISBLOCK");
			var canblock = state == "0ISOPEN" && string.IsNullOrWhiteSpace(message);
			return new
				{
					isopen,
					state,
					editable,
					canblock,
					message
				};
		}
	}
}
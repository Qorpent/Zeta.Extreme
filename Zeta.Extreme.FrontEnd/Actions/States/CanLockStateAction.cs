#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : CanLockStateAction.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd.Actions.States {
	/// <summary>
	/// 	Возвращает статус формы
	/// </summary>
	[Action("zefs.canlockstate")]
	public class CanLockStateAction : FormSessionActionBase {
		/// <summary>
		/// 	Возвращает статус формы по блокировке
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			return MySession.GetCanBlockInfo();
		}
	}
}
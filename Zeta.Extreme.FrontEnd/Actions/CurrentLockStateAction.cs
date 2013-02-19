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
	/// 	���������� ������ �����
	/// </summary>
	[Action("zefs.currentlockstate")]
	public class CurrentLockStateAction : FormSessionActionBase {
		/// <summary>
		/// 	���������� ������ ����� �� ����������
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			return MySession.GetCurrentLockInfo();
		}
	}
}
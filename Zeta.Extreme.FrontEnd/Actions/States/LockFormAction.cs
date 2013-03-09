#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : LockFormAction.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd.Actions.States {
	/// <summary>
	/// 	���������� ������� ������ ����������
	/// </summary>
	[Action("zefs.lockform")]
	public class LockFormAction : FormSessionActionBase {
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			return MySession.DoLockForm();
		}
	}
}
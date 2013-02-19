#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : DebugInfoAction.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Linq;
using Qorpent.Mvc;
using Qorpent.Utils.Extensions;

namespace Zeta.Extreme.FrontEnd.Actions {
	/// <summary>
	/// 	���������� ���������� � ������
	/// </summary>
	[Action("zefs.debuginfo")]
	public class DebugInfoAction : FormSessionActionBase {
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			return MySession.CollectDebugInfo();
		}
	}
}
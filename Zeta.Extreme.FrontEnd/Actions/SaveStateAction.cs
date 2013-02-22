#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : SaveStateAction.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Zeta.Data.Minimal;
using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// 	���������� ������� ������ ����������
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
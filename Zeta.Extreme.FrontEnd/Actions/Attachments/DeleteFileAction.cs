#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : DeleteFileAction.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd.Actions.Attachments {
	///<summary>
	///	Вызывает сохранение данных
	///</summary>
	[Action("zefs.deletefile", Role = "ADMIN")]
	public class DeleteFileAction : SingleAttachmentActionBase {
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			MySession.DeleteAttach(uid);
			return true;
		}
	}
}
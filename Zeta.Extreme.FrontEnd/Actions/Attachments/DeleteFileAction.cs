using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd.Actions.Attachments {
	///<summary>
	///	Вызывает сохранение данных
	///</summary>
	[Action("zefs.deletefile",Role = "ADMIN")]
	public class DeleteFileAction : SingleAttachmentActionBase
	{
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
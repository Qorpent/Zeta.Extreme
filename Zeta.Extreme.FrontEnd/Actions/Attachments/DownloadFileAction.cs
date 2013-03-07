using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd.Actions.Attachments {
	///<summary>
	///	Вызывает сохранение данных
	///</summary>
	[Action("zefs.downloadfile", Role = "ADMIN")]
	public class DownloadFileAction : SingleAttachmentActionBase
	{
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess()
		{
			MySession.GetDownloadAbleFileDescriptor(uid);
			return true;
		}
	}
}
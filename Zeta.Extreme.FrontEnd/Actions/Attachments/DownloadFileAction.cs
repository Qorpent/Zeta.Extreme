using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd.Actions.Attachments {
	///<summary>
	///	�������� ���������� ������
	///</summary>
	[Action("zefs.downloadfile")]
	public class DownloadFileAction : SingleAttachmentActionBase
	{
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess()
		{
			return MySession.GetDownloadAbleFileDescriptor(uid);
		}
	}
}
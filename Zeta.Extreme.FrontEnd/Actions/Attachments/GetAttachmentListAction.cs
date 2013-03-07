using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd.Actions.Attachments {
	///<summary>
	///	Вызывает сохранение данных
	///</summary>
	[Action("zefs.attachlist")]
	public class GetAttachmentListAction : FormSessionActionBase
	{
		
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			return MySession.GetAttachedFiles();
		}
		
	}
}
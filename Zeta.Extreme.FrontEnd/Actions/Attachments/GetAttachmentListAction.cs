using Qorpent.Mvc;
using Qorpent.Mvc.Binding;

namespace Zeta.Extreme.FrontEnd.Actions.Attachments {
	///<summary>
	///	�������� ���������� ������
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
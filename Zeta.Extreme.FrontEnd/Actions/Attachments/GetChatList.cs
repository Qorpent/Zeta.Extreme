using System.Linq;
using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd.Actions.Attachments {
	/// <summary>
	/// 
	/// </summary>
	[Action("zefs.chatlist")]
	public class GetChatList : FormSessionActionBase
	{
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess()
		{
			return MySession.GetChatList().OrderByDescending(_=>_.Time);
		}
	}
}
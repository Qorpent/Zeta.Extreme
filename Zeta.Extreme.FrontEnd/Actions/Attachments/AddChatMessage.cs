using Qorpent.Mvc;
using Qorpent.Mvc.Binding;

namespace Zeta.Extreme.FrontEnd.Actions.Attachments {
	/// <summary>
	/// 
	/// </summary>
	[Action("zefs.chatadd")]
	public class AddChatMessage:FormSessionActionBase {
		/// <summary>
		/// Текст сообщения
		/// </summary>
		[Bind]
		public string Text { get; set; }

		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			return MySession.AddChatMessage(Text);
		}
	}
}
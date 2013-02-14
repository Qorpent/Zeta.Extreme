using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// Выдает список загруженных форм
	/// </summary>
	[Action("exf.ready")]
	public class ReadyToServeAction : ActionBase
	{
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			return FormServer.Default.IsOk;
		}
	}
}
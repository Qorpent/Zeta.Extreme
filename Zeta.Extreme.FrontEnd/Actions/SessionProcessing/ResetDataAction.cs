using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd.Actions.SessionProcessing {
	/// <summary>
	/// 	Инициирует сессию
	/// </summary>
	[Action("zefs.resetdata")]
	public class ResetDataAction : FormSessionActionBase
	{
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			return MySession.RestartData();
		}

	}
}
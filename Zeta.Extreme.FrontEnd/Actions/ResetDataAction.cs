using Qorpent.Mvc;
using Qorpent.Mvc.Binding;

namespace Zeta.Extreme.FrontEnd {
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
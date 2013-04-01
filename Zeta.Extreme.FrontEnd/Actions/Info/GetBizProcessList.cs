using Qorpent.Mvc;
using Zeta.Extreme.FrontEnd.Helpers;

namespace Zeta.Extreme.FrontEnd.Actions.Info {
	/// <summary>
	/// ¬ыводит полный список доступных тем
	/// </summary>
	[Action("zefs.bizprocesslist")]
	public class  GetBizProcessList : FormServerActionBase {
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			return new BizProcessEnumerator().GetAllBizProcesses(Context.User);
		}
	}
}
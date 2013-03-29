using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd.Actions.States {
	/// <summary>
	/// 	Возвращает статус формы
	/// </summary>
	[Action("zefs.canlockstate")]
	public class CanLockStateAction : FormSessionActionBase {
		/// <summary>
		/// 	Возвращает статус формы по блокировке
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			return MySession.GetStateInfo();
		}
	}
}
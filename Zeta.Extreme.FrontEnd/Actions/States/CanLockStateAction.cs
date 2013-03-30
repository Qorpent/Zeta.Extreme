using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd.Actions.States {
	/// <summary>
	/// 	���������� ������ �����
	/// </summary>
	[Action("zefs.canlockstate")]
	public class CanLockStateAction : FormSessionActionBase {
		/// <summary>
		/// 	���������� ������ ����� �� ����������
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			return MySession.GetStateInfo();
		}
	}
}
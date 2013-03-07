using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd.Actions.States {
	/// <summary>
	/// 	¬озвращает текущий статус сохранени€
	/// </summary>
	[Action("zefs.lockform")]
	public class LockFormAction : FormSessionActionBase
	{
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess()
		{
			return MySession.DoLockForm();
		}
	}
}
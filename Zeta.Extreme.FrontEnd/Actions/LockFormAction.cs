using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// 	Возвращает текущий статус сохранения
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


	/// <summary>
	/// Возвращает историю блокировок
	/// </summary>
	[Action("zefs.locklist")]
	public class LockListAction : FormSessionActionBase
	{
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess()
		{
			return MySession.GetLockHistory();
		}
	}
}
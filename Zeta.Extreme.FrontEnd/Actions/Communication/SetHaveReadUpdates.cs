using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd.Actions.Communication {
	/// <summary>
	///     Действие, выставляющее глобальный признак просмотра обновлений
	/// </summary>
	[Action("zecl.haveread", Role = "BUDGET,CURATOR")]
	public class SetHaveReadUpdates : ChatProviderActionBase
	{
		/// <summary>
		///     processing of execution - main method of action
		/// </summary>
		/// <returns>
		/// </returns>
		protected override object MainProcess()
		{
			_provider.SetHaveRead(Context.User.Identity.Name);
			return true;
		}
	}
}
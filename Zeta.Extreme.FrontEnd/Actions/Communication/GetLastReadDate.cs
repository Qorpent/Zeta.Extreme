using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd.Actions.Communication {
	/// <summary>
	///     Действие, выставляющее глобальный признак просмотра обновлений
	/// </summary>
	[Action("zecl.getlastread", Role = "DEFAULT")]
	public class GetLastReadDate : ChatProviderActionBase
	{
		
		/// <summary>
		///     processing of execution - main method of action
		/// </summary>
		/// <returns>
		/// </returns>
		protected override object MainProcess()
		{
			return _provider.GetLastRead(Context.User.Identity.Name);
		}
	}
}
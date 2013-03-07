using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd.Actions.SessionProcessing {
	/// <summary>
	/// Вызывает процедуру очистки сессии после загрузки
	/// </summary>
	[Action("zefs.dataloaded")]
	public class DataLoadedAction : FormSessionActionBase
	{
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess()
		{
			return MySession.CleanupAfterDataLoaded();
		}
	}
}
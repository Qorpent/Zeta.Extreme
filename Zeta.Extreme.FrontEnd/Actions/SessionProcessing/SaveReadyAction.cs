using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd.Actions.SessionProcessing {
	/// <summary>
	/// 	Инициирует сессию
	/// </summary>
	[Action("zefs.saveready")]
	public class SaveReadyAction : SessionStartBase
	{
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess()
		{
			var session =  MyFormServer.Start(_realform, _realobj, year, period,true);
			session.WaitData();
			return session;
		}
	}
}
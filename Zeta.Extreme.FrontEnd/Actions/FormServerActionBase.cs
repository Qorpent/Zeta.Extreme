using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd.Actions {
	/// <summary>
	/// Базовое действие сервера форм
	/// </summary>
	public abstract class FormServerActionBase : ActionBase {
		/// <summary>
		/// Ссылка на сервер форм
		/// </summary>
		protected FormServer MyFormServer;

		/// <summary>
		/// 	First phase of execution - override if need special input parameter's processing
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();
			this.MyFormServer = FormServer.Default;
		}
	}
}
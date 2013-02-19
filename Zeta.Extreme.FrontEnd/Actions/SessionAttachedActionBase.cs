using System;
using System.Linq;
using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.FrontEnd.Session;

namespace Zeta.Extreme.FrontEnd.Actions {
	/// <summary>
	/// Базовое действие, работающее с сессией
	/// </summary>
	public abstract class SessionAttachedActionBase : ActionBase {
		/// <summary>
		/// Ссылка на текущую сессию
		/// </summary>
		protected FormSession _session;
		/// <summary>
		/// Параметр кода сессии
		/// </summary>
		[Bind(Required = true)] public string Session = "";

		/// <summary>
		/// 	Second phase - validate INPUT/REQUEST parameters here - it called before PREPARE so do not try validate
		/// 	second-level internal state and authorization - only INPUT PARAMETERS must be validated
		/// </summary>
		protected override void Validate() {
			FormServer.Default.ReadyToServeForms.Wait();
			if (!FormServer.Default.IsOk) {
				throw new Exception("Application not loaded properly!");
			}
			base.Validate();
			_session = FormServer.Default.Sessions.First(_ => _.Uid == Session);
		}
	}
}
#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : GetStructureAction.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Linq;
using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.FrontEnd.Session;

namespace Zeta.Extreme.FrontEnd.Actions {
	/// <summary>
	/// 	Инициирует сессию
	/// </summary>
	[Action("zefs.struct")]
	public class GetStructureAction : ActionBase {
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
			_session = FormServer.Default.Sessions.First(_ => _.Uid == session);
		}


		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			_session.PrepareStructureTask.Wait();
			return _session.Structure;
		}

		private FormSession _session;
		[Bind(Required = true)] private string session = "";
	}
}
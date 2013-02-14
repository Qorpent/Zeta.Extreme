using System;
using System.Linq;
using Qorpent.Mvc;
using Qorpent.Mvc.Binding;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// ���������� ���������� � ������
	/// </summary>
	[Action("exf.sessionstatus")]
	public class SessionStatusAction : ActionBase
	{
		[Bind(Required = true)]
		private string session = "";



		private FormSession _session;

		/// <summary>
		/// 	Second phase - validate INPUT/REQUEST parameters here - it called before PREPARE so do not try validate
		/// 	second-level internal state and authorization - only INPUT PARAMETERS must be validated
		/// </summary>
		protected override void Validate()
		{
			FormServer.Default.ReadyToServeForms.Wait();
			if (!FormServer.Default.IsOk) throw new Exception("Application not loaded properly!");
			base.Validate();
			_session = FormServer.Default.Sessions.First(_ => _.Uid == session);

		}


		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			return _session;
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using Qorpent.Mvc;
using Qorpent.Mvc.Binding;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// Инициирует сессию
	/// </summary>
	[Action("exf.getdata")]
	public class GetDataAction : ActionBase
	{
		[Bind(Required = true)]
		private string session = "";
		[Bind(Required = false)]
		private int startidx = 0;
	

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
		protected override object MainProcess()
		{
			lock(_session.Data) {
				var state = _session.IsFinished ? "f" : "w";
				if (_session.Data.Count <= startidx) {
					return new {state};
				}
				var max = _session.Data.Count - 1;
				var cnt = max - startidx + 1;
				return new {si = startidx, ei = max, state, data = _session.Data.Skip(startidx).Take(cnt).ToArray()};
			}
		}
	}
}
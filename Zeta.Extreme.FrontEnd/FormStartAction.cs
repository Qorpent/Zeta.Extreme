using System;
using Comdiv.Zeta.Model;
using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.Form.InputTemplates;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// Инициирует сессию
	/// </summary>
	[Action("exf.start")]
	public class FormStartAction : ActionBase {
		[Bind(Required = true)]private int year=0;
		[Bind(Required = true)]private int period=0;
		[Bind(Required = true)]private int obj=0;
		[Bind(Required = true)]private string form="";
		private IInputTemplate _realform;
		private IZetaMainObject _realobj;

		/// <summary>
		/// 	Second phase - validate INPUT/REQUEST parameters here - it called before PREPARE so do not try validate
		/// 	second-level internal state and authorization - only INPUT PARAMETERS must be validated
		/// </summary>
		protected override void Validate()
		{
			FormServer.Default.ReadyToServeForms.Wait();
			if (!FormServer.Default.IsOk) throw new Exception("Application not loaded properly!");
			base.Validate();
			_realform = FormServer.Default.FormProvider.Get(form);
			if(null==_realform)throw new Exception("form not found");
			_realobj = MetaCache.Default.Get<IZetaMainObject>(obj);
			if (null == _realobj) throw new Exception("obj not found");
		}

		
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			var session = new FormSession(_realform, year, period, _realobj);
			FormServer.Default.Sessions.Add(session);
			session.Start();
			return session;
		}
	}


}
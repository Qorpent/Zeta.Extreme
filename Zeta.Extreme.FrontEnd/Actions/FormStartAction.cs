#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : FormStartAction.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Comdiv.Zeta.Model;
using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.Form.InputTemplates;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// 	���������� ������
	/// </summary>
	[Action("zefs.start")]
	public class FormStartAction : FormServerActionBase {
		/// <summary>
		/// 	Second phase - validate INPUT/REQUEST parameters here - it called before PREPARE so do not try validate
		/// 	second-level internal state and authorization - only INPUT PARAMETERS must be validated
		/// </summary>
		protected override void Validate() {
			MyFormServer.ReadyToServeForms.Wait();
			if (!FormServer.Default.IsOk) {
				throw new Exception("Application not loaded properly!");
			}
			base.Validate();
			_realform = FormServer.Default.FormProvider.Get(form);
			if (null == _realform) {
				throw new Exception("form not found");
			}
			_realobj = MetaCache.Default.Get<IZetaMainObject>(obj);
			if (null == _realobj) {
				throw new Exception("obj not found");
			}
		}


		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			return MyFormServer.Start(_realform, _realobj, year, period);
		}

		private IInputTemplate _realform;
		private IZetaMainObject _realobj;
		[Bind(Required = true)] private string form = "";
		[Bind(Required = true)] private int obj = 0;
		[Bind(Required = true)] private int period = 0;
		[Bind(Required = true)] private int year = 0;
	}
}
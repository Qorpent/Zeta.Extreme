#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : FormStartAction.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Linq;
using System.Security;
using Comdiv.Zeta.Model;
using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.Form.InputTemplates;
using Zeta.Extreme.FrontEnd.Helpers;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// 	Инициирует сессию
	/// </summary>
	[Action("zefs.start")]
	public class FormStartAction : SessionStartBase {
		
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			return MyFormServer.Start(_realform, _realobj, year, period);
		}
	}
}
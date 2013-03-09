#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : FormServerActionBase.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd.Actions {
	/// <summary>
	/// 	������� �������� ������� ����
	/// </summary>
	public abstract class FormServerActionBase : ActionBase {
		/// <summary>
		/// 	First phase of execution - override if need special input parameter's processing
		/// </summary>
		protected override void Initialize() {
			base.Initialize();
			MyFormServer = FormServer.Default;
		}

		/// <summary>
		/// 	������ �� ������ ����
		/// </summary>
		protected FormServer MyFormServer;
	}
}
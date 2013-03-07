#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : FormSessionActionBase.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Linq;
using Qorpent.Mvc.Binding;

namespace Zeta.Extreme.FrontEnd.Actions {
	/// <summary>
	/// 	������� ��������, ���������� � �������
	/// </summary>
	public abstract class FormSessionActionBase : FormServerActionBase {
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
			MySession = FormServer.Default.Sessions.First(_ => _.Uid == Session);
		}

		/// <summary>
		/// 	������ �� ������� ������
		/// </summary>
		protected FormSession MySession;

		/// <summary>
		/// 	�������� ���� ������
		/// </summary>
		[Bind(Required = true)] public string Session = "";
	}
}
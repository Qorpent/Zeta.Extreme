#region LICENSE

// Copyright 2012-2013 Fagim Sadykov
// Project: Zeta.Extreme.FrontEnd
// Original file :AddChatMessage.cs
// Branch: ZEUS
// This code is produced especially for ZEUS PROJECT and
// can be used only with agreement from Fagim Sadykov
// and ZEUS PROJECTS'S owner

#endregion

using Qorpent.Mvc;
using Qorpent.Mvc.Binding;

namespace Zeta.Extreme.FrontEnd.Actions.Communication {
	/// <summary>
	/// </summary>
	[Action("zefs.chatadd")]
	public class AddChatMessage : SessionStartBase
	{
		/// <summary>
		///     ����� ���������
		/// </summary>
		[Bind] public string Text { get; set; }
		/// <summary>
		///     ����� ���������
		/// </summary>
		[Bind]
		public string Type { get; set; }


		/// <summary>
		///     processing of execution - main method of action
		/// </summary>
		/// <returns>
		/// </returns>
		protected override object MainProcess() {
			var session = MyFormServer.CreateSession(_realform, _realobj, year, period,null);
			return session.AddChatMessage(Text,Type);
		}
	}
}
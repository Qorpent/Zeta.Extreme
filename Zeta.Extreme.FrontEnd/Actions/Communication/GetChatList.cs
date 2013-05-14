#region LICENSE

// Copyright 2012-2013 Fagim Sadykov
// Project: Zeta.Extreme.FrontEnd
// Original file :GetChatList.cs
// Branch: ZEUS
// This code is produced especially for ZEUS PROJECT and
// can be used only with agreement from Fagim Sadykov
// and ZEUS PROJECTS'S owner

#endregion

using System.Linq;
using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd.Actions.Communication {
	/// <summary>
	/// </summary>
	[Action("zefs.chatlist")]
	public class GetChatList : SessionStartBase
	{
		/// <summary>
		///     processing of execution - main method of action
		/// </summary>
		/// <returns>
		/// </returns>
		protected override object MainProcess() {
			var session = MyFormServer.CreateSession(_realform, _realobj, year, period);
			var result = session.GetChatList().OrderByDescending(_ => _.Time).ToArray();
			if (!Roles.IsInRole(User, "ADMIN")) {
				result = result.Where(_ => _.Type != "admin").ToArray();
			}
			return result;
		}
	}
}
#region LICENSE

// Copyright 2012-2013 Fagim Sadykov
// Project: Zeta.Extreme.FrontEnd
// Original file :GetUpdateCount.cs
// Branch: ZEUS
// This code is produced especially for ZEUS PROJECT and
// can be used only with agreement from Fagim Sadykov
// and ZEUS PROJECTS'S owner

#endregion

using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd.Actions.Communication {
	/// <summary>
	///     Действие, возвращающее количество непрочитанных сообщений по доступным
	///     формам
	/// </summary>
	[Action("zecl.updatecount", Role = "DEFAULT")]
	public class GetUpdateCount : ChatProviderActionBase {
		/// <summary>
		///     processing of execution - main method of action
		/// </summary>
		/// <returns>
		/// </returns>
		protected override object MainProcess() {
			if (IsInRole("BUDGET")) {
				var myobjs = GetMyOwnObjects();
				if (0 == myobjs.Length) return 0;
				return _provider.GetUpdatesCount(Context.User.Identity.Name, myobjs);
			}
			if (IsInRole("OPERATOR") && !IsInRole("ADMIN") && !IsInRole("SYS_ALLOBJECTS")) {
				var myobjs = GetMyAccesibleObjects();
				var myforms = GetMyFormCodes();
				return _provider.GetUpdatesCount(Context.User.Identity.Name,myobjs,forms:myforms);
			}
			return 0;
		}
	}
}
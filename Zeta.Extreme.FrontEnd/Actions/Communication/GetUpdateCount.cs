#region LICENSE

// Copyright 2012-2013 Fagim Sadykov
// Project: Zeta.Extreme.FrontEnd
// Original file :GetUpdateCount.cs
// Branch: ZEUS
// This code is produced especially for ZEUS PROJECT and
// can be used only with agreement from Fagim Sadykov
// and ZEUS PROJECTS'S owner

#endregion

using System.Linq;
using Qorpent.Mvc;
using Zeta.Extreme.Form.Themas;

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
			//	var lastread = _provider.GetLastRead(Context.User.Identity.Name);
			long count = 0;
			if (IsInRole("BUDGET"))
			{
				var myobjs = GetMyOwnObjects();
				if (0 != myobjs.Length) {
					count += _provider.GetUpdatesCount(Context.User.Identity.Name, myobjs,
					                                   new[] {"objcurrator"});
				}

				var curforms = ((ExtremeFormProvider)FormServer.Default.FormProvider).Factory.GetAll().Where(
					_ => _.GetParameter("hold.responsibility", "") == Context.User.Identity.Name.ToLower()
					).SelectMany(_ => new[] { _.Code + "A.in", _.Code + "B.in" }).ToArray();

				if (curforms.Length != 0) {
					count += _provider.GetUpdatesCount(Context.User.Identity.Name, null, new[] {"formcurrator"}, curforms.ToArray());
				}
			}

			else if (!IsInRole("ADMIN") && !IsInRole("SYS_ALLOBJECTS"))
			{
				var myobjs = GetMyAccesibleObjects();
				var myforms = GetMyFormCodes();
				count += _provider.GetUpdatesCount(Context.User.Identity.Name, myobjs, forms: myforms);
			}



			if (IsInRole("SUPPORT", true)) {
				count += _provider.GetUpdatesCount(Context.User.Identity.Name, null,   new[] {"support"}, null);
			}

			return count;
		}
	}
}
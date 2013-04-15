#region LICENSE

// Copyright 2012-2013 Fagim Sadykov
// Project: Zeta.Extreme.FrontEnd
// Original file :SetMessageArchived.cs
// Branch: ZEUS
// This code is produced especially for ZEUS PROJECT and
// can be used only with agreement from Fagim Sadykov
// and ZEUS PROJECTS'S owner

#endregion

using Qorpent.Mvc;
using Qorpent.Mvc.Binding;

namespace Zeta.Extreme.FrontEnd.Actions.Communication {
	/// <summary>
	///     ƒействие, выставл€ющее глобальный признак просмотра обновлений
	/// </summary>
	[Action("zecl.archive", Role = "DEFAULT")]
	public class SetMessageArchived : ChatProviderActionBase {
		/// <summary>
		///     »дентификатор новости
		/// </summary>
		[Bind(Required = true)] public string Id { get; set; }

		/// <summary>
		///     processing of execution - main method of action
		/// </summary>
		/// <returns>
		/// </returns>
		protected override object MainProcess() {
			_provider.Archive(Id, Context.User.Identity.Name);
			return true;
		}
	}
}
using System;
using System.Linq;
using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.FrontEnd.Actions.Communication {
	/// <summary>
	///     Действие, выставляющее глобальный признак просмотра обновлений
	/// </summary>
	[Action("zecl.get", Role = "BUDGET,CURATOR")]
	public class GetAllMessages : ChatProviderActionBase
	{
		/// <summary>
		/// Начальная дата
		/// </summary>
		[Bind]public DateTime From { get; set; }

		/// <summary>
		/// Признак показа архивированных
		/// </summary>
		[Bind]public bool ShowArchived { get; set; }

		/// <summary>
		/// Фильтр типов сообщений
		/// </summary>
		[Bind]public string TypeFilter { get; set; }

		

		/// <summary>
		///     processing of execution - main method of action
		/// </summary>
		/// <returns>
		/// </returns>
		protected override object MainProcess() {
			var myobjs = GetMyOwnObjects();
			if (0 == myobjs.Length) return new FormChatItem[] {};
			return _provider.FindAll(Context.User.Identity.Name, From, myobjs, TypeFilter.SmartSplit().ToArray(), ShowArchived).ToArray();
		}
	}
}
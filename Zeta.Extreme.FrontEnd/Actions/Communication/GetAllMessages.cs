using System;
using System.Linq;
using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Qorpent.Utils.Extensions;

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
			return _provider.FindAll(Context.User.Identity.Name, From, GetMyOwnObjects(), TypeFilter.SmartSplit().ToArray(),
			                         ShowArchived).ToArray();
		}
	}
}
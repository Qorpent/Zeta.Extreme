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
	[Action("zecl.get", Role = "DEFAULT")]
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
			var lastread = _provider.GetLastRead(Context.User.Identity.Name);
			FormChatItem[] items = null;
			if (IsInRole("BUDGET"))
			{
				var myobjs = GetMyOwnObjects();
				if (0 == myobjs.Length) return new FormChatItem[]{};
				items= _provider.FindAll(Context.User.Identity.Name, From, myobjs, includeArchived: ShowArchived).OrderByDescending(_ => _.Time).ToArray() ;
			}
			else if (!IsInRole("ADMIN") && !IsInRole("SYS_ALLOBJECTS"))
			{
				var myobjs = GetMyAccesibleObjects();
				var myforms = GetMyFormCodes();
				items = _provider.FindAll(Context.User.Identity.Name, From, myobjs, forms: myforms, includeArchived:ShowArchived).OrderByDescending(_=>_.Time).ToArray();
			}
			if (null != items) {
				foreach (var chatitem in items) {
					if (chatitem.Time > lastread) {
						chatitem.Userdata["isnew"] = true;
					}
				}
			}
			return  items ?? new FormChatItem[]{};
		}
	}
}
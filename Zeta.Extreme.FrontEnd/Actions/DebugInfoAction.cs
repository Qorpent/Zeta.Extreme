using System;
using System.Linq;
using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.FrontEnd.Session;

namespace Zeta.Extreme.FrontEnd.Actions {
	/// <summary>
	/// 	Возвращает информацию о сессии
	/// </summary>
	[Action("zefs.debuginfo")]
	public class DebugInfoAction : SessionAttachedActionBase
	{
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			var stats = string.IsNullOrWhiteSpace(_session.DataStatistics)
				            ? null
				            : _session.DataStatistics.SmartSplit(false, true, '\r', '\n').ToArray();
			return new {colset = _session.Colset, stats, sql = _session.SqlLog};
		}
	}
}
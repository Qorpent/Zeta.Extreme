#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : DebugInfoAction.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Linq;
using Qorpent.Mvc;
using Qorpent.Utils.Extensions;

namespace Zeta.Extreme.FrontEnd.Actions {
	/// <summary>
	/// 	Возвращает информацию о сессии
	/// </summary>
	[Action("zefs.debuginfo")]
	public class DebugInfoAction : SessionAttachedActionBase {
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
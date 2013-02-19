#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : GetDataAction.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Linq;
using Qorpent.Mvc;
using Qorpent.Mvc.Binding;

namespace Zeta.Extreme.FrontEnd.Actions {
	/// <summary>
	/// 	Инициирует сессию
	/// </summary>
	[Action("zefs.data")]
	public class GetDataAction : SessionAttachedActionBase {
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			lock (_session.Data) {
				var state = _session.IsFinished ? "f" : "w";
				if (!string.IsNullOrWhiteSpace(_session.ErrorMessage)) {
					state = "e";
				}
				var max = _session.Data.Count - 1;
				if (_session.Data.Count <= startidx) {
					return new {state, ei = max};
				}

				var cnt = max - startidx + 1;

				return
					new
						{
							si = startidx,
							ei = max,
							state,
							e = _session.ErrorMessage,
							data = _session.Data.Skip(startidx).Take(cnt).ToArray()
						};
			}
		}

		[Bind(Required = false)] private int startidx = 0;
	}
}
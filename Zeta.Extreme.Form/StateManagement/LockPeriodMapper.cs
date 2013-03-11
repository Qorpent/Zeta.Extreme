#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : LockPeriodMapper.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using System.Linq;
using Qorpent.Applications;
using Qorpent.Bxl;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.BizProcess.StateManagement;

namespace Zeta.Extreme.Form.StateManagement {
	/// <summary>
	/// 	Локер периодов по умолчанию
	/// </summary>
	public class LockPeriodMapper : ILockPeriodMapper {
		private static bool loaded;

		private static readonly IDictionary<string, int[]> lockmap = new Dictionary<string, int[]>();

		

		/// <summary>
		/// 	Получить список блокированных периодов с командами блокировки
		/// </summary>
		/// <param name="operation"> </param>
		/// <param name="givenperiod"> </param>
		/// <returns> </returns>
		public int[] GetLockingPeriods(LockOperation operation, int givenperiod) {
			checkMap();
			var prefix = operation.ToString() + givenperiod;
			var toperiods = lockmap.SafeGet(prefix)?? new int[] {};
			return toperiods;
		}

		private static void checkMap() {
			lock (lockmap) {
				if (!loaded) {
					reloadCheckMap();
				}
			}
		}

		private static void reloadCheckMap() {
			var file = Application.Current.Files.Read<string>("lockmap.bxl");
			if (file.IsNotEmpty()) {
				var xml = Application.Current.Bxl.Parse(file);
				foreach (var element in xml.Elements()) {
					var fromperiod = element.Attr("code");
					var prefix = element.Name.LocalName;
					if (prefix == "lock") {
						prefix = "Block";
					}
					else if (prefix == "unlock") {
						prefix = "Open";
					}
					prefix = prefix + fromperiod;
					var _toperiods = element.Value;
					var toperiods = _toperiods.SmartSplit().Select(x => x.ToInt()).ToArray();
					lockmap[prefix] = toperiods;
				}
			}
			loaded = true;
		}
	}
}
#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : LockPeriodMapper.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using System.Linq;
using Comdiv.Application;
using Comdiv.Common;
using Comdiv.Extensions;
using Comdiv.IO;
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

		static LockPeriodMapper() {
			myapp.OnReload += myapp_OnReload;
		}


		/// <summary>
		/// 	Получить список блокированных периодов с командами блокировки
		/// </summary>
		/// <param name="operation"> </param>
		/// <param name="givenperiod"> </param>
		/// <returns> </returns>
		public int[] GetLockingPeriods(LockOperation operation, int givenperiod) {
			checkMap();
			var prefix = operation.ToString() + givenperiod;
			var toperiods = lockmap.get(prefix, new int[] {});
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
			var file = myapp.files.Read("lockmap.bxl");
			if (file.IsNotEmpty()) {
				var xml = new BxlParser().Parse(file);
				foreach (var element in xml.Elements()) {
					var fromperiod = element.attr("code");
					var prefix = element.Name.LocalName;
					if (prefix == "lock") {
						prefix = "Block";
					}
					else if (prefix == "unlock") {
						prefix = "Open";
					}
					prefix = prefix + fromperiod;
					var _toperiods = element.Value;
					var toperiods = _toperiods.SmartSplit().Select(x => x.toInt()).ToArray();
					lockmap[prefix] = toperiods;
				}
			}
			loaded = true;
		}

		private static void myapp_OnReload(object sender, EventWithDataArgs<int> args) {
			lock (lockmap) {
				lockmap.Clear();
				loaded = false;
			}
		}
	}
}
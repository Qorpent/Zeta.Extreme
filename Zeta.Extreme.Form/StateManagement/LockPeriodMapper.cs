#region LICENSE
// Copyright 2007-2013 Qorpent Team - http://github.com/Qorpent
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// PROJECT ORIGIN: Zeta.Extreme.Form/LockPeriodMapper.cs
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
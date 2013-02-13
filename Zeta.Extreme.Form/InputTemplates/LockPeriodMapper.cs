// // Copyright 2007-2010 Comdiv (F. Sadykov) - http://code.google.com/u/fagim.sadykov/
// // Supported by Media Technology LTD 
// //  
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// //  
// //      http://www.apache.org/licenses/LICENSE-2.0
// //  
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
// // 
// // MODIFICATIONS HAVE BEEN MADE TO THIS FILE
using System.Collections.Generic;
using System.Linq;
using Comdiv.Application;
using Comdiv.Common;
using Comdiv.Extensions;
using Comdiv.IO;
using Qorpent.Bxl;

namespace Comdiv.Zeta.Web.InputTemplates{
	/// <summary>
	/// Локер периодов по умолчанию
	/// </summary>
    public class LockPeriodMapper : ILockPeriodMapper{
        private static bool loaded;

        private static readonly IDictionary<string, int[]> lockmap = new Dictionary<string, int[]>();

        static LockPeriodMapper(){
            myapp.OnReload += myapp_OnReload;
        }


		/// <summary>
		/// Получить список блокированных периодов с командами блокировки
		/// </summary>
		/// <param name="operation"></param>
		/// <param name="givenperiod"></param>
		/// <returns></returns>
		public int[] GetLockingPeriods(LockOperation operation, int givenperiod){
            checkMap();
            var prefix = operation.ToString() + givenperiod;
            var toperiods = lockmap.get(prefix, new int[]{});
            return toperiods;
        }

        private static void checkMap(){
            lock (lockmap){
                if (!loaded){
                    reloadCheckMap();
                }
            }
        }

        private static void reloadCheckMap(){
            var file = myapp.files.Read("lockmap.bxl");
            if (file.hasContent()){
				var xml = new BxlParser().Parse(file);
                foreach (var element in xml.Elements()){
                    var fromperiod = element.attr("code");
                    var prefix = element.Name.LocalName;
                    if (prefix == "lock"){
                        prefix = "Block";
                    }
                    else if (prefix == "unlock"){
                        prefix = "Open";
                    }
                    prefix = prefix + fromperiod;
                    var _toperiods = element.Value;
                    var toperiods = _toperiods.split().Select(x => x.toInt()).ToArray();
                    lockmap[prefix] = toperiods;
                }
            }
            loaded = true;
        }

        private static void myapp_OnReload(object sender, EventWithDataArgs<int> args){
            lock (lockmap){
                lockmap.Clear();
                loaded = false;
            }
        }
    }
}
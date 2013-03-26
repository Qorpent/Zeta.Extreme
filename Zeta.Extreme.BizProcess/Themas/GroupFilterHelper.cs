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
// PROJECT ORIGIN: Zeta.Extreme.BizProcess/GroupFilterHelper.cs
#endregion
using System.Linq;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.BizProcess.Themas
{
	/// <summary>
	/// Фильтр по группам предприятий для колонок
	/// </summary>
	public static class GroupFilterHelper
	{
		/// <summary>
		/// Проверяет соответвие группе
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="forgroupstr"></param>
		/// <returns></returns>
		public static bool IsMatch(IZetaMainObject obj, string forgroupstr) {
			if(forgroupstr.IsEmpty()) return true;
			if (null == obj) return true;
			var rules = forgroupstr.SmartSplit(false,true,'/');
			var includes = rules.Where(x => !x.StartsWith("!")).ToArray();
			var excludes = rules.Where(x => x.StartsWith("!")).Select(x => x.Substring(1)).ToArray();
			if (excludes.Any(exclude => IsMatchRule(obj, exclude))) {
				return false;
			}
			if (includes.Any(include => IsMatchRule(obj, include))) {
				return true;
			}
			if( includes.Length != 0 ) {
				return false; //должен был просоответствовать хотя бы одному правилу включения (если есть)
			}

			return true;
		}
		/// <summary>
		/// Проверяет соответствие правилу
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="rule"></param>
		/// <returns></returns>
		public static bool IsMatchRule(IZetaMainObject obj, string rule) {
			bool ismatch = false;
			if (rule.StartsWith("div_")) {
				ismatch = obj.Division.Code == rule.Substring(4);
			}
			else if (rule.StartsWith("obj_")) {
				ismatch = obj.Id == rule.Substring(4).ToInt();
			}
			else if (rule.StartsWith("otr_")) {
				ismatch = obj.Department.Code == rule.Substring(4);
			}
			else {
				ismatch = obj.GroupCache.Contains("/" + rule + "/");
			}
			return ismatch;
		}
	}
}

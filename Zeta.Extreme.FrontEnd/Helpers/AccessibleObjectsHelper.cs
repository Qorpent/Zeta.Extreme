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
// PROJECT ORIGIN: Zeta.Extreme.FrontEnd/AccessibleObjectsHelper.cs
#endregion
using System.Linq;
using System.Security.Principal;
using Qorpent.Applications;
using Zeta.Extreme.Form.Meta;

namespace Zeta.Extreme.FrontEnd.Helpers {
	/// <summary>
	/// 	Хелпер для получения доступных предприятий
	/// </summary>
	public class AccessibleObjectsHelper {
		/// <summary>
		/// 	Подготавливает объект доступа
		/// </summary>
		/// <param name="principal"> </param>
		/// <returns> </returns>
		public AccessibleObjects GetAccessibleObjects(IPrincipal principal = null) {
			principal = principal ?? Application.Current.Principal.CurrentUser;
			var objects = UserOrgDataMapper.GetAvailOrgs(principal, null, true).Where(_ => null != _.Division).ToArray();
			var divs =
				objects.Select(_ => _.Division).Distinct().Select(_ => new DivisionRecord {code = _.Code, name = _.Name, idx = _.Idx}).
					ToArray();
			var objs =
				objects.Select(
					_ =>
					new ObjectRecord
						{id = _.Id, name = _.Name, shortname = _.ShortName, div = _.Division.Code, idx = _.Idx})
					.ToArray();
			return new AccessibleObjects {divs = divs, objs = objs};
		}
	}
}
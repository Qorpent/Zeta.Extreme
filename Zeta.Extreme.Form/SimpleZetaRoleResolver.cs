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
// PROJECT ORIGIN: Zeta.Extreme.Form/SimpleZetaRoleResolver.cs
#endregion
using System.Collections.Generic;
using System.Security.Principal;
using Qorpent;
using Qorpent.Mvc;
using Qorpent.Security;

namespace Zeta.Extreme.Form {
	/// <summary>
	/// 	Простое расширение, запрашивающее роли посредством хранимой процедуры
	/// </summary>
	public class SimpleZetaRoleResolver : ServiceBase, IRoleResolverExtension {
		/// <summary>
		/// 	An index of object
		/// </summary>
		public int Index { get; set; }

		/// <param name="principal"> </param>
		/// <param name="role"> </param>
		/// <param name="exact"> </param>
		/// <param name="callcontext"> </param>
		/// <param name="customcontext"> </param>
		/// <returns> </returns>
		public bool IsInRole(IPrincipal principal, string role, bool exact = false, IMvcContext callcontext = null,
		                     object customcontext = null) {
			using (var c = Application.DatabaseConnections.GetConnection("Default")) {
				c.Open();
				var cmd = c.CreateCommand();
				cmd.CommandText = string.Format("select count(*) from zeta.usrrolemap where login = '{0}' and rolename ='{1}'",
				                                principal.Identity.Name, role);
				var result = (int) cmd.ExecuteScalar();
				return 0 != result;
			}
		}

		/// <summary>
		/// 	Returns registered roles of this extension
		/// </summary>
		/// <returns> </returns>
		public IEnumerable<string> GetRoles() {
			using (var c = Application.DatabaseConnections.GetConnection("Default")) {
				c.Open();
				var cmd = c.CreateCommand();
				cmd.CommandText = "select distinct(rolename) from zeta.usrrolemap";
				using (var r = cmd.ExecuteReader()) {
					while (r.Read()) {
						yield return r.GetString(0);
					}
				}
			}
		}

		/// <summary>
		/// 	Возвращает признак обслуживания учетных записей суперпользователя
		/// </summary>
		public bool IsExclusiveSuProvider {
			get { return false; }
		}
	}
}
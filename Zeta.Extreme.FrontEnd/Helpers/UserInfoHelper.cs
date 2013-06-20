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
// PROJECT ORIGIN: Zeta.Extreme.FrontEnd/UserInfoHelper.cs
#endregion
using System;
using System.Linq;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.SqlSupport;

namespace Zeta.Extreme.FrontEnd.Helpers {
	/// <summary>
	/// 	¬спомогательный класс дл€ доступа к данным пользовател€
	/// </summary>
	public class UserInfoHelper {
		/// <summary>
		/// 	¬озвращает упрощенную запись о пользователе
		/// </summary>
		/// <param name="login"> </param>
		/// <returns> </returns>
		/// <exception cref="Exception"></exception>
		public SimpleUserInfo GetUserInfo(string login) {
			if (login.Contains(" ") || login.Contains("'")) {
				throw new Exception("sql injection with usrinfo " + login);
			}
			var usr = new NativeZetaReader().ReadUsers("Login = '" + login + "'").FirstOrDefault();
			if (null == usr) {
				return new SimpleUserInfo {Login = login, Name = "NOT REGISTERED IN DB"};
			}
			var result = GetUserInfo(usr);
			return result;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SimpleUserInfo GetUserInfoByName(string name) {
            if (name.Contains("'")) {
                throw new Exception("sql injection with usrinfo " + name);
            }

            var usr = new NativeZetaReader().ReadUsers("Name like '%" + name + "%'").FirstOrDefault();
            if (null == usr) {
                return new SimpleUserInfo {Name = name };
            }
            var result = GetUserInfo(usr);
            return result;
        }

		/// <summary>
		///  онвертирует существующий логин в запись
		/// </summary>
		/// <param name="usr"></param>
		/// <param name="fullData"></param>
		/// <returns></returns>
		public  SimpleUserInfo GetUserInfo(IZetaUser usr,bool fullData = false) {
			var result = new SimpleUserInfo
				{
					Active = usr.Active,
					Contact = usr.Contact,
					Dolzh = usr.Occupation,
					Email = usr.Comment,
					IsObjAdmin = usr.IsLocalAdmin,
					Login = usr.Login,
					Name = usr.Name,
				};
			if (null != usr.Object) {
				result.ObjId = usr.Object.Id;
				result.ObjName = usr.Object.Name;
			}
			if (fullData) {
				result.Slots = usr.Slots.ToArray();
				var allroles = usr.Roles.SmartSplit();
				var objroles = allroles.Where(_ => 
					_.EndsWith("_OPERATOR") || 
					_.EndsWith("_UNDERWRITER") || 
					_.EndsWith("_ANALYTIC") ||
					_.EndsWith("_POPERATOR") ||
					_.EndsWith("_PUNDERWRITER") ||
					_.EndsWith("_PANALYTIC") 
					).ToArray();
				var sysroles = allroles.Except(objroles).ToArray();
				result.ObjRoles = objroles;
				result.SysRoles = sysroles;
			}
			return result;
		}
	}
}
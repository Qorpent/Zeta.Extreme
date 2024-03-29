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
using System.Collections.Generic;
using System.Linq;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.SqlSupport;

namespace Zeta.Extreme.FrontEnd.Helpers {
	/// <summary>
	/// 	��������������� ����� ��� ������� � ������ ������������
	/// </summary>
	public class UserInfoHelper {
        /// <summary>
        ///     ���������� ���������� ������ � ������������
        /// </summary>
        /// <param name="login"></param>
        /// <param name="withRoles"></param>
        /// <returns></returns>
		public SimpleUserInfo GetUserInfo(string login, bool withRoles = false) {
			if (login.Contains(" ") || login.Contains("'")) {
				throw new Exception("sql injection with usrinfo " + login);
			}

			var usr = new NativeZetaReader().ReadUsers("Login = '" + login + "'").FirstOrDefault();
			if (null == usr) {
                return new SimpleUserInfo {
                    Login = login,
                    Name = "NOT REGISTERED IN DB"
                };
			}

            return GetUserInfo(usr, withRoles);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEnumerable<SimpleUserInfo> GetUsersInfoByName(string name) {
            if (name.Contains("'")) {
                throw new Exception("sql injection with usrinfo " + name);
            }
            
            var users = new NativeZetaReader().ReadUsers("Name like '%" + name + "%'");
            var result = new List<SimpleUserInfo>();

            foreach (var usr in users) {
                result.Add(GetUserInfo(usr));
            }
            
            return result;
        }

                /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IList<SimpleUserInfo> GetUsersInfoByNameWithRoles(string name) {
            if (name.Contains("'")) {
                throw new Exception("sql injection with usrinfo " + name);
            }
            
            var users = new NativeZetaReader().ReadUsers("Name like '%" + name + "%'");
            var result = new List<SimpleUserInfo>();

            foreach (var usr in users) {
                result.Add(GetUserInfoWithRoles(usr));
            }
            
            return result;
        }

		/// <summary>
		/// ������������ ������������ ����� � ������
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <param name="fullData"></param>
        /// <returns></returns>
        public SimpleUserInfoWithRoles GetUserInfoWithRoles(string login, bool fullData = false) {
            if (login.Contains(" ") || login.Contains("'")) {
                throw new Exception("sql injection with usrinfo " + login);
            }

            var usr = new NativeZetaReader().ReadUsers("Login = '" + login + "'").FirstOrDefault();
            if (null == usr) {
                return new SimpleUserInfoWithRoles {
                    Login = login,
                    Name = "NOT REGISTERED IN DB"
                };
            }

            return GetUserInfoWithRoles(usr, fullData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usr"></param>
        /// <param name="fullData"></param>
        /// <returns></returns>
        public SimpleUserInfoWithRoles GetUserInfoWithRoles(IZetaUser usr, bool fullData = false) {
            var userInfo = GetUserInfo(usr, true);
            return new SimpleUserInfoWithRoles {
                Login = userInfo.Login,
                Contact = userInfo.Contact,
                Name = userInfo.Name,
                Dolzh = userInfo.Dolzh,
                Email = userInfo.Email,
                ObjId = userInfo.ObjId,
                ObjName = userInfo.ObjName,
                IsObjAdmin = userInfo.IsObjAdmin,
                Active = userInfo.Active,
                Slots = (fullData) ? userInfo.Slots : null,
                ObjRoles = (fullData) ? userInfo.ObjRoles : null,
                SysRoles = (fullData) ? userInfo.SysRoles : null,
                Roles = string.Join(",", userInfo.SysRoles)
            };
        }
        /// <summary>
        /// ���������� ������������ ������������� �� ��������� ������� � ������
        /// </summary>
        /// <param name="logins"></param>
        /// <returns></returns>
	    public IEnumerable<SimpleUserInfo> GetUsersInfoWithRoles(string[] logins) {
            return logins.Select(login => GetUserInfoWithRoles(login));
        }

	    /// <summary>
        /// ���������� ������������ ������������� �� ��������� ������� ��� �����
        /// </summary>
        /// <param name="logins"></param>
        /// <returns></returns>
        public IEnumerable<SimpleUserInfo> GetUsersInfo(string[] logins) {
            return logins.Select(login => GetUserInfo(login));
        }
	}
}
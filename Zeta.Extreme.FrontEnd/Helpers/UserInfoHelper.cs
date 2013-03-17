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
using Zeta.Extreme.Model.SqlSupport;

namespace Zeta.Extreme.FrontEnd.Helpers {
	/// <summary>
	/// 	Вспомогательный класс для доступа к данным пользователя
	/// </summary>
	public class UserInfoHelper {
		/// <summary>
		/// 	Возвращает упрощенную запись о пользователе
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
			var result = new SimpleUserInfo
				{
					Active = usr.Active,
					Contact = usr.Contact,
					Dolzh = usr.Dolzh,
					Email = usr.Comment,
					IsObjAdmin = usr.Boss,
					Login = usr.Login,
					Name = usr.Name
				};
			if (null != usr.Object) {
				result.ObjId = usr.Object.Id;
				result.ObjName = usr.Object.Name;
			}
			return result;
		}
	}
}
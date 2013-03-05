using System;
using System.Linq;
using Zeta.Extreme.Poco.NativeSqlBind;

namespace Zeta.Extreme.FrontEnd.Helpers {
	/// <summary>
	/// Вспомогательный класс для доступа к данным пользователя
	/// </summary>
	public class UserInfoHelper {
		/// <summary>
		/// Возвращает упрощенную запись о пользователе
		/// </summary>
		/// <param name="login"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public SimpleUserInfo GetUserInfo(string login) {
			if(login.Contains(" ")||login.Contains("'")) throw new Exception("sql injection with usrinfo " + login);
			var usr = new NativeZetaReader().ReadUsers("Login = '" + login + "'").FirstOrDefault();
			if(null==usr) {
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
			if(null!=usr.Object) {
				result.ObjId = usr.Object.Id;
				result.ObjName = usr.Object.Name;
			}
			return result;
		}
	}
}
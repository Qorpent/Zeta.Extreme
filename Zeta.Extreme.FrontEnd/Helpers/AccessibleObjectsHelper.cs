using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Qorpent.Applications;
using Zeta.Extreme.Form.Meta;
using Zeta.Extreme.Meta;
using Zeta.Extreme.Poco.NativeSqlBind;

namespace Zeta.Extreme.FrontEnd.Helpers
{
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

	/// <summary>
	/// Хелпер для получения доступных предприятий
	/// </summary>
	public class AccessibleObjectsHelper
	{
		/// <summary>
		/// Подготавливает объект доступа
		/// </summary>
		/// <param name="principal"> </param>
		/// <returns></returns>
		public AccessibleObjects GetAccessibleObjects(IPrincipal principal = null) {
			principal = principal ?? Application.Current.Principal.CurrentUser;
			var objects = UserOrgDataMapper.GetAvailOrgs(principal,null,true).Where(_=>null!=_.Group).ToArray();
			var divs =
				objects.Select(_ => _.Group).Distinct().Select(_ => new DivisionRecord {code = _.Code, name = _.Name, idx = _.Idx}).ToArray();
			var objs =
				objects.Select(
					_ =>
					new ObjectRecord
						{id = _.Id, name = _.Name,  shortname= _.ShortName, div = _.Group.Code, idx = _.Idx})
					.ToArray();
			return new AccessibleObjects {divs = divs, objs = objs};
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Qorpent.Applications;
using Zeta.Extreme.Form.Meta;
using Zeta.Extreme.Meta;

namespace Zeta.Extreme.FrontEnd.Helpers
{

	

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

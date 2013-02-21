using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Comdiv.Zeta.Data.Minimal;

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
		/// <returns></returns>
		public AccessibleObjects GetAccessibleObjects() {
			var objects = UserOrgDataMapper.GetAvailOrgs(true).Where(_=>null!=_.Group).ToArray();
			var divs =
				objects.Select(_ => _.Group).Distinct().Select(_ => new DivisionRecord {code = _.Code, name = _.Name, idx = _.Idx}).ToArray();
			var objs =
				objects.Select(
					_ =>
					new ObjectRecord
						{id = _.Id, name = string.IsNullOrWhiteSpace(_.ShortName) ? _.Name : _.ShortName, div = _.Group.Code, idx = _.Idx})
					.ToArray();
			return new AccessibleObjects {divs = divs, objs = objs};
		}
	}
}

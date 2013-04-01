using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Qorpent.Applications;
using Zeta.Extreme.Form.Themas;

namespace Zeta.Extreme.FrontEnd.Helpers
{
	/// <summary>
	/// Перечисляет все доступные пользователю формы с указанием групп и родительских форм,
	/// учитываются только доступные формы
	/// </summary>
	public class BizProcessEnumerator
	{
		/// <summary>
		/// Возвращает полный список тем, доступных пользователю на заполнение
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		public IEnumerable<BizProcessRecord> GetAllBizProcesses(IPrincipal user = null) {
			if (null == user) {
				user = Application.Current.Principal.CurrentUser;
			}
			var roles = Application.Current.Roles;
			var factory = ((ExtremeFormProvider) FormServer.Default.FormProvider).Factory;
			var allthemas = factory.GetAll().Where(_ => !_.Code.EndsWith("lib") && roles.IsInRole(user, _.Role) && _.GetAllForms().Any(f=>roles.IsInRole(user,f.Role)));
			var themarecords =
				allthemas.Select(
					_ => new BizProcessRecord
						{
							Code = _.Code, 
							Name = _.Name, 
							Group = _.Group, 
							Idx=_.Idx, 
							Parent = _.Parent, 
							IsGroup = _.IsGroup
						});


			return
				themarecords.OrderByDescending(_ => _.IsGroup)
				            .ThenBy(_ => _.Idx)
				            .ThenBy(_ => _.Parent)
				            .ThenBy(_ => _.Name)
				            .ToArray();
		} 
	}
}

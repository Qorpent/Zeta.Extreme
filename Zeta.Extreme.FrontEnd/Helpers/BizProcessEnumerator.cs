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
							Idx=_.Idx == 0?100:_.Idx, 
							Parent = _.Parent, 
							IsGroup = _.IsGroup,
							Thema = _,
						}).ToArray();

			//для родительских тем, по которым в набор включены дочки выводим их тоже в под-группу
			var parents = themarecords.Where(_ => !string.IsNullOrWhiteSpace(_.Parent)).Select(_ => _.Parent).Distinct().ToArray();
			foreach (var bizProcessRecord in themarecords) {
				if (parents.Any(_ => _ == bizProcessRecord.Code)) {
					bizProcessRecord.Parent = bizProcessRecord.Code;
					if (bizProcessRecord.Idx >= 100) {
						bizProcessRecord.Idx = 1;
					}
				}
			}

			

			var groupbygrop = themarecords.Where(_=>!string.IsNullOrWhiteSpace(_.Group)).GroupBy(_ => _.Group).ToArray();
			var groupbyparent = themarecords.Where(_ => !string.IsNullOrWhiteSpace(_.Parent)).GroupBy(_ => _.Parent).ToArray();

			foreach (var p in groupbyparent) {
				if (1 == p.Count()) {
					p.First().Parent = "";
				}
				else {
					var fstgroup = p.First().Group;
					foreach (var bizProcessRecord in p) {
						bizProcessRecord.Group = fstgroup;
					}
				}

			}
			foreach (var p in groupbygrop)
			{
				if (1 == p.Count())
				{
					p.First().Group = "";
				}
			}

			return
				themarecords.OrderByDescending(_ =>string.IsNullOrWhiteSpace(_.Group)?"ZZZZZZZ": _.Group)
				            .ThenBy(_ =>string.IsNullOrWhiteSpace(_.Parent)?"ZZZZZZZ": _.Parent)
				            .ThenBy(_ => _.Idx)
				            .ThenBy(_ => _.Name)
				            .ToArray();
		} 
	}
}

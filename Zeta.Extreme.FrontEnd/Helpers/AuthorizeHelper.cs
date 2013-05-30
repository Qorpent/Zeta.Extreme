using Qorpent.Applications;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.BizProcess.Themas;

namespace Zeta.Extreme.FrontEnd.Helpers {
	/// <summary>
	/// Вспомогательный класс оценки доступности формы
	/// </summary>
	public class AuthorizeHelper {
		/// <summary>
		/// Проверяет возможность доступа пользователя к форме ввода
		/// </summary>
		/// <param name="form"></param>
		/// <returns></returns>
		public bool IsAllowed(IInputTemplate form) {	
			if (!string.IsNullOrWhiteSpace(form.Thema.Role))
			{
				var roles = form.Thema.Role.SmartSplit();
				bool allow = false;
				foreach (var role in roles)
				{
					if (Application.Current.Roles.IsInRole(Application.Current.Principal.CurrentUser, role))
					{
						allow = true;
					}
				}
				if (!allow) {
					return false;
				}
			}
			if (!Application.Current.Roles.IsInRole(Application.Current.Principal.CurrentUser, form.Role)) {
				return false;
			}

			return true;
		}
	}
}
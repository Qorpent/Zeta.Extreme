using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Qorpent.Security;
using Zeta.Extreme.Form;

namespace Zeta.Extreme.FrontEnd.Actions.Info {

    [Action("zefs.getuserroles", Role = "DEFAULT")]
    class GetUserRolesAction : ActionBase {
        /// <summary>
        ///     User login
        /// </summary>
        [Bind(Required = true)] protected string Login;

        protected override object MainProcess()
        {
            return null;
            /*var roleResolver = Application.Container.Get<ISimpleZetaRoleResolver>();
            
 
            if (Login == null) {
                Login = Context.LogonUser.Identity.Name;
            }

            if (Application.Roles.IsInRole(Context.LogonUser, "ADMIN")) {
                return roleResolver.GetUserRoles(Login).ToString();
            }

            if (Context.LogonUser.Identity.Name == Login) {
                return roleResolver.GetUserRoles(Login).ToString();
            }

            throw new QorpentSecurityException("Вы не имеете прав на просмотр ролей других пользователей");*/
        }

    }
}

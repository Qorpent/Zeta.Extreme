using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using Qorpent.Applications;
using Qorpent.Mvc;
using Qorpent.Security;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.MetaCaches;
using Zeta.Extreme.Model.SqlSupport;

namespace Zeta.Extreme.Model
{
    /// <summary>
    /// Вспомогательный класс для работы с минихолдингами
    /// </summary>
    public static class MiniholdingHelper {
        /// <summary>
        /// Имя тега управляющей компании
        /// </summary>
        public const string ManageCompanyTag = "uc";
        /// <summary>
        /// Имя роли минихолдина
        /// </summary>
        public const string AllMiniholdingRole = "ALL_MINIHOLDING";

        /// <summary>
       /// True если DIV имеет пометку тегом uc
       /// </summary>
       /// <param name="division"></param>
       /// <returns></returns>
       public static bool IsMiniholding(this IObjectDivision division) {
           return 0 != division.TagGet(ManageCompanyTag).ToInt();
       }
        /// <summary>
        /// Возвращает идентификатор
        /// </summary>
        /// <param name="division"></param>
        /// <returns></returns>
        public static int GetManageCompanyId(this IObjectDivision division) {
            return division.TagGet(ManageCompanyTag).ToInt();
        }
        /// <summary>
        /// Возвращает ID управляющей компании для отдельного объекта
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int GetManageCompanyId(this IZetaMainObject obj) {
            if (null == obj.Division) return 0;
            return obj.Division.GetManageCompanyId();
        }
        /// <summary>
        /// Проверяет, является ли объект управляющей компанией
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsManageCompany(this IZetaMainObject obj) {
            return obj.GetManageCompanyId() == obj.Id;
        }
        /// <summary>
        /// Получить объект управляющей компании
        /// </summary>
        /// <param name="div"></param>
        /// <returns></returns>
        public static IZetaMainObject GetManageCompany(this IObjectDivision div)
        {
            var mcid = div.GetManageCompanyId();
            if (0 == mcid) return null;
            return MetaCache.Default.Get<IZetaMainObject>(mcid);
        }
        /// <summary>
        /// Получить объект управляющей компании
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IZetaMainObject GetManageCompany(this IZetaMainObject obj) {
            var mcid = obj.GetManageCompanyId();
            if (0 == mcid) return null;
            if (obj.Id == mcid) return obj;
            return MetaCache.Default.Get<IZetaMainObject>(mcid);
        }
       
    }
}

using System.Linq;
using Qorpent.Serialization;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Developer.Security.PrefixProblemAnalyzer {
    /// <summary>
    /// Параметры вызова анализатора
    /// </summary>
    [Serialize]
    public class PrefixRecordParameters {
        /// <summary>
        /// Создает стандартные параметры анализатора
        /// </summary>
        public PrefixRecordParameters() {
            UseFact = true;
            UsePlan = true;
            MaxOperators = 5;
            MaxUnderwriters = 5;
        }
        /// <summary>
        /// Признак исключения предприятия
        /// </summary>
        public int[] ExcludeObjects { get; set; }
        /// <summary>
        /// Признак включения предприятий
        /// </summary>
        public int[] IncludeObjects { get; set; }
        /// <summary>
        /// Признак того, что неустановленный префикс - проблема
        /// </summary>
        public bool NotSetPrefixIsProblem { get; set; }
        /// <summary>
        /// Признак того, что неустановленный подписант - проблема
        /// </summary>
        public bool NotSetUnderwriterIsProblem { get; set; }
        /// <summary>
        /// Ограничение на резрешенное число операторов по префиксу
        /// </summary>
        public int MaxOperators { get; set; }
        /// <summary>
        /// Ограничение на разрешенное число подписантов по префиксу
        /// </summary>
        public int MaxUnderwriters { get; set; }

        /// <summary>
        /// Признак использования плановых префиксов
        /// </summary>
        public bool UsePlan { get; set; }
        /// <summary>
        /// Признак использования фактических префиксов
        /// </summary>
        public bool UseFact { get; set; }
        /// <summary>
        /// Список затребованных префиксов
        /// </summary>
        [SerializeNotNullOnly]
        public string[] IncludePrefixes { get; set; }
        /// <summary>
        /// Список исключенных префиксов
        /// </summary>
        [SerializeNotNullOnly]
        public string[] ExcludePrefixes { get; set; }

        /// <summary>
        /// Проверяет применимость данных параметров для конкретной роли
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public bool IsMatch(RoleRecord record) {
             if (record.IsPlan && !UsePlan) return false;
            if (!record.IsPlan && !UseFact) return false;
            if (ExcludePrefixes.ToBool()) {
                if (ExcludePrefixes.Any(_ => _ == record.BasePrefix)) return false;
            }
            if (IncludePrefixes.ToBool()) {
                if (IncludePrefixes.All(_ => _ != record.BasePrefix)) return false;
            }
            return true;
        }
        /// <summary>
        /// Возвращает статус проблемности конкретной записи
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public bool IsValid(PrefixRecord record) {
            if (record.Operators > MaxOperators) return false;
            if (record.Underwriters > MaxUnderwriters) return false;
            if (NotSetUnderwriterIsProblem) {
                if (record.Underwriters == 0) return false;
            }
            return true;
        }
        /// <summary>
        /// Возвращает статус проблемности объекта
        /// </summary>
        /// <returns></returns>
        public bool IsValid(PrefixObjectStats obj, string[] prefixes)
        {
            if (NotSetPrefixIsProblem) {
                if (prefixes.Any(_ => obj.PrefixRecords.All(__ => __.Prefix != _))) {
                    return false;
                }
            }
            return obj.PrefixRecords.All(IsValid);
        }
        /// <summary>
        /// Проверка применимости пользователя
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public bool IsMatch(IZetaUser record) {
            if (ExcludeObjects.ToBool())
            {
                if (ExcludeObjects.Any(_ => _ == record.Object.Id)) return false;
            }
            if (IncludeObjects.ToBool())
            {
                if (IncludeObjects.All(_ => _ != record.Object.Id)) return false;
            }
            return true;
        }
    }
}
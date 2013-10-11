using Qorpent.Serialization;

namespace Zeta.Extreme.Developer.Security.PrefixProblemAnalyzer
{
    /// <summary>
    /// Запись о статистике ролей на префиксе
    /// </summary>
    [Serialize]
    public class PrefixRecord
    {
        /// <summary>
        /// Идентификатор объекта
        /// </summary>
        public int ObjId { get; set; }
        /// <summary>
        /// Код префикса
        /// </summary>
        public string Prefix { get; set; }
        /// <summary>
        /// Общее количество операторов
        /// </summary>
        [SerializeNotNullOnly]
        public int Operators { get; set; }
        /// <summary>
        /// Общее количество подписантов
        /// </summary>
        [SerializeNotNullOnly]
        public int Underwriters { get; set; }
        /// <summary>
        /// Общее количество аналитиков
        /// </summary>
        [SerializeNotNullOnly]
        public int Analytics { get; set; }
        /// <summary>
        /// Количество отметок Оператор+Аналитик, Оператор
        /// </summary>
        [SerializeNotNullOnly]
        public int SoloOperators { get; set; }
        /// <summary>
        /// Количество отметок Подписант+Аналитик, Подписант
        /// </summary>
        [SerializeNotNullOnly]
        public int SoloUnderwriters { get; set; }
        /// <summary>
        /// Количество отметок Аналитик
        /// </summary>
        [SerializeNotNullOnly]
        public int SoloAnalytics { get; set; }
        
        /// <summary>
        /// Применяет роль к счетчикам
        /// </summary>
        /// <param name="roleRecord"></param>
        public void Register(RoleRecord roleRecord) {
            if (roleRecord.Type.HasFlag(RoleType.Operator)) {
                Operators++;
            }
            if (roleRecord.Type.HasFlag(RoleType.Analytic))
            {
                Analytics++;
            }
            if (roleRecord.Type.HasFlag(RoleType.Underwriter))
            {
                Underwriters++;
            }
            if (roleRecord.Type.HasFlag(RoleType.Underwriter) && !roleRecord.Type.HasFlag(RoleType.Operator))
            {
                SoloUnderwriters++;
            }
            if (roleRecord.Type.HasFlag(RoleType.Operator) && !roleRecord.Type.HasFlag(RoleType.Underwriter))
            {
                SoloOperators++;
            }
            if (roleRecord.Type==RoleType.Analytic)
            {
                SoloAnalytics++;
            }
        }
    }
}

using Qorpent.Serialization;

namespace Zeta.Extreme.Developer.Analyzers.UsageStat {
    /// <summary>
    /// Статистика использования
    /// </summary>
    [Serialize]
    public class UsageStatistics
    {
        /// <summary>
        /// Целевой объект
        /// </summary>
        public object Target { get; set; }
        /// <summary>
        /// Количество ссылок в данных
        /// </summary>
        public int Primary { get; set; }
        /// <summary>
        /// Количество ссылко в ненолевых данных
        /// </summary>
        public int NotZeroPrimary { get; set; }
        /// <summary>
        /// Ссылок  в формулах строки
        /// </summary>
        public int RowFormula { get; set; }
        /// <summary>
        /// В формулах колонок
        /// </summary>
        public int ColFormula { get; set; }
        /// <summary>
        /// В формулах периодов
        /// </summary>
        public int PeriodFormula { get; set; }
        /// <summary>
        /// В темах
        /// </summary>
        public int Themas { get; set; }
        /// <summary>
        /// В тегах строк
        /// </summary>
        public int RowTags { get; set; }
        /// <summary>
        /// В тегах строк
        /// </summary>
        public int ColTags { get; set; }
        /// <summary>
        /// В тегах объектов
        /// </summary>
        public int ObjTags { get; set; }
        /// <summary>
        /// Признак первичности
        /// </summary>
        public bool IsPrimary { get; set; }
    }
}
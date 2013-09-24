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
        [SerializeNotNullOnly]
        public int Primary { get; set; }
        /// <summary>
        /// Количество ссылко в ненолевых данных
        /// </summary>

        [SerializeNotNullOnly]
        public int NotZeroPrimary { get; set; }
        /// <summary>
        /// Ссылок  в формулах строки
        /// </summary>
        [SerializeNotNullOnly]
        public int RowFormula { get; set; }
        /// <summary>
        /// В формулах колонок
        /// </summary>
        [SerializeNotNullOnly]
        public int ColFormula { get; set; }
        /// <summary>
        /// В формулах периодов
        /// </summary>
        [SerializeNotNullOnly]
        public int PeriodFormula { get; set; }

        /// <summary>
        /// В темах
        /// </summary>
        [SerializeNotNullOnly]
        public int Themas { get; set; }
        /// <summary>
        /// В тегах строк
        /// </summary>
        [SerializeNotNullOnly]
        public int RowTags { get; set; }
        /// <summary>
        /// В тегах строк
        /// </summary>
        [SerializeNotNullOnly]
        public int ColTags { get; set; }
        /// <summary>
        /// В тегах объектов
        /// </summary>
        [SerializeNotNullOnly]
        public int ObjTags { get; set; }
        /// <summary>
        /// Признак первичности
        /// </summary>
        [SerializeNotNullOnly]
        public bool IsPrimary { get; set; }
        /// <summary>
        /// Код элемента
        /// </summary>
        [SerializeNotNullOnly]
        public string Code { get; set; }
        /// <summary>
        /// Имя
        /// </summary>
        [SerializeNotNullOnly]
        public string Name { get; set; }
        /// <summary>
        /// Признак проблемы
        /// </summary>
        [SerializeNotNullOnly]
        public bool IsProblem { get; set; }
        /// <summary>
        /// Описание проблемы
        /// </summary>
        [SerializeNotNullOnly]
        public string Problem { get; set; }
    }
}
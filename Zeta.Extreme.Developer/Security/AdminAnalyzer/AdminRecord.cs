using Qorpent.Serialization;

namespace Zeta.Extreme.Developer.Security.AdminAnalyzer {
    /// <summary>
    /// 
    /// </summary>
    [Serialize]
    public class AdminRecord {
        /// <summary>
        /// Логин
        /// </summary>
        [Serialize]
        public string Login { get; set; }
        /// <summary>
        /// Имя
        /// </summary>
        [Serialize]
        public string Name { get; set; }
        /// <summary>
        /// Должность
        /// </summary>
        [Serialize]
        public string Occupation { get; set; }
        /// <summary>
        /// Tags
        /// </summary>
        [Serialize]
        public string SlotList { get; set; }

    }
}
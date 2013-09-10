using System.Collections.Generic;
using System.Linq;
using Qorpent.Serialization;
using Qorpent.Utils.Extensions;

namespace Zeta.Extreme.Developer.Security.AdminAnalyzer {
    /// <summary>
    /// Запись об админе на предприятии
    /// </summary>
    [Serialize]
    public class AdminAnalyzerRecord    {
        /// <summary>
        /// Признак наличия присоединенного файла
        /// </summary>
        [Serialize]
        public bool FileAttached { get; set; }

        /// <summary>
        /// Uid присоединенного файла
        /// </summary>
        [SerializeNotNullOnly]
        public string FileUid { get; set; }
        /// <summary>
        /// Количество одновременно активных админов
        /// </summary>
        [Serialize]
        public int ActiveAdminCount { get; set; }
        /// <summary>
        /// Идентификатор предприятия
        /// </summary>
        [Serialize]
        public int ObjectId { get; set; }
        /// <summary>
        /// Проверяет валидность записи
        /// </summary>
        [Serialize]
        public bool IsValid {
            get { return ActiveAdminCount == 1 && FileAttached && Admins.All(_=>_.Login.IsNotEmpty()); }
        }

        /// <summary>
        /// Имя предприятия
        /// </summary>
        [Serialize]
        public string ObjectName { get; set; }
        /// <summary>
        /// Коллекция записей об администраторах
        /// </summary>
        [SerializeNotNullOnly]
        public IList<AdminRecord> Admins {
            get { return _admins; }
        }
        /// <summary>
        /// Порядковый номер в списке
        /// </summary>
        [Serialize]
        public int Index { get; set; }

        private IList<AdminRecord> _admins = new List<AdminRecord>();
 

    }
}
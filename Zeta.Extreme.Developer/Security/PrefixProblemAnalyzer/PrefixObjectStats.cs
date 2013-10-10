using System.Collections.Generic;
using System.Linq;
using Qorpent.Serialization;

namespace Zeta.Extreme.Developer.Security.PrefixProblemAnalyzer {
    /// <summary>
    /// 
    /// </summary>
    [Serialize]
    public class PrefixObjectStats {
        /// <summary>
        /// 
        /// </summary>
        public PrefixObjectStats() {
            PrefixRecords = new List<PrefixRecord>();
        }
        /// <summary>
        /// Обратная ссылка на индекс
        /// </summary>
        
        [IgnoreSerialize]
        public PrefixRecordIndex Index { get; set; }
        /// <summary>
        /// Ид объекта
        /// </summary>
        public int ObjectId { get; set; }
        /// <summary>
        /// Имя объекта
        /// </summary>
        public string ObjectName { get; set; }
        /// <summary>
        /// Записи о префиксах
        /// </summary>
        [IgnoreSerialize]
        public List<PrefixRecord> PrefixRecords { get; private set; }
        /// <summary>
        /// Признак проблемного предприятия
        /// </summary>
        [Serialize]
        public bool IsProblem {
            get { 
               // if (NoSetPrefixes.Length > 0) return true;
                if (PrefixRecords.Any(_ => _.IsProblem)) return true;
                return false;
            }
        }
        /// <summary>
        /// Регистрирует запись о роли
        /// </summary>
        /// <param name="roleRecord"></param>
        public void Register(RoleRecord roleRecord) {
            var existed = PrefixRecords.FirstOrDefault(_ => _.Prefix == roleRecord.Prefix);
            if (null == existed) {
                existed = new PrefixRecord {Prefix = roleRecord.Prefix, ObjId = this.ObjectId};
                PrefixRecords.Add(existed);
            }
            existed.Register(roleRecord);
        }

        private PrefixRecord[] _problemRecords;
        /// <summary>
        /// Проблемные записи
        /// </summary>
        [SerializeNotNullOnly]
        public PrefixRecord[] ProblemRecords {
            get {
                if (null == _problemRecords) {
                    _problemRecords = PrefixRecords.Where(_ => _.IsProblem).ToArray();
                }
                return _problemRecords;
            }
        }

        private string[] _noSetPrefixes = null;
        /// <summary>
        /// Не настроенные префиксы
        /// </summary>
        [SerializeNotNullOnly]
        public string[] NoSetPrefixes {
            get {
                if (null == _noSetPrefixes) {
                    _noSetPrefixes = Index.AllPrefixes.Where(_ => PrefixRecords.All(__ => __.Prefix != _)).ToArray();
                }
                return _noSetPrefixes;
            }
        }
    }
}
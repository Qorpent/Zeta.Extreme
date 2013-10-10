using System.Collections.Generic;
using System.Linq;
using Qorpent.Utils.Extensions;

namespace Zeta.Extreme.Developer.Security.PrefixProblemAnalyzer {
    /// <summary>
    /// Запись о полной роли
    /// </summary>
    public class RoleRecord
    {
        /// <summary>
        /// Префикс
        /// </summary>
        public string Prefix { get; set; }
        /// <summary>
        /// Тип
        /// </summary>
        public RoleType Type { get; set; }

        /// <summary>
        /// Признак плановой роли
        /// </summary>
        public bool IsPlan { get; set; }
        /// <summary>
        /// 
        /// </summary>
        
        public string BasePrefix { get { return IsPlan ? Prefix.Substring(0, Prefix.Length - 1) : Prefix; } }

        /// <summary>
        /// Выполняет сбор описания комплексных ролей из строки
        /// </summary>
        /// <param name="rolelist"></param>
        /// <returns></returns>
        public static IEnumerable<RoleRecord> Collect(string rolelist) {
            var roles = rolelist.SmartSplit().Distinct().OrderBy(_ => _).ToArray();
            IDictionary<string, RoleType> _map =new Dictionary<string, RoleType>();
            foreach (var role in roles) {
                var pair = role.Split('_');
                if (pair.Length != 2) continue;
                var tail = pair[1];
                bool plan = false;
                if (tail.StartsWith("P")) {
                    plan = true;
                    tail = tail.Substring(1);
                }
                if (tail == "OPERATOR" || tail == "UNDERWRITER" || tail == "ANALYTIC") {
                    var prefix = pair[0];
                    if (plan) {
                        prefix = "--"+prefix;

                    }
                    if (_map.ContainsKey(prefix)) {
                        _map[prefix] = _map[prefix] | tail.To<RoleType>();
                    }
                    else {
                        _map[prefix] = tail.To<RoleType>();
                    }
                }
            }
            return _map.Select(_ => {
                var result = new RoleRecord {Prefix = _.Key, Type = _.Value};
                if (result.Prefix.StartsWith("--")) {
                    result.Prefix = result.Prefix.Substring(2)+"P";
                    result.IsPlan = true;
                }
                return result;

            });
        }
    }
}
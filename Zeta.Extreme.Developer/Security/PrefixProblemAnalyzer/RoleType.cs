using System;

namespace Zeta.Extreme.Developer.Security.PrefixProblemAnalyzer {
    /// <summary>
    /// Типы ролей
    /// </summary>
    [Flags]
    public enum RoleType {
        /// <summary>
        /// Оператор
        /// </summary>
        Operator =1,
        /// <summary>
        /// Подписант
        /// </summary>
        Underwriter=2,
        /// <summary>
        /// Аналитик
        /// </summary>
        /// ANALYTIC
        Analytic=4,
    }
}
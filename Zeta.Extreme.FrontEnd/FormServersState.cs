using System;

namespace Zeta.Extreme.FrontEnd {
    /// <summary>
    /// 
    /// </summary>
    public static class FormServersState {
        /// <summary>
        ///     Count of current reload operations
        /// </summary>
        public static Int64 CurrentReloadOperations;

        /// <summary>
        /// 
        /// </summary>
        public static Int64 TotalReloadsCount { get; set; }

        /// <summary>
        ///     Count of total handled sessions
        /// </summary>
        public static Int64 TotalSessionsHandled { get; set; }

        /// <summary>
        ///     Увеличить значение текущих операций перезагрузки
        /// </summary>
        public static void CurrentReloadOperationsIncrease() {
            TotalReloadsCount++;
            CurrentReloadOperations++;
        }

        /// <summary>
        ///     Уменьшить значение текущих операций перезагрузки
        /// </summary>
        public static void CurrentReloadOperationsDecrease() {
            CurrentReloadOperations--;
        }

        /// <summary>
        ///     Увеличивает счётчик количества обработанных сессий
        /// </summary>
        public static void TotalSessionsHandledIncrease() {
            TotalSessionsHandled++;
        }
    }
}

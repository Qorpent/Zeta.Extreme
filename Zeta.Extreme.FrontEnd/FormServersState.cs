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
        ///     Увеличить значение текущих операций перезагрузки
        /// </summary>
        public static void CurrentReloadOperationsIncrease() {
            Qorpent.ServiceState.TotalServerReloadsIncrease();
            CurrentReloadOperations++;
        }

        /// <summary>
        ///     Уменьшить значение текущих операций перезагрузки
        /// </summary>
        public static void CurrentReloadOperationsDecrease() {
            CurrentReloadOperations--;
        }
    }
}

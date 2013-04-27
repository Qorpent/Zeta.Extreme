using System;
using System.Collections.Generic;
using Qorpent;

namespace Zeta.Extreme.FrontEnd {
    class NodeLoadingStatistics {
        /// <summary>
        ///     Словарь, хранящий баллы для вычисления коэффициента старения сервера
        /// </summary>
        public static readonly IDictionary<string, int> Points = new Dictionary<string, int> {
            {"QueriesHandled", 1},
            {"CpuTime", 1},
            {"ServerReloads", 20},
            {"SessionsHandled", 10},
            
            {"ReloadOperations", 5},
            {"FormLoadingOperations", 10},
            {"FormRenderingOperations", 3},
            {"LockFormOperations", 1},
            {"AttachFileOperations", 2},
            {"DataSaveOperations", 7}
        };

        /// <summary>
        ///     Наибольший вес должен иметь загрузка, на втором месте сила, на третьем возраст.
        /// </summary>
        public static readonly IDictionary<string, int> K = new Dictionary<string, int> {
            {"a", 1},
            {"p", 2},
            {"l", 3}
        };

        public static int Age() {
            return ServiceState.TotalQueriesHandled * Points["QueriesHandled"] +
                   Convert.ToInt32(ServiceState.CpuTime.TotalMinutes) * Points["CpuTime"] +
                   FormServersState.TotalReloadsCount * Points["ServerReloads"] +
                   FormServersState.TotalSessionsHandled * Points["SessionsHandled"];
        }

        public static int Load() {
            return FormServersState.CurrentReloadOperations*Points["ReloadOperations"] +
                   FormSessionsState.CurrentFormLoadingOperations * Points["FormLoadingOperations"] +
                   FormSessionsState.CurrentFormRenderingOperations * Points["FormRenderingOperations"] +
                   FormSessionsState.CurrentLockFormOperations * Points["LockFormOperations"] +
                   FormSessionsState.CurrentAttachFileOperations * Points["AttachFileOperations"] +
                   FormSessionsState.CurrentDataSaveOperations * Points["DataSaveOperations"];
        }
    
        public static int Power() {
            return ServiceState.TotalQueriesHandled / (Convert.ToInt32(ServiceState.CpuTime.TotalMinutes) != 0 ? Convert.ToInt32(ServiceState.CpuTime.TotalMinutes) : 1);
        }

        public static int Availability() {
            return K["a"]*Age() + K["p"]*Power() + K["l"]*Load();
        }
    }
}

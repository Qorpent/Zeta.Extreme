using System;
using System.Collections.Generic;

namespace Zeta.Extreme.FrontEnd {
    static class ClusterNodeLoadBuilder {
        /// <summary>
        ///     Подсчитывает текущую загрузку сервера исходя из балльной системы оценки
        /// </summary>
        /// <returns></returns>
        public static IDictionary<string, Int64> Build() {
            Int64 load = 0;

            load += (FormServersState.CurrentReloadOperations) * (ClusterNodeLoadPoints.Reloading);
            load += (FormSessionsState.CurrentFormLoadingOperations) * (ClusterNodeLoadPoints.FormDataLoading);
            load += (FormSessionsState.CurrentFormRenderingOperations) * (ClusterNodeLoadPoints.FromStructureRendering);
            load += (FormSessionsState.CurrentLockFormOperations) * (ClusterNodeLoadPoints.FormBlocking);
            load += (FormSessionsState.CurrentAttachFileOperations) * (ClusterNodeLoadPoints.FileAttaching);
            load += (FormSessionsState.CurrentDataSaveOperations) * (ClusterNodeLoadPoints.DataSave);

            var loadInfo = new Dictionary<string, Int64> {
                {"CurrentReloadOperations", (FormServersState.CurrentReloadOperations) * (ClusterNodeLoadPoints.Reloading)},
                {"CurrentFormLoadingOperations", (FormSessionsState.CurrentFormLoadingOperations) * (ClusterNodeLoadPoints.FormDataLoading)},
                {"CurrentFormRenderingOperations", (FormSessionsState.CurrentFormRenderingOperations) * (ClusterNodeLoadPoints.FromStructureRendering)},
                {"CurrentLockFormOperations", (FormSessionsState.CurrentLockFormOperations) * (ClusterNodeLoadPoints.FormBlocking)},
                {"CurrentAttachFileOperations", (FormSessionsState.CurrentAttachFileOperations) * (ClusterNodeLoadPoints.FileAttaching)},
                {"CurrentDataSaveOperations", (FormSessionsState.CurrentDataSaveOperations) * (ClusterNodeLoadPoints.DataSave)},
                {"Load", load}
            };

            return loadInfo;
        }

        public static void MergeWithAgeStat(IDictionary<string, long> load, Dictionary<string, long> ageInfo) {
            load["TotalQueriesHandled"] = ageInfo["TotalQueriesHandled"];
            load["TotalCpuTime"] = ageInfo["TotalCpuTime"];
            load["TotalServerReloads"] = ageInfo["TotalServerReloads"];
            load["TotalSesionsHandled"] = ageInfo["TotalSesionsHandled"];
            load["Age"] = ageInfo["Age"];
        }
    }
}

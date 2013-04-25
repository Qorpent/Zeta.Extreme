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
            load += (FormSessionsState.LockFormCurrentOperations) * (ClusterNodeLoadPoints.FormBlocking);
            load += (FormSessionsState.CurrentAttachFileOperations) * (ClusterNodeLoadPoints.FileAttaching);
            load += (FormSessionsState.DataSaveOperations) * (ClusterNodeLoadPoints.DataSave);

            var loadInfo = new Dictionary<string, Int64> {
                {"CurrentReloadOperations", (FormServersState.CurrentReloadOperations) * (ClusterNodeLoadPoints.Reloading)},
                {"CurrentFormLoadingOperations", (FormSessionsState.CurrentFormLoadingOperations) * (ClusterNodeLoadPoints.FormDataLoading)},
                {"CurrentFormRenderingOperations", (FormSessionsState.CurrentFormRenderingOperations) * (ClusterNodeLoadPoints.FromStructureRendering)},
                {"LockFormCurrentOperations", (FormSessionsState.LockFormCurrentOperations) * (ClusterNodeLoadPoints.FormBlocking)},
                {"CurrentAttachFileOperations", (FormSessionsState.CurrentAttachFileOperations) * (ClusterNodeLoadPoints.FileAttaching)},
                {"DataSaveOperations", (FormSessionsState.DataSaveOperations) * (ClusterNodeLoadPoints.DataSave)},
                {"Summary", load}
            };

            return loadInfo;
        }
    }
}

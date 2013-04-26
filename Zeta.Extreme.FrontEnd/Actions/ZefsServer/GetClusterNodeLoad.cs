using System;
using System.Collections.Generic;
using Qorpent;
using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd.Actions.ZefsServer {
    /// <summary>
    ///     Возвращает JSON-строку, описывающую текущеее состояние загрузки сервера
    /// </summary>
    [Action("zefs.nodeload",Role="DEFAULT")]
    public class GetClusterNodeLoad : ActionBase {
        /// <summary>
        ///     processing of execution - main method of action
        /// </summary>
        /// <returns></returns>
        protected override object MainProcess() {
            var ageInfo = new Qorpent.Mvc.Actions.GetClusterNodeLoad().Process() as Dictionary<string, Int64> ?? new Dictionary<string, long>();

            ageInfo["TotalServerReloads"] = (FormServersState.TotalReloadsCount) * (ClusterNodeLoadPoints.POINT_PER_RELOAD);
            ageInfo["TotalSesionsHandled"] = (FormServersState.TotalSessionsHandled) * (ClusterNodeLoadPoints.POINT_PEP_HANDLED_SESSION);
            ageInfo["Age"] += (FormServersState.TotalReloadsCount)*(ClusterNodeLoadPoints.POINT_PER_RELOAD);
            ageInfo["Age"] += (FormServersState.TotalSessionsHandled) * (ClusterNodeLoadPoints.POINT_PEP_HANDLED_SESSION);

            var load = ClusterNodeLoadBuilder.Build();
            ClusterNodeLoadBuilder.MergeWithAgeStat(load, ageInfo);

            return load;
        }
    }
}

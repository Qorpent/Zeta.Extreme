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
            Int64 age = 0;

            age += (ServiceState.TotalQueriesHandled) * (Qorpent.Mvc.ClusterNodeLoadPoints.POINT_PER_HANDLED_QUERY);
            age += Convert.ToInt64( (ServiceState.CpuTime.TotalMinutes) * (Qorpent.Mvc.ClusterNodeLoadPoints.POINT_PER_CPU_MINUTE));
            age += (FormServersState.TotalReloadsCount) * (ClusterNodeLoadPoints.POINT_PER_RELOAD);
            age += (FormServersState.TotalSessionsHandled) * (ClusterNodeLoadPoints.POINT_PER_HANDLED_SESSION);

            var ageInfo = new Dictionary<string, object> {
                {"TotalQueriesHandled", (ServiceState.TotalQueriesHandled) * (Qorpent.Mvc.ClusterNodeLoadPoints.POINT_PER_HANDLED_QUERY)},
                {"TotalCpuTime", (ServiceState.CpuTime.TotalMinutes) * (Qorpent.Mvc.ClusterNodeLoadPoints.POINT_PER_CPU_MINUTE)},
                {"TotalServerReloads", (FormServersState.TotalReloadsCount) * (ClusterNodeLoadPoints.POINT_PER_RELOAD)},
                {"TotalSesionsHandled", (FormServersState.TotalSessionsHandled) * (ClusterNodeLoadPoints.POINT_PER_HANDLED_SESSION)},
                {"Age", age}
            };

            var load = ClusterNodeLoadBuilder.Build();
            ClusterNodeLoadBuilder.MergeWithAgeStat(load, ageInfo);
	        
            return load;
        }
    }
}

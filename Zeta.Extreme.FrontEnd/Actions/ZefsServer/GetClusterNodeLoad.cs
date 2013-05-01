using System;
using System.Collections.Generic;
using System.Linq;
using Qorpent;
using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd.Actions.ZefsServer {
    /// <summary>
    ///     Возвращает JSON-строку, описывающую текущеее состояние загрузки сервера
    /// </summary>
    [Action("zefs.nodeload", Role="DEFAULT")]
    public class GetClusterNodeLoad : ActionBase {
        /// <summary>
        ///     processing of execution - main method of action
        /// </summary>
        /// <returns></returns>
        protected override object MainProcess() {
            var statistics = new NodeLoadingStatistics(
                Application.Container.Get<INodeLoadingPoints>() ?? new NodeLoadingPoints()   
            );

            return new Dictionary<string, object> {
               {
                   "Total", new Dictionary<string, object> {
                        {"QueriesHandled", ServiceState.TotalQueriesHandled},
                        {"CpuTime", ServiceState.CpuMinutes},
                        {"ServerReloads", FormServersState.TotalReloadsCount},
                        {"SessionsHandled", FormServersState.TotalSessionsHandled}
                    }
                },
                {
                    "Current", new Dictionary<string, object> {
                        {"CurrentReloadOperations", FormServersState.CurrentReloadOperations},
                        {"CurrentFormLoadingOperations", FormSessionsState.CurrentFormLoadingOperations},
                        {"CurrentFormRenderingOperations", FormSessionsState.CurrentFormRenderingOperations},
                        {"CurrentLockFormOperations", FormSessionsState.CurrentLockFormOperations},
                        {"CurrentAttachFileOperations", FormSessionsState.CurrentAttachFileOperations},
                        {"CurrentDataSaveOperations", FormSessionsState.CurrentDataSaveOperations}
                    }
                },
                {
                    "Age", statistics.Age(100)
                },
                {
                    "Power", statistics.Power()
                },
                {
                    "Load", statistics.Load()
                },
                {
                    "Availability", statistics.Availability()
                }
            };
        }
    }
}

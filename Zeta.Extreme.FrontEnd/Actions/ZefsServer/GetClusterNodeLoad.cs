using System;
using System.Collections.Generic;
using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd.Actions.ZefsServer {
    /// <summary>
    ///     Возвращает JSON-строку, описывающую текущеее состояние загрузки сервера
    /// </summary>
    [Action("zefs.nodeload")]
    class GetClusterNodeLoad : ActionBase {

        /// <summary>
        ///     processing of execution - main method of action
        /// </summary>
        /// <returns></returns>
        protected override object MainProcess() {
            Int64 load = 0;
            
            load += (FormServersState.CurrentReloadOperations)*(5);
            load += (FormSessionsState.CurrentFormLoadingOperations)*(10);
            load += (FormSessionsState.CurrentFormRenderingOperations)*(3);
            load += (FormSessionsState.LockFormCurrentOperations)*(1);
            load += (FormSessionsState.CurrentAttachFileOperations)*(2);

            var loadInfo = new Dictionary<string, Int64> {
                {"CurrentReloadOperations", (FormServersState.CurrentReloadOperations)*(5)},
                {"CurrentFormLoadingOperations", (FormSessionsState.CurrentFormLoadingOperations)*(10)},
                {"CurrentFormRenderingOperations", (FormSessionsState.CurrentFormRenderingOperations)*(3)},
                {"LockFormCurrentOperations", (FormSessionsState.LockFormCurrentOperations)*(1)},
                {"CurrentAttachFileOperations", (FormSessionsState.CurrentAttachFileOperations)*(2)},
                {"Summary", load}
            };

            return loadInfo;
        }
    }
}

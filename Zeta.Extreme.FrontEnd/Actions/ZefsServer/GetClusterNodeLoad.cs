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
            return ClusterNodeLoadBuilder.Build();
        }
    }
}

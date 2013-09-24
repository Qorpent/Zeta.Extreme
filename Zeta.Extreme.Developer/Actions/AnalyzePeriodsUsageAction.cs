﻿using System.Linq;
using Qorpent.Mvc;
using Qorpent.Mvc.Binding;

namespace Zeta.Extreme.Developer.Actions {
    /// <summary>
    /// Выводит статистику использования периодов
    /// </summary>
    [Action("zdev.analyzeperiodsusage",Arm = "dev",Role = "DEVELOPER")]
    public class AnalyzePeriodsUsageAction : ActionBase {
        [Bind]
        private bool ProblemsOnly { get; set; }
        /// <summary>
        /// Выводит всю статистику использования периодов
        /// </summary>
        /// <returns></returns>
        protected override object MainProcess() {
            var result = new Analyzers.UsageStat.PeriodUsageAnalyzer().AnalyzeAll();
            if (ProblemsOnly) return result.Where(_ => _.IsProblem).ToArray();
            return result.ToArray();
        }
    }
}
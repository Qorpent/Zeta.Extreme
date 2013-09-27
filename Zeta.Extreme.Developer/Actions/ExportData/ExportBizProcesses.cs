using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.Developer.Analyzers;
using Zeta.Extreme.Developer.MetaStorage;

namespace Zeta.Extreme.Developer.Actions {
    /// <summary>
    /// Экспорт сфорировать файл биз процессов
    /// </summary>
    [Action(DeveloperConstants.ExportBizprocessesCommand, Arm = "dev", Help = "Сформировать эксортный файл бизпроцессов", Role = "DEVELOPER")]
    public class ExportBizProcesses : ExportActionBase<BizProcessExporter> {
        [Bind]
        bool primaryonly { get; set; }
        /// <summary>
        /// Пробрасывает параметр primaryonly
        /// </summary>
        /// <returns></returns>
        protected override BizProcessExporter InitializeExporter()
        {
            var result = base.InitializeExporter();
            result.PrimaryOnly = primaryonly;
            return result;
        }
    }
}
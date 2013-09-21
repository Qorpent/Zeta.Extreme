using Qorpent.Mvc;
using Zeta.Extreme.Developer.MetaStorage;

namespace Zeta.Extreme.Developer.Actions {
    /// <summary>
    /// Ёкспорт сфорировать файл биз процессов
    /// </summary>
    [Action("zdev.exportbizprocesses", Arm = "dev", Help = "—формировать эксортный файл бизпроцессов", Role = "DEVELOPER")]
    public class ExportBizProcesses : ExportActionBase<BizProcessExporter> { }
}
using Qorpent.Mvc;
using Zeta.Extreme.Developer.MetaStorage;

namespace Zeta.Extreme.Developer.Actions {
    /// <summary>
    /// Ёкспорт сфорировать файл периодов
    /// </summary>
    [Action("zdev.exportcolumns", Arm = "dev", Help = "—формировать эксортный файл колонок", Role = "DEVELOPER")]
    public class ExportColumnsAction : ExportActionBase<ColumnExporter> {
    }
}
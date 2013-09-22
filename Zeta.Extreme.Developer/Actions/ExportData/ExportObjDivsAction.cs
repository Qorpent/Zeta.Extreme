using Qorpent.Mvc;
using Zeta.Extreme.Developer.MetaStorage;

namespace Zeta.Extreme.Developer.Actions {
    /// <summary>
    /// Ёкспорт сфорировать файл периодов
    /// </summary>
    [Action("zdev.exportobjdivs", Arm = "dev", Help = "—формировать эксортный файл дивизионов", Role = "DEVELOPER")]
    public class ExportObjDivsAction : ExportActionBase<ObjDivExporter> { }
}
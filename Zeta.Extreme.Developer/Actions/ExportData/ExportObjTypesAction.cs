using Qorpent.Mvc;
using Zeta.Extreme.Developer.MetaStorage;

namespace Zeta.Extreme.Developer.Actions {
    /// <summary>
    /// Ёкспорт сфорировать файл периодов
    /// </summary>
    [Action("zdev.exportobjtypes", Arm = "dev", Help = "—формировать эксортный файл типов объектов", Role = "DEVELOPER")]
    public class ExportObjTypesAction : ExportActionBase<ObjTypeExporter> { }
}
using Qorpent.Mvc;
using Zeta.Extreme.Developer.MetaStorage;

namespace Zeta.Extreme.Developer.Actions {
    /// <summary>
    /// Ёкспорт сфорировать файл периодов
    /// </summary>
    [Action(DeveloperConstants.ExportGeoCommand, Arm = "dev", Help = "—формировать эксортный файл географического положени€", Role = "DEVELOPER")]
    public class ExportGeoAction : ExportActionBase<GeoExporter> { }
}
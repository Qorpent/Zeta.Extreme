using Qorpent.Mvc;
using Zeta.Extreme.Developer.MetaStorage;

namespace Zeta.Extreme.Developer.Actions {
    /// <summary>
    /// Ёкспорт сфорировать файл периодов
    /// </summary>
    [Action(DeveloperConstants.ExportThemastructureCommand, Arm = "dev", Help = "—формировать эксортный файл структуры тем", Role = "DEVELOPER")]
    public class ExportThemaStructureAction : ExportActionBase<ThemaStructureExporter>
    {
    }
}
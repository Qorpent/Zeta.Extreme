using Qorpent.Mvc;
using Zeta.Extreme.Developer.MetaStorage;

namespace Zeta.Extreme.Developer.Actions {
    /// <summary>
	/// Ёкспорт сфорировать файл периодов
	/// </summary>
	[Action("zdev.exportperiods",Arm="dev",Help="—формировать эксортный файл периодов", Role="DEVELOPER")]
    public class ExportPeriodsAction : ExportActionBase<PeriodsExporter>{}
}
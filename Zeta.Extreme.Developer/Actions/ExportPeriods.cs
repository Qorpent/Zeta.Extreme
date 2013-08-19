using Qorpent.Mvc;
using Zeta.Extreme.Developer.MetaStorage.Periods;

namespace Zeta.Extreme.Developer.Actions {
	/// <summary>
	/// Ёкспорт сфорировать файл периодов
	/// </summary>
	[Action("zdev.exportperiods",Arm="dev",Help="—формировать эксортный файл периодов", Role="DEVELOPER")]
	public class ExportPeriods : ActionBase {
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override object MainProcess() {
			return new PeriodsExporter().GeneratePeriods();
		}
	}
}
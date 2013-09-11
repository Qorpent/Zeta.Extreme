using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.Developer.MetaStorage.Periods;

namespace Zeta.Extreme.Developer.Actions {
	/// <summary>
	/// Экспорт сфорировать файл периодов
	/// </summary>
	[Action("zdev.exportperiods",Arm="dev",Help="Сформировать эксортный файл периодов", Role="DEVELOPER")]
	public class ExportPeriods : ActionBase {

		/// <summary>
		/// Имя класса
		/// </summary>
		[Bind(Default="periods")]
		public string ClassName { get; set; }
		/// <summary>
		/// Пространство имен
		/// </summary>
		[Bind(Default="import")]
		public string Namespace { get; set; }
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override object MainProcess() {
			return new PeriodsExporter{Namespace = Namespace,ClassName = ClassName}
				.GeneratePeriods();
		}
	}
}
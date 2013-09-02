using System.Linq;
using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Developer.DataMigrations;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Developer.Actions {
	/// <summary>
	/// 
	/// </summary>
	[Action("zdev.formulatoprimary", Arm = "dev", Help = "Готовит скрипты переноса формул в первичку", Role = "DEVELOPER")]
	public class FormulaToPrimaryAction : ActionBase
	{
		[Bind]
		private string root { get; set; }
		[Bind]
		private bool metrics { get; set; }
		[Bind]
		private string columns { get; set; }

		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			var row = MetaCache.Default.Get<IZetaRow>(root);
			if (string.IsNullOrWhiteSpace(columns)) {
				columns = "Б1";
			}
			if (metrics) {
				return new FormulaToPrimaryHelper().CalculateMetrics(row, columns.SmartSplit().ToArray());
			}
			return new FormulaToPrimaryHelper().GenerateScript(row, columns.SmartSplit().ToArray());
		}
	}
}
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
		[Bind(Required = true)]
		private string root { get; set; }
		[Bind]
		private string target { get; set; }
		[Bind]
		private bool metrics { get; set; }
		[Bind(Required = true)]
		private string columns { get; set; }
		[Bind]
		private string years { get; set; }
		[Bind]
		private string periods { get; set; }
		[Bind]
		private string objects { get; set; }
		[Bind(Default = "transfer")]
		private string usercode { get; set; }

		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			var task = new TransferTask {
				SourceCode = root,
				TargetCode = target,
				ColumnCodes = columns.SmartSplit().ToArray(),
				Years = years.SmartSplit().Select(_ => _.ToInt()).ToArray(),
				Periods = periods.SmartSplit().Select(_ => _.ToInt()).ToArray(),
				ObjectIds = objects.SmartSplit().Select(_ => _.ToInt()).ToArray(),
				UserCode = string.IsNullOrWhiteSpace(usercode)?"transfer":usercode,
			};
			if (metrics) {
				return task;
			}
			return new TransferHelper().GenerateScript(task);
		}
	}
}
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
	[Action("zdev.transferdata", Arm = "dev", Help = "Готовит скрипты переноса данных", Role = "DEVELOPER")]
	public class TransferDataAction : ActionBase
	{
		[Bind(Required = true)]
		private string @from { get; set; }
		[Bind]
		private string to { get; set; }
		[Bind]
		private bool metrics { get; set; }
		[Bind(Required = true)]
		private string col { get; set; }
		[Bind]
		private string year { get; set; }
		[Bind]
		private string period { get; set; }
		[Bind]
		private string @object { get; set; }
		[Bind(Default = "transfer")]
		private string usr { get; set; }
        [Bind]
        private bool changedonly { get; set; }
        [Bind]
        private bool execute { get; set; }
        [Bind(Name = "makesrcprimary")]
        private bool makeSourceRowPrimary { get; set; }
        [Bind(Name = "srcisformula")]
        private bool treatSourceRowAsFormula { get; set; }
        [Bind(Name = "srcissum")]
        private bool treatSourceRowAsSumma { get; set; }

		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			var task = new TransferTask {
				SourceCode = @from,
				TargetCode = to,
				ColumnCodes = col.SmartSplit().ToArray(),
				Years = year.SmartSplit().Select(_ => _.ToInt()).ToArray(),
				Periods = period.SmartSplit().Select(_ => _.ToInt()).ToArray(),
				ObjectIds = @object.SmartSplit().Select(_ => _.ToInt()).ToArray(),
				UserCode = string.IsNullOrWhiteSpace(usr)?"transfer":usr,
                Execute = execute,
                ChangedOnly = changedonly,
                MakeSourcePrimary = makeSourceRowPrimary,
                TreatSourceAsSum = treatSourceRowAsSumma,
                TreatSourceAsFormula = treatSourceRowAsFormula,
			};
			if (metrics) {
				return task;
			}
			return new TransferHelper().GenerateScript(task);
		}
	}
}
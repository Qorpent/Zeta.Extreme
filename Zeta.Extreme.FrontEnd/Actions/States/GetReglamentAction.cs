using System.Linq;
using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.BizProcess.StateManagement;

namespace Zeta.Extreme.FrontEnd.Actions.States {
	/// <summary>
	/// Действие, возвращающее действующий регламент по блокировкам
	/// </summary>
	[Action("zefs.getreglament",Role="DEFAULT")]
	public class GetReglamentAction : ActionBase {
		/// <summary>
		/// Возвращает упорядоченный массив действующих статей регламента
		/// </summary>
		/// <returns></returns>
		protected override object MainProcess() {
			return
				Container.All<IFormStateAvailabilityChecker>()
				         .OfType<FormStateAvailabilityCheckerBase>()
				         .OrderBy(_ => _.Index)
				         .ThenBy(_ => _.ReglamentCode)
				         .ToArray();
		}
	}
}
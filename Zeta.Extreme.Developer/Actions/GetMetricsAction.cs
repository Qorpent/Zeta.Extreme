using System.Linq;
using Qorpent.IoC;
using Qorpent.Mvc;
using Zeta.Extreme.Developer.CodeMetrics;

namespace Zeta.Extreme.Developer.Actions {
	/// <summary>
	/// 
	/// </summary>
	[Action("zdev.getmetrics", Arm = "dev", Help = "ѕодсчитывает метрики исходного кода", Role = "DEVELOPER")]
	public class GetMetricsAction :ActionBase {
		[Inject] private IMetricService MetricService = null;

		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			return MetricService.Collect().OrderBy(_ => _.Group).ThenBy(_ => _.Idx).ThenBy(_ => _.Name).ToArray();
		}
	}
}
using Qorpent.Mvc;

namespace Zeta.Extreme.Developer.Actions {
	/// <summary>
	/// Возвращает атрибуты колонок
	/// </summary>
	[Action("zdev.params", Role = "DEVELOPER", Arm = "dev")]
	public class GetParamsAction : AnalyzerActionBase
	{
		/// <summary>
		/// Возвращает атрибуты колонок
		/// </summary>
		/// <returns></returns>
		protected override object MainProcess()
		{
			return Analyzer.GetParameters();
		}
	}
}
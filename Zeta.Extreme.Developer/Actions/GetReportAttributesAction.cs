using Qorpent.Mvc;

namespace Zeta.Extreme.Developer.Actions {
	/// <summary>
	/// Возвращает атрибуты колонок
	/// </summary>
	[Action("zdev.reportattributes", Role = "DEVELOPER", Arm = "dev")]
	public class GetReportAttributesAction : AnalyzerActionBase
	{
		/// <summary>
		/// Возвращает атрибуты колонок
		/// </summary>
		/// <returns></returns>
		protected override object MainProcess()
		{
			return Analyzer.GetReportAttributes();
		}
	}
}
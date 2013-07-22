using Qorpent.Mvc;

namespace Zeta.Extreme.Developer.Actions {
	/// <summary>
	/// Возвращает атрибуты колонок
	/// </summary>
	[Action("zdev.substattributes", Role = "DEVELOPER", Arm = "dev")]
	public class GetSubstAttributesAction : AnalyzerActionBase
	{
		/// <summary>
		/// Возвращает атрибуты колонок
		/// </summary>
		/// <returns></returns>
		protected override object MainProcess()
		{
			return Analyzer.GetSubstAttributes();
		}
	}
}
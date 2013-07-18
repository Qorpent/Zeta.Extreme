using Qorpent.Mvc;

namespace Zeta.Extreme.Developer.Actions {
	/// <summary>
	/// Возвращает атрибуты колонок
	/// </summary>
	[Action("zdev.colattributes", Role = "DEVELOPER",Arm="dev")]
	public class GetColAttributesAction : AnalyzerActionBase
	{
		/// <summary>
		/// Возвращает атрибуты колонок
		/// </summary>
		/// <returns></returns>
		protected override object MainProcess()
		{
			return Analyzer.GetColsetAttribtes();
		}
	}
}
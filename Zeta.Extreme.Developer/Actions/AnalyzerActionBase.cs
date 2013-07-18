using Qorpent.IoC;
using Qorpent.Mvc;
using Zeta.Extreme.Developer.Analyzers;

namespace Zeta.Extreme.Developer.Actions {
	/// <summary>
	/// Базовое действие анализатора
	/// </summary>
	public abstract class AnalyzerActionBase : ActionBase {
		/// <summary>
		/// Индекс кода
		/// </summary>
		[Inject]
		public IAnalyzer Analyzer { get; set; }

		/// <summary>
		/// Эти действия всегда возвращают все одинаково
		/// </summary>
		/// <returns></returns>
		protected override bool GetSupportNotModified() {
			return true;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override System.DateTime EvalLastModified() {
			return Analyzer.CodeIndex.LastResetTime;
		}
	}
}
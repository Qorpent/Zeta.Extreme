using System;

namespace Zeta.Extreme.Developer.Scripting {
	/// <summary>
	/// Формирует экспортные формы на B#
	/// </summary>
	public class GenerateFormCommand:GenerateFormActionBase {
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override string GetDialect() {
			return "BSharp";
		}
	}
}
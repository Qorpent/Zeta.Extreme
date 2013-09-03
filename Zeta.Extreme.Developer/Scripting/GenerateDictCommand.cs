using System;
using Qorpent.Config;

namespace Zeta.Extreme.Developer.Scripting {
	/// <summary>
	/// Формирует экспортные словари из Zeta
	/// </summary>
	public class GenerateDictCommand : GenerateFormActionBase {
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override string GetDialect() {
			return "BSharpDict";
		}
	}
}
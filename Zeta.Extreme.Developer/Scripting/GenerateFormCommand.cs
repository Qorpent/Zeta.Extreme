using System;
using Qorpent.Config;

namespace Zeta.Extreme.Developer.Scripting {
	/// <summary>
	/// Формирует экспортные формы на B#
	/// </summary>
	public class GenerateFormCommand:ScriptCommandBase {
		/// <summary>
		/// Выполнение скрипта
		/// </summary>
		/// <param name="context"></param>
		public override void Run(IConfig context) {
			Log.Debug(this.GetType().Name + " not implemented");
		}
	}
}
using System;
using Qorpent.Config;

namespace Zeta.Extreme.Developer.Scripting {
	/// <summary>
	/// Формирует экспортные словари из Zeta
	/// </summary>
	public class GenerateDictCommand : ScriptCommandBase {
		/// <summary>
		/// Выполнение скрипта
		/// </summary>
		/// <param name="context"></param>
		public override void Run(IConfig context) {
			Log.Debug(this.GetType().Name + " not implemented");
		}
	}
}
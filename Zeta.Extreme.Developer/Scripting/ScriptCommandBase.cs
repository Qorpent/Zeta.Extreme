using System.Xml.Linq;
using Qorpent.Config;
using Qorpent.Log;

namespace Zeta.Extreme.Developer.Scripting {
	/// <summary>
	/// 
	/// </summary>
	public abstract class ScriptCommandBase : IScriptCommand {
		/// <summary>
		/// Инициализатор
		/// </summary>
		/// <param name="def"></param>
		public virtual void Initialize(XElement def) {
			Definition = def;
		}
		/// <summary>
		/// Исходное определение
		/// </summary>
		protected XElement Definition { get; set; }

		/// <summary>
		/// Установить родительский скрипт
		/// </summary>
		/// <param name="script"></param>
		public virtual void SetParent(Script script) {
			Script = script;
			Log = script.Log;
		}
		/// <summary>
		/// Журнал
		/// </summary>
		protected IUserLog Log { get; set; }

		/// <summary>
		/// Родительский скрипт
		/// </summary>
		protected Script Script { get; set; }

		/// <summary>
		/// Выполнение скрипта
		/// </summary>
		/// <param name="context"></param>
		public abstract void Run(IConfig context);
	}
}
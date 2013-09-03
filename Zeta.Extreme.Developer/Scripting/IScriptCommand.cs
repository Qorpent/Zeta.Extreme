using System.Xml.Linq;
using Qorpent.Config;

namespace Zeta.Extreme.Developer.Scripting {
	/// <summary>
	/// Интерфейс отдельной команды
	/// </summary>
	public interface IScriptCommand {
		/// <summary>
		/// Инициализатор
		/// </summary>
		/// <param name="def"></param>
		void Initialize(XElement def);
		/// <summary>
		/// Выполнение скрипта
		/// </summary>
		/// <param name="context"></param>
		void Run(IConfig context);

		/// <summary>
		/// Установить родительский скрипт
		/// </summary>
		/// <param name="script"></param>
		void SetParent(Script script);
	}
}
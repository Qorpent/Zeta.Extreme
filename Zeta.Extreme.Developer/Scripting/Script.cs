using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Qorpent.BSharp.Runtime;
using Qorpent.Config;
using Qorpent.Log;

namespace Zeta.Extreme.Developer.Scripting
{
	/// <summary>
	/// Скрипт ZDEV
	/// </summary>
	public class Script: IBSharpRuntimeBound {
		/// <summary>
		/// Журнал
		/// </summary>
		public IUserLog Log = new StubUserLog();
		/// <summary>
		/// 
		/// </summary>
		public Script() {
			
		}

		/// <summary>
		/// Инициализирует объект на биндинг с рантайм-классом
		/// </summary>
		/// <param name="cls"></param>
		public void Initialize(IBSharpRuntimeClass cls) {
			SourceClass = cls;
			Commands = ScriptCommandFactory.GenerateCommands(cls.GetClassElement()).ToArray();
			foreach (var c in Commands) {
				c.SetParent(this);
			}
		}
		/// <summary>
		/// Команды скрипта
		/// </summary>
		public IScriptCommand[] Commands { get; set; }

		/// <summary>
		/// Исходный класс с определением
		/// </summary>
		public IBSharpRuntimeClass SourceClass { get; set; }



		/// <summary>
		/// Выполняет скрипт от имени пользователя
		/// </summary>
		/// <param name="credentials"></param>
		public void Run(ICredentials credentials = null) {
			var context = new ConfigBase();
			context["credentials"] = credentials;
			foreach (var c in Commands) {
				c.Run(context);
			}
		}
	}
}

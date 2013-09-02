using System.IO;
using System.Xml.Linq;
using Qorpent.Config;
using Qorpent.Utils.Extensions;

namespace Zeta.Extreme.Developer.Scripting {
	/// <summary>
	/// Очищает указанную папку от файлов
	/// </summary>
	public class CleanCommand:ScriptCommandBase {
		/// <summary>
		/// Инициализатор
		/// </summary>
		/// <param name="def"></param>
		public override void Initialize(XElement def)
		{
			base.Initialize(def);
			DirectoryName = def.Attr("code");
		}
		/// <summary>
		/// Имя директории для очистки
		/// </summary>
		public string DirectoryName { get; set; }
		/// <summary>
		/// Выполнение скрипта
		/// </summary>
		/// <param name="context"></param>
		public override void Run(IConfig context) {
			Log.Trace("start clean "+DirectoryName);
			if (Directory.Exists(DirectoryName)) {
				foreach (var f in Directory.GetFiles(DirectoryName)) {
					File.Delete(f);
					Log.Debug("delete "+f);
				}
			}
			else {
				Log.Trace("directory not existed");
			}
		}
	}
}
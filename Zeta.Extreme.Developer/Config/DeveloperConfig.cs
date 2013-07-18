using Qorpent;

namespace Zeta.Extreme.Developer.Config
{
	/// <summary>
	/// Описывает настройки для разработки
	/// </summary>
	public class DeveloperConfig : ServiceBase,IDeveloperConfig {
		/// <summary>
		/// Перечень директорий с определениями тем
		/// </summary>
		public string[] ThemaSourceFolders { get; set; }
		/// <summary>
		/// Директория с компилированными темами
		/// </summary>
		public string ThemaComiledFolder { get; set; }

		/// <summary>
		/// Папка с документацией
		/// </summary>
		public string DocFolder { get; set; }

		
		
	}
}

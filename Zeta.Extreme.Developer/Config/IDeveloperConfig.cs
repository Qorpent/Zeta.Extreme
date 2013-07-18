using System.Collections.Generic;
using System.Xml.Linq;
using Zeta.Extreme.Developer.Model;

namespace Zeta.Extreme.Developer.Config {
	/// <summary>
	/// Интерфейс конфигурации среды разработки
	/// </summary>
	public interface IDeveloperConfig {
		/// <summary>
		/// Перечень директорий с определениями тем
		/// </summary>
		string[] ThemaSourceFolders { get; set; }

		/// <summary>
		/// Директория с компилированными темами
		/// </summary>
		string ThemaComiledFolder { get; set; }

		/// <summary>
		/// Папка с документацией
		/// </summary>
		string DocFolder { get; set; }
	}
}
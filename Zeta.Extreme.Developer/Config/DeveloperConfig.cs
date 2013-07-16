using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Qorpent;
using Qorpent.Bxl;
using Qorpent.IoC;
using Zeta.Extreme.Developer.Model;

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

		
		
	}
}

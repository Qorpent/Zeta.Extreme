using System.Xml.Linq;

namespace Zeta.Extreme.Developer.Model {
	/// <summary>
	/// Описание источника темы (файла источника)
	/// </summary>
	public class Source {
		/// <summary>
		/// Имя файла
		/// </summary>
		public string FileName { get; set; }
		/// <summary>
		/// Исходный код (BXL)
		/// </summary>
		public string SourceContent { get; set; }
		/// <summary>
		/// Обработанный контент в формате Xml
		/// </summary>
		public XElement XmlContent { get; set; }
	}
}
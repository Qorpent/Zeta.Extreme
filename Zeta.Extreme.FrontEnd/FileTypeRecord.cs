using Qorpent.Serialization;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// Описывает привязываемые файлы
	/// </summary>
	[Serialize]
	public class FileTypeRecord {
		/// <summary>
		/// Код
		/// </summary>
		public string code { get; set; }
		/// <summary>
		/// Тип
		/// </summary>
		public string name { get; set; }

	}
}
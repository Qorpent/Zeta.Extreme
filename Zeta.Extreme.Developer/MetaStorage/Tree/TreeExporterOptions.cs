namespace Zeta.Extreme.Developer.MetaStorage.Tree {
	/// <summary>
	/// Опции генератора схемы
	/// </summary>
	public class TreeExporterOptions {
		/// <summary>
		/// 
		/// </summary>
		public TreeExporterOptions() {
			CodeMode = TreeExporterCodeMode.Default;
		}
		/// <summary>
		/// При генерации корень экспорта отвязывается от исходного узла
		/// </summary>
		public bool DetachRoot { get; set; }
		/// <summary>
		/// Режим генерации кодов
		/// </summary>
		public TreeExporterCodeMode CodeMode { get; set; }
		/// <summary>
		/// Пространство имен для выгона B#
		/// </summary>
		public string Namespace { get; set; }

		/// <summary>
		/// Имя класса для выгона B#
		/// </summary>
		public string ClassName { get; set; }
	}
}
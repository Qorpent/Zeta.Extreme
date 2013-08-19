namespace Zeta.Extreme.Developer.MetaStorage {
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

	}
}
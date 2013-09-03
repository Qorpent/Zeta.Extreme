namespace Zeta.Extreme.Developer.MetaStorage.Tree.DependencyAnalyzer {
	/// <summary>
	/// Направление поиска зависимостей
	/// </summary>
	public enum DependencyDirection {
		/// <summary>
		/// По восходящей к вызывающим строкам
		/// </summary>
		Up,
		/// <summary>
		/// По нисходящей по зависимым строкам
		/// </summary>
		Down,
	}
}
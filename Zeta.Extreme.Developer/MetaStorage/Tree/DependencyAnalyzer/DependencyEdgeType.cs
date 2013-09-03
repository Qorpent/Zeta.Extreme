using System;

namespace Zeta.Extreme.Developer.MetaStorage.Tree.DependencyAnalyzer {
	/// <summary>
	/// Тип зависимости
	/// </summary>
	[Flags]
	public enum DependencyEdgeType {
		/// <summary>
		/// Через сумму
		/// </summary>
		Sum = 1,
		/// <summary>
		/// Ссылка
		/// </summary>
		Ref = 1<<1,
		/// <summary>
		/// Добавочная ссылка
		/// </summary>
		ExRef = 1<<2,
		/// <summary>
		/// Формула
		/// </summary>
		Formula = 1<<3,
	}
}
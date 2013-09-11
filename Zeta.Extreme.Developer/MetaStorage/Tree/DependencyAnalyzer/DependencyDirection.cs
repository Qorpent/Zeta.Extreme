using System;

namespace Zeta.Extreme.Developer.MetaStorage.Tree.DependencyAnalyzer {
	/// <summary>
	/// Направление поиска зависимостей
	/// </summary>
	[Flags]
	public enum DependencyDirection {
		/// <summary>
		/// По восходящей к вызывающим строкам
		/// </summary>
		Up = 1,
		/// <summary>
		/// По нисходящей по зависимым строкам
		/// </summary>
		Down = 1<<1,
		/// <summary>
		/// В оба направления
		/// </summary>
		Both = Up | Down,
		/// <summary>
		/// По умолчанию в оба направления
		/// </summary>
		Default = Both
	}
}
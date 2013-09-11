using System;

namespace Zeta.Extreme.Developer.MetaStorage.Tree {
	/// <summary>
	/// Тип включения узла
	/// </summary>
	[Flags]
	public enum IncludeType {
		/// <summary>
		/// Не включать
		/// </summary>
		None=1,
		/// <summary>
		/// Сам
		/// </summary>
		Self =1<<1,
		/// <summary>
		/// И узел и дочерние
		/// </summary>
		SelfAndDescendants=1<<2,
		
	}
}
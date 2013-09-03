using System;

namespace Zeta.Extreme.Developer.MetaStorage.Tree.DependencyAnalyzer {
	/// <summary>
	/// Тип узла
	/// </summary>
	[Flags]
	public enum DependencyNodeType {
		/// <summary>
		/// Первичный
		/// </summary>
		Primary =1 ,
		/// <summary>
		/// Суммовой
		/// </summary>
		Sum = 1<<1,
		/// <summary>
		/// Формульный
		/// </summary>
		Formula=1<<2,
		/// <summary>
		/// Контролька
		/// </summary>
		ControlPoint=1<<3,
		/// <summary>
		/// Ссылка
		/// </summary>
		Ref = 1<<4,
		/// <summary>
		/// Набор по умолчанию
		/// </summary>
		Default =Primary|Sum|Formula,
	}
}
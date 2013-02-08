#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : RowTreeUsage.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;

namespace Zeta.Extreme {
	/// <summary>
	/// 	Использование дерева при поиске
	/// </summary>
	[Flags]
	public enum RowTreeUsage {
		/// <summary>
		/// 	Не используется
		/// </summary>
		None = 0,

		/// <summary>
		/// 	Все поддерево
		/// </summary>
		AllWithSelf = 1,

		/// <summary>
		/// 	Все дочерние
		/// </summary>
		AllDescendants = 2,

		/// <summary>
		/// 	Первичные дочерние
		/// </summary>
		Primaries = 4,
	}
}
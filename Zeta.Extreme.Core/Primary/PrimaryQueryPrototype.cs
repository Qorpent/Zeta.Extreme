#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : PrimaryQueryPrototype.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.Primary {
	/// <summary>
	/// 	Описатель прототипа первичного запроса
	/// </summary>
	public struct PrimaryQueryPrototype {
		/// <summary>
		/// 	Признак использования агрегации
		/// </summary>
		public bool UseSum { get; set; }

		/// <summary>
		/// 	Запрет на использование деталей
		/// </summary>
		public bool PreserveDetails { get; set; }

		/// <summary>
		/// 	Потребноcть в использовании деталей
		/// </summary>
		public bool RequireDetails { get; set; }

		/// <summary>
		/// 	Использование специального метода доступа к первичным значениям
		/// </summary>
		public bool RequreZetaEval { get; set; }
	}
}
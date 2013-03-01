namespace Zeta.Extreme {
	/// <summary>
	/// Описатель прототипа первичного запроса
	/// </summary>
	public struct PrimaryQueryPrototype {
		/// <summary>
		/// Признак использования агрегации
		/// </summary>
		public bool UseSum { get; set; }
		/// <summary>
		/// Запрет на использование деталей
		/// </summary>
		public bool PreserveDetails { get; set; }
		/// <summary>
		/// Потребноcть в использовании деталей
		/// </summary>
		public bool RequireDetails { get; set; }
		/// <summary>
		/// Использование специального метода доступа к первичным значениям
		/// </summary>
		public bool RequreZetaEval { get; set; }
	}
}
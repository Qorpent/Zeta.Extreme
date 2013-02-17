namespace Zeta.Extreme {
	/// <summary>
	/// Статус подготовки запроса
	/// </summary>
	public enum PrepareState {
		/// <summary>
		/// Не начат
		/// </summary>
		None,	
		/// <summary>
		/// Задача начата
		/// </summary>
		TaskStarted,
		/// <summary>
		/// Идет процесс подготовки
		/// </summary>
		InPrepare,
		/// <summary>
		/// Готово
		/// </summary>
		Prepared
	}
}
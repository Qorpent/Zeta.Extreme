namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// Вспомогательный интерфейс для сессий для доступа к статистике
	/// </summary>
	public interface IWithSessionStatistics {
		/// <summary>
		/// Доступ к статистике сессии
		/// </summary>
		SessionStatistics Statistics { get; set; }

		/// <summary>
		/// 	Если включено, службы накапливают статистические данные по работе сессии
		/// </summary>
		bool CollectStatistics { get; }
	}
}
namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// Интерфейс измерения "контрагент"
	/// </summary>
	public interface IReferenceHandler:IWithCacheKey {
		/// <summary>
		/// Фильтр по контрагентам
		/// </summary>
		string Contragents { get; set; }
		/// <summary>
		/// Фильтр по счетам
		/// </summary>
		string Accounts { get; set; }
		/// <summary>
		/// Нормализация ищмерения для запроса
		/// </summary>
		/// <param name="session"></param>
		void Normalize(ISession session);
		/// <summary>
		/// Копирование при копировании запроса
		/// </summary>
		/// <returns></returns>
		IReferenceHandler Copy();
	}
}
namespace Comdiv.Zeta.Web.InputTemplates {
	/// <summary>
	/// Интерфейс менеджера статуса периодов
	/// </summary>
	public interface IPeriodStateManager
	{
		/// <summary>
		/// Система
		/// </summary>
		string System { get; set; }
		/// <summary>
		/// БД
		/// </summary>
		string Database { get; set; }
		/// <summary>
		/// Получить все записи
		/// </summary>
		/// <param name="year"></param>
		/// <returns></returns>
		PeriodStateRecord[] All(int year);
		/// <summary>
		/// Получить запись по году и периоду
		/// </summary>
		/// <param name="year"></param>
		/// <param name="period"></param>
		/// <returns></returns>
		PeriodStateRecord Get(int year, int period);
		/// <summary>
		/// Обновить статус
		/// </summary>
		/// <param name="record"></param>
		void UpdateState(PeriodStateRecord record);
		/// <summary>
		/// Обновить дедлайн
		/// </summary>
		/// <param name="record"></param>
		void UpdateDeadline(PeriodStateRecord record);
		/// <summary>
		/// Обновить дедлайн по подписанию
		/// </summary>
		/// <param name="record"></param>
		void UpdateUDeadline(PeriodStateRecord record);
	}
}
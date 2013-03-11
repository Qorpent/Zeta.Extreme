namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// Абстрактная фабрика запросов и сессий
	/// </summary>
	public interface IExtremeFactory {
		/// <summary>
		/// Создает сессии
		/// </summary>
		/// <returns></returns>
		ISession CreateSession(SessionSetupInfo setupInfo = null);
		/// <summary>
		/// Создает запросы
		/// </summary>
		/// <returns></returns>
		IQuery CreateQuery(QuerySetupInfo setupInfo = null);

		/// <summary>
		/// Создает RowHandler
		/// </summary>
		/// <returns></returns>
		IRowHandler CreateRowHandler();
		/// <summary>
		/// Создает ColumnHandler
		/// </summary>
		/// <returns></returns>
		IColumnHandler CreateColumnHandler();

		/// <summary>
		/// Создает ObjHandler
		/// </summary>
		/// <returns></returns>
		IObjHandler CreateObjHandler();


		/// <summary>
		/// Создает TimeHandler
		/// </summary>
		/// <returns></returns>
		ITimeHandler CreateTimeHandler();
		/// <summary>
		/// Акцессор к хранилищу формул
		/// </summary>
		/// <returns></returns>
		IFormulaStorage GetFormulaStorage();
	}
}
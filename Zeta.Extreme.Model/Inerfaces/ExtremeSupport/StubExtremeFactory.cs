 namespace Zeta.Extreme.Model.Inerfaces {
	class StubExtremeFactory : IExtremeFactory {

		/// <summary>
		/// Создает сессии
		/// </summary>
		/// <returns></returns>
		public ISession CreateSession(SessionSetupInfo setupInfo = null) {
			throw new System.NotImplementedException("it's stub factory implementation");
		}

		/// <summary>
		/// Создает запросы
		/// </summary>
		/// <returns></returns>
		public IQuery CreateQuery(QuerySetupInfo setupInfo = null) {
			throw new System.NotImplementedException("it's stub factory implementation");
		}

		/// <summary>
		/// Создает RowHandler
		/// </summary>
		/// <returns></returns>
		public IRowHandler CreateRowHandler() {
			throw new System.NotImplementedException("it's stub factory implementation");
		}

		/// <summary>
		/// Создает ColumnHandler
		/// </summary>
		/// <returns></returns>
		public IColumnHandler CreateColumnHandler() {
			throw new System.NotImplementedException("it's stub factory implementation");
		}

		/// <summary>
		/// Создает ObjHandler
		/// </summary>
		/// <returns></returns>
		public IObjHandler CreateObjHandler() {
			throw new System.NotImplementedException("it's stub factory implementation");
		}

		/// <summary>
		/// Создает TimeHandler
		/// </summary>
		/// <returns></returns>
		public ITimeHandler CreateTimeHandler() {
			throw new System.NotImplementedException("it's stub factory implementation");
		}

		public IFormulaStorage GetFormulaStorage() {
			throw new System.NotImplementedException("it's stub factory implementation");
		}
	}
}
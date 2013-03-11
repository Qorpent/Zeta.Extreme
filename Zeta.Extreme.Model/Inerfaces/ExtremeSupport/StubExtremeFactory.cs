 namespace Zeta.Extreme.Model.Inerfaces {
	class StubExtremeFactory : IExtremeFactory {

		/// <summary>
		/// ������� ������
		/// </summary>
		/// <returns></returns>
		public ISession CreateSession(SessionSetupInfo setupInfo = null) {
			throw new System.NotImplementedException("it's stub factory implementation");
		}

		/// <summary>
		/// ������� �������
		/// </summary>
		/// <returns></returns>
		public IQuery CreateQuery(QuerySetupInfo setupInfo = null) {
			throw new System.NotImplementedException("it's stub factory implementation");
		}

		/// <summary>
		/// ������� RowHandler
		/// </summary>
		/// <returns></returns>
		public IRowHandler CreateRowHandler() {
			throw new System.NotImplementedException("it's stub factory implementation");
		}

		/// <summary>
		/// ������� ColumnHandler
		/// </summary>
		/// <returns></returns>
		public IColumnHandler CreateColumnHandler() {
			throw new System.NotImplementedException("it's stub factory implementation");
		}

		/// <summary>
		/// ������� ObjHandler
		/// </summary>
		/// <returns></returns>
		public IObjHandler CreateObjHandler() {
			throw new System.NotImplementedException("it's stub factory implementation");
		}

		/// <summary>
		/// ������� TimeHandler
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
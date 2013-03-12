using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme {
	/// <summary>
	/// Фабрика классов Extreme
	/// </summary>
	public class DefaultExtremeFactory : IExtremeFactory {
		
		/// <summary>
		/// Создает сессии
		/// </summary>
		/// <returns></returns>
		public ISession CreateSession(SessionSetupInfo setupInfo = null) {
			setupInfo = setupInfo ?? new SessionSetupInfo();
			return new Session(setupInfo.CollectStatistics);
		}

		/// <summary>
		/// Создает запросы
		/// </summary>
		/// <returns></returns>
		public IQuery CreateQuery(QuerySetupInfo setupInfo = null) {
			var result = new Query();
			if(null!=setupInfo) {
				result.Row = setupInfo.Row;
				result.Col = setupInfo.Col;
				result.Obj = setupInfo.Obj;
				result.Time = setupInfo.Time;
				result.Valuta = setupInfo.Valuta;
			}
			return result;
		}

		/// <summary>
		/// Создает RowHandler
		/// </summary>
		/// <returns></returns>
		public IRowHandler CreateRowHandler() {
			return new RowHandler();
		}

		/// <summary>
		/// Создает ColumnHandler
		/// </summary>
		/// <returns></returns>
		public IColumnHandler CreateColumnHandler() {
			return new ColumnHandler();
		}

		/// <summary>
		/// Создает ObjHandler
		/// </summary>
		/// <returns></returns>
		public IObjHandler CreateObjHandler() {
			return new ObjHandler();
		}

		/// <summary>
		/// Создает TimeHandler
		/// </summary>
		/// <returns></returns>
		public ITimeHandler CreateTimeHandler() {
			return new TimeHandler();
		}

		/// <summary>
		/// Акцессор к хранилищу формул
		/// </summary>
		/// <returns></returns>
		public IFormulaStorage GetFormulaStorage() {
			return FormulaStorage.Default;
		}
	}
}
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme {
	/// <summary>
	/// ������� ������� Extreme
	/// </summary>
	public class DefaultExtremeFactory : IExtremeFactory {
		
		/// <summary>
		/// ������� ������
		/// </summary>
		/// <returns></returns>
		public ISession CreateSession(SessionSetupInfo setupInfo = null) {
			setupInfo = setupInfo ?? new SessionSetupInfo();
			return new Session(setupInfo.CollectStatistics);
		}

		/// <summary>
		/// ������� �������
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
		/// ������� RowHandler
		/// </summary>
		/// <returns></returns>
		public IRowHandler CreateRowHandler() {
			return new RowHandler();
		}

		/// <summary>
		/// ������� ColumnHandler
		/// </summary>
		/// <returns></returns>
		public IColumnHandler CreateColumnHandler() {
			return new ColumnHandler();
		}

		/// <summary>
		/// ������� ObjHandler
		/// </summary>
		/// <returns></returns>
		public IObjHandler CreateObjHandler() {
			return new ObjHandler();
		}

		/// <summary>
		/// ������� TimeHandler
		/// </summary>
		/// <returns></returns>
		public ITimeHandler CreateTimeHandler() {
			return new TimeHandler();
		}

		/// <summary>
		/// �������� � ��������� ������
		/// </summary>
		/// <returns></returns>
		public IFormulaStorage GetFormulaStorage() {
			return FormulaStorage.Default;
		}
	}
}
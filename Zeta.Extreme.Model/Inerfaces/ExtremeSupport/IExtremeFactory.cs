namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// ����������� ������� �������� � ������
	/// </summary>
	public interface IExtremeFactory {
		/// <summary>
		/// ������� ������
		/// </summary>
		/// <returns></returns>
		ISession CreateSession(SessionSetupInfo setupInfo = null);
		/// <summary>
		/// ������� �������
		/// </summary>
		/// <returns></returns>
		IQuery CreateQuery(QuerySetupInfo setupInfo = null);

		/// <summary>
		/// ������� RowHandler
		/// </summary>
		/// <returns></returns>
		IRowHandler CreateRowHandler();
		/// <summary>
		/// ������� ColumnHandler
		/// </summary>
		/// <returns></returns>
		IColumnHandler CreateColumnHandler();

		/// <summary>
		/// ������� ObjHandler
		/// </summary>
		/// <returns></returns>
		IObjHandler CreateObjHandler();


		/// <summary>
		/// ������� TimeHandler
		/// </summary>
		/// <returns></returns>
		ITimeHandler CreateTimeHandler();
		/// <summary>
		/// �������� � ��������� ������
		/// </summary>
		/// <returns></returns>
		IFormulaStorage GetFormulaStorage();
	}
}
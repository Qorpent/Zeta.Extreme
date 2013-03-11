namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// ��������� ������� ��� ��������
	/// </summary>
	public class QuerySetupInfo {
		/// <summary>
		/// 
		/// </summary>
		public QuerySetupInfo() {
			Row = ExtremeFactory.CreateRowHandler();
			Col = ExtremeFactory.CreateColumnHandler();
			Obj = ExtremeFactory.CreateObjHandler();
			Time = ExtremeFactory.CreateTimeHandler();
		}
		/// <summary>
		/// ��������� ������
		/// </summary>
		public IRowHandler Row { get;  set; }
		/// <summary>
		/// ��������� �������
		/// </summary>
		public IColumnHandler Col { get; set; }
		/// <summary>
		/// ��������� �������
		/// </summary>
		public IObjHandler Obj { get; set; }
		/// <summary>
		/// ��������� �������
		/// </summary>
		public ITimeHandler Time { get;  set; }
		/// <summary>
		/// ��������� ������
		/// </summary>
		public string Valuta { get; set; }
	}
}
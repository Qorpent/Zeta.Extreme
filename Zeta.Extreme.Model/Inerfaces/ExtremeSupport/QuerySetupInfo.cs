namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// Настройки запроса при создании
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
		/// Описатель строки
		/// </summary>
		public IRowHandler Row { get;  set; }
		/// <summary>
		/// Описатель колонки
		/// </summary>
		public IColumnHandler Col { get; set; }
		/// <summary>
		/// Описатель объекта
		/// </summary>
		public IObjHandler Obj { get; set; }
		/// <summary>
		/// Описатель времени
		/// </summary>
		public ITimeHandler Time { get;  set; }
		/// <summary>
		/// Описатель валюты
		/// </summary>
		public string Valuta { get; set; }
	}
}
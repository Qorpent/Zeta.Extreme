namespace Comdiv.Zeta.Model.ExtremeSupport {
	/// <summary>
	/// Интерфейс описателя запроса
	/// </summary>
	public interface IQuery :IWithCacheKey{
		/// <summary>
		/// 	Условие на время
		/// </summary>
		ITimeHandler Time { get; set; }

		/// <summary>
		/// 	Условие на строку
		/// </summary>
		IRowHandler Row { get; set; }

		/// <summary>
		/// 	Условие на колонку
		/// </summary>
		IColumnHandler Col { get; set; }

		/// <summary>
		/// 	Условие на объект
		/// </summary>
		IObjHandler Obj { get; set; }

		/// <summary>
		/// 	Выходная валюта
		/// </summary>
		string Valuta { get; set; }
	}
}
using Qorpent.Serialization;

namespace Zeta.Extreme.Form {
	/// <summary>
	/// Структура, для представления ячейки в форме ввода
	/// </summary>
	public class OutCell {
		/// <summary>
		/// 	Ссылка на Id ячейки в БД
		/// </summary>
		[SerializeNotNullOnly] public int c;

		/// <summary>
		/// 	Признак значения, которое может быть целью сохранения
		/// </summary>
		[IgnoreSerialize] public bool canbefilled;

		/// <summary>
		/// 	Уникальный ИД ячейки
		/// </summary>
		public string i;

		/// <summary>
		/// 	Позволяет связать две ячейки в разных наборах
		/// </summary>
		[IgnoreSerialize] public OutCell linkedcell;

		/// <summary>
		/// 	Значение ячейки
		/// </summary>
		[SerializeNotNullOnly] public string v;

		/// <summary>
		/// Реальный ключ ячейки
		/// </summary>
		[SerializeNotNullOnly]public string ri;

		/// <summary>
		/// 	Ссылка на запрос для заполняемых значений
		/// </summary>
		[IgnoreSerialize] public IQuery query;
	}
}